using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npoi.Mapper;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace VAllens.LookupTable
{
    public partial class FrmMain : Form
    {
        #region 字段/属性

        /// <summary>
        /// 不查找目录
        /// </summary>
        private bool _isNotFindFolder;

        #endregion

        #region 构造函数

        public FrmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region 窗体/控件事件

        private void FrmMain_Load(object sender, EventArgs e)
        {
#if DEBUG
            txtMainDbConnectionStrings.Text = "Data Source=172.20.3.31,3341;Initial Catalog=MainDb;User ID=sa;Password=123456";
            txtSlaveDbConnectionStrings.Text = "Data Source=172.20.3.31,3341;Initial Catalog=SlaveDb;User ID=sa;Password=123456";
            txtFindFolder.Text = "D:\\works\\projects\\SlnTest\\ProjectTest";
            _isNotFindFolder = Properties.Settings.Default.IsNotFindFolder;
            cbxNotFindFolder_Click(cbxNotFindFolder, EventArgs.Empty);
#else
            txtMainDbConnectionStrings.Text = Properties.Settings.Default.MainDbConnectionStrings;
            txtSlaveDbConnectionStrings.Text = Properties.Settings.Default.SlaveDbConnectionStrings;
            txtFindFolder.Text = Properties.Settings.Default.FindFolder;
            _isNotFindFolder = Properties.Settings.Default.IsNotFindFolder;
            cbxNotFindFolder_Click(cbxNotFindFolder, EventArgs.Empty);
#endif

            //启动日志获取
            lbxLog.Start();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //持久化配置信息
            Properties.Settings.Default.MainDbConnectionStrings = txtMainDbConnectionStrings.Text;
            Properties.Settings.Default.SlaveDbConnectionStrings = txtSlaveDbConnectionStrings.Text;
            Properties.Settings.Default.FindFolder = txtFindFolder.Text;
            Properties.Settings.Default.IsNotFindFolder = cbxNotFindFolder.Checked;

            Properties.Settings.Default.Save();

            Environment.Exit(0);
        }

        private void btnTestDbConnection_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dicts = new Dictionary<string, string>
            {
                {"目标连接: ", txtMainDbConnectionStrings.Text.Trim()},
                {"扫描连接: ", txtSlaveDbConnectionStrings.Text.Trim()}
            };

            foreach (var dict in dicts)
            {
                try
                {
                    new DbHelper(dict.Value).TestDbConnection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(dict.Key + ex.Message, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }

            MessageBox.Show("连接测试成功！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "请选择项目文件夹",
                ShowNewFolderButton = false
            };

            bool faild;
            do
            {
                DialogResult dialogResult = folderBrowserDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                {
                    if (Directory.Exists(folderBrowserDialog.SelectedPath))
                    {
                        txtFindFolder.Text = folderBrowserDialog.SelectedPath;
                        faild = false;
                    }
                    else
                    {
                        MessageBox.Show("该路径不存在，请选择一个有效的文件夹！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        faild = true;
                    }
                }
                else
                {
                    faild = false;
                }
            } while (faild);
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            DbHelper mainDbHelper = new DbHelper(txtMainDbConnectionStrings.Text.Trim());
            DbHelper slaveDbHelper = new DbHelper(txtSlaveDbConnectionStrings.Text.Trim());

            #region 校验

            try
            {
                mainDbHelper.TestDbConnection();
                slaveDbHelper.TestDbConnection();
            }
            catch
            {
                MessageBox.Show("请检查数据库连接字符串，并且使其连接测试通过！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (txtFindFolder.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入要查找的项目文件夹！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            if (!Directory.Exists(txtFindFolder.Text.Trim()))
            {
                MessageBox.Show("该文件夹不存在，请输入一个有效的文件夹路径！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            #endregion

            //清空日志列表
            lbxLog.Items.Clear();
            ConcurrentQueue<SqlAnalysis> analysisQueue = new ConcurrentQueue<SqlAnalysis>();

            Thread thread = new Thread(() =>
            {
                try
                {
                    btnExec.Invoke((MethodInvoker) delegate { btnExec.Enabled = false; });

                    #region 预热数据源

                    Stopwatch totalStopWatch = new Stopwatch();
                    totalStopWatch.Start();

                    lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 开始预热数据源...", DateTime.Now));

                    //文件后缀名
                    const string extensions = ".cs";
                    List<string> matchFiles = new List<string>();
                    Dictionary<string, Regex> tableNameRegexDicts = new Dictionary<string, Regex>();
                    List<SqlModule> sqlModuleList = new List<SqlModule>();
                    Dictionary<string, string> tableNameDescDicts = new Dictionary<string, string>();

                    string GetDescByTableName(string tableName) => tableNameDescDicts.ContainsKey(tableName)
                        ? tableNameDescDicts[tableName]
                        : string.Empty;

                    //查找要匹配的文件
                    Task findFileTask = new Task(() =>
                    {
                        Stopwatch stopWatch = new Stopwatch();

                        List<string> excludedFiles = new List<string>();

                        #region 查找要匹配的扩展名文件

                        stopWatch.Watch(() =>
                        {
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 开始查找匹配的文件...", DateTime.Now));
                            DirectoryHelper.GetFilesOfRecursionFolder(txtFindFolder.Text, matchFiles, excludedFiles,
                                extensions);
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 结束查找匹配的文件...", DateTime.Now));
                        }, lbxLog, "查找匹配的文件");

                        if (matchFiles.Any())
                        {
                            lbxLog.AddItem(string.Format("扫描到共 {0} 个记录匹配的文件，如下: ", matchFiles.Count));
                            matchFiles.ForEach(filePath => lbxLog.AddItem("匹配的文件: " + filePath));
                        }
                        else
                        {
                            lbxLog.AddItem("没有匹配的文件！");
                        }

                        if (excludedFiles.Any())
                        {
                            lbxLog.AddItem(string.Format("扫描到共 {0} 个记录忽略的文件，如下: ", excludedFiles.Count));
                            excludedFiles.ForEach(filePath => lbxLog.AddItem("忽略的文件: " + filePath));
                        }
                        else
                        {
                            lbxLog.AddItem("没有忽略的文件！");
                        }

                        #endregion
                    });

                    //从数据库获取表名称
                    Task getTableNameFormDbTask = new Task(() =>
                    {
                        Stopwatch stopWatch = new Stopwatch();

                        #region 获取数据库的表名称

                        List<string> tmpTableNameList = new List<string>();
                        stopWatch.Watch(() =>
                        {
                            const string selectTableNamesSql =
                                "SELECT name FROM sysobjects WHERE xtype='u' AND name != 'sysdiagrams'";
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 开始获取数据库的表名称...", DateTime.Now));
                            tmpTableNameList = mainDbHelper.GetData<string>(selectTableNamesSql).ToList();
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 结束获取数据库的表名称...", DateTime.Now));
                        }, lbxLog, "始获取数据库的表名称");

                        if (tmpTableNameList.Any())
                        {
                            lbxLog.AddItem(string.Format("查询到共 {0} 个表名称记录，如下: ", tmpTableNameList.Count));

                            tmpTableNameList.ForEach(tableName =>
                            {
                                lbxLog.AddItem("**** 表名: " + tableName);
                                //预热正则表达式
                                tableNameRegexDicts[tableName] =
                                    new Regex(string.Format("(\\(|\\[| |,|\\.|'){0}(\\)|\\]| |,|\\.|')", tableName),
                                        RegexOptions.IgnoreCase | RegexOptions.Compiled);
                            });

                            lbxLog.AddItem("预热 表名正则表达式 完成");
                        }
                        else
                        {
                            lbxLog.AddItem("没有查询到表名称，你可能没有权限！");
                        }

                        #endregion
                    });

                    //从数据库获取SQL模块(视图，存储过程，函数等)
                    Task getSqlModuleFormDbTask = new Task(() =>
                    {
                        Stopwatch stopWatch = new Stopwatch();

                        #region 获取数据库的SQL模块信息

                        stopWatch.Watch(() =>
                        {
                            const string selectModulesSql =
                                "SELECT sm.object_id as ObjectId, OBJECT_NAME(sm.object_id) AS ObjectName, o.type as TypeCode, o.type_desc as TypeDesc, sm.definition as Definition FROM sys.sql_modules AS sm JOIN sys.objects AS o ON sm.object_id = o.object_id ORDER BY o.type";
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 开始获取数据库的SQL模块(视图，存储过程，函数)...",
                                DateTime.Now));
                            sqlModuleList = slaveDbHelper.GetData<SqlModule>(selectModulesSql).ToList();
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 结束获取数据库的SQL模块(视图，存储过程，函数)...",
                                DateTime.Now));
                        }, lbxLog, "获取数据库的SQL模块(视图，存储过程，函数)");

                        if (sqlModuleList.Any())
                        {
                            lbxLog.AddItem(string.Format("查询到共 {0} 个SQL模块(视图，存储过程，函数)记录。", sqlModuleList.Count));
                            sqlModuleList.GroupBy(t => t.TypeName).ToList().ForEach(moduleGroup =>
                            {
                                lbxLog.AddItem(string.Format("**** 模块类型: {0}, 共{1}个记录，如下: ", moduleGroup.Key,
                                    moduleGroup.Count()));
                                foreach (var sqlModule in moduleGroup)
                                {
                                    lbxLog.AddItem(string.Format("**** 模块类型: {0}, 模块名称: {1}", sqlModule.TypeName,
                                        sqlModule.ObjectName));
                                }
                            });
                        }
                        else
                        {
                            lbxLog.AddItem("没有查询到SQL模块(视图，存储过程，函数)，你可能没有权限！");
                        }

                        #endregion
                    });

                    Task getTableDescFormDbTask = new Task(() =>
                    {
                        Stopwatch stopWatch = new Stopwatch();

                        #region 获取数据库的表描述信息

                        /*
                            SELECT * FROM dbo.TS_DataSetInfo
                            SELECT * FROM dbo.TS_TStuctMaster
                            SELECT * FROM dbo.TS_TStuctDetail WHERE ColumnDesc LIKE N'%包车%'
                        */

                        stopWatch.Watch(() =>
                        {
                            const string selectTableDescSql =
                                "SELECT DISTINCT TableID AS [Key],TableName AS [Value] FROM dbo.TS_TStuctMaster";
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 开始获取数据库的表描述信息...", DateTime.Now));
                            tableNameDescDicts = mainDbHelper.GetData<KeyValuePair<string, string>>(selectTableDescSql)
                                .ToDictionary(pair => pair.Key, pair => pair.Value);
                            lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 结束获取数据库的表描述信息...", DateTime.Now));
                        }, lbxLog, "获取数据库的表描述信息");

                        if (tableNameDescDicts.Any())
                        {
                            lbxLog.AddItem(string.Format("查询到共 {0} 个表描述信息记录。", tableNameDescDicts.Count));
                            foreach (var tableNameDesc in tableNameDescDicts)
                            {
                                lbxLog.AddItem(string.Format("**** 表名: {0}, 表描述: {1}", tableNameDesc.Key,
                                    tableNameDesc.Value));
                            }
                        }
                        else
                        {
                            lbxLog.AddItem("没有查询到表描述信息记录，你可能没有权限！");
                        }

                        #endregion
                    });

                    List<Task> tasks = new List<Task>
                    {
                        getTableNameFormDbTask,
                        getSqlModuleFormDbTask,
                        getTableDescFormDbTask
                    };

                    if (!cbxNotFindFolder.Checked)
                    {
                        tasks.Insert(0, findFileTask);
                    }

                    //批量启动任务
                    tasks.ForEach(task => task.Start());

                    //等待所有任务完成
                    Task.WaitAll(tasks.ToArray());

                    lbxLog.AddItem(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} 预热数据源完成...", DateTime.Now));

                    totalStopWatch.Stop();
                    totalStopWatch.WriteElapsedLog(lbxLog, "预热数据源");

                    #endregion

                    if (!matchFiles.Any() && !cbxNotFindFolder.Checked)
                    {
                        lbxLog.AddItem("执行完成！");
                        return;
                    }

                    if (!tableNameRegexDicts.Any())
                    {
                        lbxLog.AddItem("你可能没有数据库权限，请改用其它数据库账户访问");
                        lbxLog.AddItem("执行完成！");
                        return;
                    }

                    #region CSharp文件 语法分析

                    ConcurrentDictionary<string, List<string>> findTableNameDicts =
                        new ConcurrentDictionary<string, List<string>>();
                    //如果 不查目录 没有选中
                    if (!cbxNotFindFolder.Checked)
                    {
                        Parallel.ForEach(matchFiles, file =>
                        {
                            //获取C#文件的所有字符串
                            IEnumerable<string> literals = CSharpSyntaxHelper.ExtractStringLiterals(file);

                            //匹配所有表名称
                            List<string> findTableNameList = tableNameRegexDicts
                                .Where(kv => literals.Any(s => kv.Value.IsMatch(s))).Select(kv => kv.Key).ToList();

                            if (findTableNameList.Any())
                            {
                                findTableNameList.ForEach(tableName =>
                                {
                                    analysisQueue.Enqueue(new SqlAnalysis
                                    {
                                        ObjectSource = ObjectSource.File,
                                        ObjectName = file,
                                        FoundTableName = tableName,
                                        FoundTableDesc = GetDescByTableName(tableName),
                                        IsMatch = true
                                    });

                                    lbxLog.AddItem(string.Format("在 {0} 文件中，找到表名称 {1}", file, tableName));
                                });

                                lbxLog.AddItem(string.Format("在 {0} 文件中，总计找到 {1} 个表名称", file, findTableNameList.Count));

                                findTableNameDicts.TryAdd(file,
                                    findTableNameList.Distinct(new CaseInsensitiveComparer<string>()).ToList());
                            }
                        });

                        lbxLog.AddItem(string.Format("项目文件分析完成，总计在 {0} 个文件总找到 {1} 个表名称", findTableNameDicts.Keys.Count,
                            findTableNameDicts.Values.SelectMany(t => t).Distinct(new CaseInsensitiveComparer<string>())
                                .Count()));
                    }

                    #endregion

                    #region SQL模块 语法分析

                    //CSharp文件中匹配到的表名
                    //var tableNameDicts = findTableNameDicts.Values.SelectMany(t => t).Distinct(new CaseInsensitiveComparer<string>()).Select(t => tableNameRegexDicts.First(s => s.Key.Equals(t, StringComparison.CurrentCultureIgnoreCase))).OrderByDescending(t => t.Key).ToDictionary(t => t.Key, t => t.Value);
                    //主库上的表名
                    var tableNameDicts = tableNameRegexDicts;

                    var findSqlModuleList = new ConcurrentDictionary<string, List<string>>();
                    if (tableNameDicts.Any() && sqlModuleList.Any())
                    {
                        Parallel.ForEach(sqlModuleList.ToArray(), sqlModule =>
                        {
                            //获取SQL模块中的所有表名称
                            List<string> tableNames = TSqlTableFinder.GetTableNames(sqlModule.Definition)
                                .Select(SubStringTableName).Where(t => !t.StartsWith("#")).ToList();

                            //匹配的表名称
                            //List<string> findTableNameList = tableNameDicts.Keys.Intersect(tableNames, t => t).Distinct(new CaseInsensitiveComparer<string>()).ToList();
                            if (tableNames.Any())
                            {
                                tableNames.ForEach(tableName =>
                                {
                                    analysisQueue.Enqueue(new SqlAnalysis
                                    {
                                        ObjectSource = ObjectSource.Db,
                                        ObjectName = sqlModule.ObjectName,
                                        TypeCode = sqlModule.TypeCode,
                                        FoundTableName = tableName,
                                        FoundTableDesc = GetDescByTableName(tableName),
                                        IsMatch = false
                                    });

                                    lbxLog.AddItem(string.Format("在 {0} {1} 中，找到表名称 {2}", sqlModule.ObjectName,
                                        sqlModule.TypeName, tableName));
                                });

                                lbxLog.AddItem(string.Format("在 {0} {1} 中，总计找到 {2} 个表名称", sqlModule.ObjectName,
                                    sqlModule.TypeName, tableNames.Count));

                                findSqlModuleList.TryAdd(
                                    string.Format("{0}|{1}", sqlModule.TypeCode, sqlModule.ObjectName),
                                    tableNames.Distinct(new CaseInsensitiveComparer<string>()).ToList());
                            }
                        });
                    }

                    #endregion

                    lbxLog.AddItem("正在导出分析结果到Excel...");
                    ExportExcelByAnalysisData(analysisQueue);
                    lbxLog.AddItem("导出分析结果到Excel完成！");

                    lbxLog.AddItem("执行完成！");
                    MessageBox.Show("执行完成！");
                }
                catch (Exception ex)
                {
                    lbxLog.AddItem("发生异常: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        lbxLog.AddItem("内部异常: " + ex.InnerException.Message);
                    }
                }
                finally
                {
                    btnExec.Invoke((MethodInvoker) delegate { btnExec.Enabled = true; });
                }
            });

            thread.IsBackground = true;

            thread.Start();
        }

        private void btnExportLogs_Click(object sender, EventArgs e)
        {
            if (!lbxLog.Items.Cast<string>().Any())
            {
                MessageBox.Show("无任何日志需要导出！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                CreatePrompt = false,
                OverwritePrompt = false,
                AddExtension = true,
                CheckFileExists = false,
                DefaultExt = "log",
                FileName = string.Format("Log_{0:yyyyMMddHHmmssfff}", DateTime.Now),
                Title = "请选择保存日志文件的目标文件夹",
                Filter = "日志文件|*.log,*.txt"
            };

            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
            {
                string txtContent = string.Join("\r\n", lbxLog.Items.Cast<string>());
                File.WriteAllText(saveFileDialog.FileName, txtContent, Encoding.UTF8);
                Process.Start(saveFileDialog.FileName);
                MessageBox.Show("导出成功！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lbxLog.Items.Clear();
            }
        }

        private void cbxNotFindFolder_Click(object sender, EventArgs e)
        {
            var cbx = (CheckBox) sender;
            cbx.Checked = _isNotFindFolder;

            _isNotFindFolder = !_isNotFindFolder;

            txtFindFolder.Enabled = _isNotFindFolder;
            BtnOpenFolder.Enabled = _isNotFindFolder;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取全限定表名的简写/实际表名
        /// </summary>
        /// <param name="fullTableName">全限定表名</param>
        /// <returns></returns>
        private static string SubStringTableName(string fullTableName)
        {
            int lastIndex = fullTableName.LastIndexOf(".", StringComparison.Ordinal);

            string subTableName = lastIndex > 0 ? fullTableName.Substring(lastIndex + 1) : fullTableName;

            return subTableName.Replace("[", "").Replace("]", "");
        }

        /// <summary>
        /// 导出Excel并且使用默认程序打开
        /// </summary>
        /// <param name="data"></param>
        private static void ExportExcelByAnalysisData(IEnumerable<SqlAnalysis> data)
        {
            var mapper = new Mapper();

            #region 设置表格样式

            mapper.ForHeader(cell =>
            {
                IWorkbook workbook = cell.Sheet.Workbook;

                IFont font = workbook.CreateFont();
                //字体大小
                font.FontHeightInPoints = 14;
                //字体加粗
                font.Boldweight = (short) FontBoldWeight.Bold;

                var style = workbook.CreateCellStyle();

                //字体设置
                style.SetFont(font);

                //设置单元格上下左右边框线
                style.BorderTop = style.BorderBottom =
                    style.BorderLeft = style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //填充背景样式
                style.FillPattern = FillPattern.SolidForeground;
                //填充背景色
                style.FillForegroundColor = HSSFColor.Yellow.Index;
                //文字水平和垂直对齐方式
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;

                cell.CellStyle = style;

                //自动列宽
                cell.Row.Sheet.AutoSizeColumn(cell.ColumnIndex);
            });

            #endregion

            var sourceGroups = data.GroupBy(k => k.ObjectSource);

            foreach (var sourceGroup in sourceGroups)
            {
                var dist = sourceGroup.Distinct(new CommonEqualityComparer<SqlAnalysis, string>(t => t.FoundTableName,
                    new CaseInsensitiveComparer<string>()));
                foreach (var sqlAnalysis in dist)
                {
                    sqlAnalysis.Count = sourceGroup.Count(t =>
                        t.FoundTableName.Equals(sqlAnalysis.FoundTableName, StringComparison.OrdinalIgnoreCase));
                }

                mapper.Put(dist, GetSheetNameByObjectSource(sourceGroup.Key));
            }

            mapper.Ignore<SqlAnalysis>(t => t.Count);
            mapper.Put(data, "分析结果明细");

            List<SqlAnalysisBase> objectAnalysis = data.GroupBy(t => new
            {
                t.ObjectSource,
                t.ObjectName
            }).Select(t => new SqlAnalysisBase
            {
                ObjectSource = t.Key.ObjectSource,
                ObjectName = t.Key.ObjectName,
                Count = t.Count()
            }).ToList();

            mapper.Ignore<SqlAnalysisBase>(t => t.TypeName);
            mapper.Put(objectAnalysis, "分析结果统计");

            string excelFileName = string.Format("SqlAnalysis_{0:yyyyMMddHHmmssfff}.xlsx", DateTime.Now);
            mapper.Save(excelFileName);
            Process.Start(excelFileName);
        }

        /// <summary>
        /// 根据对象来源获取表格名称
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string GetSheetNameByObjectSource(ObjectSource source)
        {
            switch (source)
            {
                case ObjectSource.File:
                    return "CSharp文件分析结果";
                case ObjectSource.Db:
                    return "数据库分析结果";
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }

        #endregion
    }
}