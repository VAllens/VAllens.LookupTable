using Npoi.Mapper.Attributes;

namespace VAllens.LookupTable
{
    public class SqlAnalysisBase
    {
        public SqlAnalysisBase()
        {
            TypeCode = "U";
        }

        /// <summary>
        /// 类型代码
        /// </summary>
        private string _typeCode;

        /// <summary>
        /// 类型代码
        /// </summary>
        [Ignore]
        public virtual string TypeCode
        {
            get => _typeCode;
            set => _typeCode = value?.Trim();
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Column("类型名称")]
        public virtual string TypeName
        {
            get
            {
                switch (TypeCode)
                {
                    case "TR":
                        return "触发器";
                    case "FN":
                        return "函数";
                    case "P":
                        return "存储过程";
                    case "TF":
                        return "表值函数";
                    case "IF":
                        return "内联表值函数";
                    case "V":
                        return "视图";
                    case "U":
                        return "表";
                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// 对象来源
        /// </summary>
        [Column("对象来源")]
        public virtual ObjectSource ObjectSource { get; set; }

        /// <summary>
        /// 文件名或SQL模块名称
        /// </summary>
        [Column("对象名称")]
        public virtual string ObjectName { get; set; }

        /// <summary>
        /// 出现的次数
        /// </summary>
        [Column("计数")]
        public virtual int Count { get; set; }
    }
}