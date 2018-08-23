using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace VAllens.LookupTable
{
    /// <summary>
    /// T-SQL的Table对象发现者
    /// </summary>
    public class TSqlTableFinder
    {
        public static List<string> GetTableNames(string sqlScripts, SqlVersion version = SqlVersion.Sql130,
            bool initialQuotedIdentifiers = true)
        {
            var result = new List<string>();

            var sb = new StringBuilder();
            var parser = TSqlParserFactory.Create(version, initialQuotedIdentifiers);

            var fromTokenTypes = new[]
            {
                TSqlTokenType.From,
                TSqlTokenType.Join
            };

            var identifierTokenTypes = new[]
            {
                TSqlTokenType.Identifier,
                TSqlTokenType.QuotedIdentifier
            };

            using (TextReader txtReader = new StringReader(sqlScripts))
            {
                IList<ParseError> errors;
                var queryTokens = parser.GetTokenStream(txtReader, out errors);
                if (errors.Any())
                {
                    return errors
                        .Select(e => string.Format("Error: {0}; Line: {1}; Column: {2}; Offset: {3};  Message: {4};",
                            e.Number, e.Line, e.Column, e.Offset, e.Message))
                        .ToList();
                }

                for (var i = 0; i < queryTokens.Count; i++)
                {
                    if (fromTokenTypes.Contains(queryTokens[i].TokenType))
                    {
                        for (var j = i + 1; j < queryTokens.Count; j++)
                        {
                            if (queryTokens[j].TokenType == TSqlTokenType.WhiteSpace)
                            {
                                continue;
                            }

                            if (identifierTokenTypes.Contains(queryTokens[j].TokenType))
                            {
                                sb.Clear();
                                GetQuotedIdentifier(queryTokens[j], sb);

                                while (j + 2 < queryTokens.Count
                                       && queryTokens[j + 1].TokenType == TSqlTokenType.Dot
                                       && (queryTokens[j + 2].TokenType == TSqlTokenType.Dot ||
                                           identifierTokenTypes.Contains(queryTokens[j + 2].TokenType)))
                                {
                                    sb.Append(queryTokens[j + 1].Text);

                                    if (queryTokens[j + 2].TokenType == TSqlTokenType.Dot)
                                    {
                                        if (queryTokens[j - 1].TokenType == TSqlTokenType.Dot)
                                            GetQuotedIdentifier(queryTokens[j + 1], sb);

                                        j++;
                                    }
                                    else
                                    {
                                        GetQuotedIdentifier(queryTokens[j + 2], sb);
                                        j += 2;
                                    }
                                }

                                result.Add(sb.ToString());
                            }

                            break;
                        }
                    }
                }

                return result.Distinct().OrderBy(tableName => tableName).ToList();
            }
        }

        private static void GetQuotedIdentifier(TSqlParserToken token, StringBuilder sb)
        {
            switch (token.TokenType)
            {
                case TSqlTokenType.Identifier:
                    sb.Append('[').Append(token.Text).Append(']');
                    break;
                case TSqlTokenType.QuotedIdentifier:
                case TSqlTokenType.Dot:
                    sb.Append(token.Text);
                    break;

                default:
                    throw new ArgumentException(
                        "Error: expected TokenType of token should be TSqlTokenType.Dot, TSqlTokenType.Identifier, or TSqlTokenType.QuotedIdentifier");
            }
        }
    }
}