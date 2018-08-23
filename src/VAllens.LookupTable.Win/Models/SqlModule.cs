namespace VAllens.LookupTable
{
    public class SqlModule
    {
        public virtual int ObjectId { get; set; }

        public virtual string ObjectName { get; set; }

        private string _typeCode;

        public virtual string TypeCode
        {
            get => _typeCode;
            set => _typeCode = value?.Trim();
        }

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

        public virtual string TypeDesc { get; set; }

        public virtual string Definition { get; set; }
    }
}