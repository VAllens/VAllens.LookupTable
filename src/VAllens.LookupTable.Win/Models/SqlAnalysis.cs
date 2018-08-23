using Npoi.Mapper.Attributes;

namespace VAllens.LookupTable
{
    public class SqlAnalysis : SqlAnalysisBase
    {
        /// <summary>
        /// 找到的表名称
        /// </summary>
        [Column("表名称")]
        public virtual string FoundTableName { get; set; }

        /// <summary>
        /// 找到的表描述
        /// </summary>
        [Column("表描述")]
        public virtual string FoundTableDesc { get; set; }

        /// <summary>
        /// 找到的表名是否匹配
        /// </summary>
        [Ignore]
        public virtual bool IsMatch { get; set; }
    }
}