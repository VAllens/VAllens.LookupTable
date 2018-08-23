namespace VAllens.LookupTable
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSlaveDbConnectionStrings = new System.Windows.Forms.TextBox();
            this.lblSlaveDb = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtFindFolder = new System.Windows.Forms.TextBox();
            this.btnExec = new System.Windows.Forms.Button();
            this.lbxLog = new System.Windows.Forms.ListBox();
            this.btnExportLogs = new System.Windows.Forms.Button();
            this.BtnOpenFolder = new System.Windows.Forms.Button();
            this.btnTestDbConnection = new System.Windows.Forms.Button();
            this.lblMainDb = new System.Windows.Forms.Label();
            this.txtMainDbConnectionStrings = new System.Windows.Forms.TextBox();
            this.cbxNotFindFolder = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtSlaveDbConnectionStrings
            // 
            this.txtSlaveDbConnectionStrings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSlaveDbConnectionStrings.Location = new System.Drawing.Point(113, 55);
            this.txtSlaveDbConnectionStrings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSlaveDbConnectionStrings.Name = "txtSlaveDbConnectionStrings";
            this.txtSlaveDbConnectionStrings.Size = new System.Drawing.Size(855, 31);
            this.txtSlaveDbConnectionStrings.TabIndex = 0;
            // 
            // lblSlaveDb
            // 
            this.lblSlaveDb.AutoSize = true;
            this.lblSlaveDb.Location = new System.Drawing.Point(5, 58);
            this.lblSlaveDb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSlaveDb.Name = "lblSlaveDb";
            this.lblSlaveDb.Size = new System.Drawing.Size(100, 24);
            this.lblSlaveDb.TabIndex = 1;
            this.lblSlaveDb.Text = "扫描链接：";
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(5, 99);
            this.lblFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(100, 24);
            this.lblFolder.TabIndex = 3;
            this.lblFolder.Text = "扫描目录：";
            // 
            // txtFindFolder
            // 
            this.txtFindFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFindFolder.Location = new System.Drawing.Point(113, 96);
            this.txtFindFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFindFolder.Name = "txtFindFolder";
            this.txtFindFolder.Size = new System.Drawing.Size(855, 31);
            this.txtFindFolder.TabIndex = 2;
            // 
            // btnExec
            // 
            this.btnExec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExec.Location = new System.Drawing.Point(998, 148);
            this.btnExec.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(100, 38);
            this.btnExec.TabIndex = 4;
            this.btnExec.Text = "执行";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // lbxLog
            // 
            this.lbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxLog.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxLog.ItemHeight = 20;
            this.lbxLog.Location = new System.Drawing.Point(113, 148);
            this.lbxLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbxLog.Name = "lbxLog";
            this.lbxLog.ScrollAlwaysVisible = true;
            this.lbxLog.Size = new System.Drawing.Size(855, 564);
            this.lbxLog.TabIndex = 5;
            // 
            // btnExportLogs
            // 
            this.btnExportLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportLogs.Location = new System.Drawing.Point(998, 674);
            this.btnExportLogs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportLogs.Name = "btnExportLogs";
            this.btnExportLogs.Size = new System.Drawing.Size(100, 38);
            this.btnExportLogs.TabIndex = 6;
            this.btnExportLogs.Text = "导出日志";
            this.btnExportLogs.UseVisualStyleBackColor = true;
            this.btnExportLogs.Click += new System.EventHandler(this.btnExportLogs_Click);
            // 
            // BtnOpenFolder
            // 
            this.BtnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOpenFolder.Location = new System.Drawing.Point(998, 92);
            this.BtnOpenFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtnOpenFolder.Name = "BtnOpenFolder";
            this.BtnOpenFolder.Size = new System.Drawing.Size(100, 38);
            this.BtnOpenFolder.TabIndex = 7;
            this.BtnOpenFolder.Text = "选择目录";
            this.BtnOpenFolder.UseVisualStyleBackColor = true;
            this.BtnOpenFolder.Click += new System.EventHandler(this.BtnOpenFolder_Click);
            // 
            // btnTestDbConnection
            // 
            this.btnTestDbConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestDbConnection.Location = new System.Drawing.Point(998, 51);
            this.btnTestDbConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTestDbConnection.Name = "btnTestDbConnection";
            this.btnTestDbConnection.Size = new System.Drawing.Size(100, 38);
            this.btnTestDbConnection.TabIndex = 8;
            this.btnTestDbConnection.Text = "连接测试";
            this.btnTestDbConnection.UseVisualStyleBackColor = true;
            this.btnTestDbConnection.Click += new System.EventHandler(this.btnTestDbConnection_Click);
            // 
            // lblMainDb
            // 
            this.lblMainDb.AutoSize = true;
            this.lblMainDb.Location = new System.Drawing.Point(5, 17);
            this.lblMainDb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainDb.Name = "lblMainDb";
            this.lblMainDb.Size = new System.Drawing.Size(100, 24);
            this.lblMainDb.TabIndex = 10;
            this.lblMainDb.Text = "目标链接：";
            // 
            // txtMainDbConnectionStrings
            // 
            this.txtMainDbConnectionStrings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMainDbConnectionStrings.Location = new System.Drawing.Point(113, 14);
            this.txtMainDbConnectionStrings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMainDbConnectionStrings.Name = "txtMainDbConnectionStrings";
            this.txtMainDbConnectionStrings.Size = new System.Drawing.Size(855, 31);
            this.txtMainDbConnectionStrings.TabIndex = 9;
            // 
            // cbxNotFindFolder
            // 
            this.cbxNotFindFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxNotFindFolder.AutoCheck = false;
            this.cbxNotFindFolder.AutoSize = true;
            this.cbxNotFindFolder.Location = new System.Drawing.Point(998, 15);
            this.cbxNotFindFolder.Name = "cbxNotFindFolder";
            this.cbxNotFindFolder.Size = new System.Drawing.Size(104, 28);
            this.cbxNotFindFolder.TabIndex = 11;
            this.cbxNotFindFolder.Text = "不查目录";
            this.cbxNotFindFolder.UseVisualStyleBackColor = true;
            this.cbxNotFindFolder.Click += new System.EventHandler(this.cbxNotFindFolder_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 731);
            this.Controls.Add(this.cbxNotFindFolder);
            this.Controls.Add(this.lblMainDb);
            this.Controls.Add(this.txtMainDbConnectionStrings);
            this.Controls.Add(this.btnTestDbConnection);
            this.Controls.Add(this.BtnOpenFolder);
            this.Controls.Add(this.btnExportLogs);
            this.Controls.Add(this.lbxLog);
            this.Controls.Add(this.btnExec);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.txtFindFolder);
            this.Controls.Add(this.lblSlaveDb);
            this.Controls.Add(this.txtSlaveDbConnectionStrings);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmMain";
            this.Text = "查找表引用依赖工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSlaveDbConnectionStrings;
        private System.Windows.Forms.Label lblSlaveDb;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox txtFindFolder;
        private System.Windows.Forms.Button btnExec;
        private System.Windows.Forms.ListBox lbxLog;
        private System.Windows.Forms.Button btnExportLogs;
        private System.Windows.Forms.Button BtnOpenFolder;
        private System.Windows.Forms.Button btnTestDbConnection;
        private System.Windows.Forms.Label lblMainDb;
        private System.Windows.Forms.TextBox txtMainDbConnectionStrings;
        private System.Windows.Forms.CheckBox cbxNotFindFolder;
    }
}

