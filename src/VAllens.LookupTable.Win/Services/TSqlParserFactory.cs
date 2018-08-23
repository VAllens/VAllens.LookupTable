using System;
using System.Globalization;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace VAllens.LookupTable
{
    /// <summary>
    /// T-SQL语法解析器工厂类
    /// <para>Microsoft.SqlServer.TransactSql.ScriptDom.SqlVersion: </para>
    /// <para>  Sql80:  SQL Server 2000 (8)</para>
    /// <para>  Sql90:  SQL Server 2005 (9)</para>
    /// <para>  Sql100: SQL Server 2008 (10), SQL Server 2008 R2 (10.5)</para>
    /// <para>  Sql110: SQL Server 2012 (11.x)</para>
    /// <para>  Sql120: SQL Server 2014 (12.x)</para>
    /// <para>  Sql130: SQL Server 2016 (13.x), Azure SQL Database 逻辑服务器 (12.x), Azure SQL Database 托管实例 (12.x)</para>
    /// <para>  Sql140: SQL Server 2017 (14.x)</para>
    /// <para>更多详情: </para>
    /// <para>https://docs.microsoft.com/zh-cn/sql/t-sql/statements/alter-database-transact-sql-compatibility-level</para>
    /// </summary>
    public static class TSqlParserFactory
    {
        /// <summary>
        /// 创建T-SQL语法解析器
        /// </summary>
        public static TSqlParser Create(SqlVersion tsqlParserVersion, bool initialQuotedIdentifiers = true)
        {
            switch (tsqlParserVersion)
            {
                case SqlVersion.Sql90:
                    return new TSql90Parser(initialQuotedIdentifiers);

                case SqlVersion.Sql80:
                    return new TSql80Parser(initialQuotedIdentifiers);

                case SqlVersion.Sql100:
                    return new TSql100Parser(initialQuotedIdentifiers);

                case SqlVersion.Sql110:
                    return new TSql110Parser(initialQuotedIdentifiers);

                case SqlVersion.Sql120:
                    return new TSql120Parser(initialQuotedIdentifiers);

                case SqlVersion.Sql130:
                    return new TSql130Parser(initialQuotedIdentifiers);

                case SqlVersion.Sql140:
                    return new TSql140Parser(initialQuotedIdentifiers);
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, "The value ({0}) provided for type ({1}) is unknown",
                            new object[] {tsqlParserVersion, "TSqlParserVersion"}), nameof(tsqlParserVersion));
            }
        }
    }
}