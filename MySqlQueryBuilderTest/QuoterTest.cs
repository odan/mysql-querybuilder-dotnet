using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MySqlQueryBuilder;
using Xunit;

namespace MySqlQueryBuilderTest
{
    public class QuoterTest : BaseTest
    {
        [Fact]
        public void TestQuoter()
        {
            Quoter quoter = new Quoter();

            Assert.Equal("NULL", quoter.Quote(null));
            Assert.Equal("'\\0'", quoter.Quote("\x00"));
            Assert.Equal("'0'", quoter.Quote(0));
            Assert.Equal("'0'", quoter.Quote("0"));
            Assert.Equal("'0'", quoter.Quote(false));
            Assert.Equal("'1'", quoter.Quote(true));
            Assert.Equal("'-1'", quoter.Quote(-1));
            Assert.Equal("'abc123'", quoter.Quote("abc123"));
            Assert.Equal("'öäüÖÄÜß'", quoter.Quote("öäüÖÄÜß"));
            Assert.Equal("'?'", quoter.Quote("?"));
            Assert.Equal("':'", quoter.Quote(":"));
            Assert.Equal("'\\''", quoter.Quote("'"));
            Assert.Equal("'\\\"'", quoter.Quote("\""));
            Assert.Equal("'\\\\'", quoter.Quote("\\"));
            Assert.Equal("'\\0'", quoter.Quote("\x00"));
            Assert.Equal("'\\Z'", quoter.Quote("\x1a"));
            Assert.Equal("'\\n'", quoter.Quote("\n"));
            Assert.Equal("'\\r'", quoter.Quote("\r"));
            Assert.Equal("','", quoter.Quote(","));
            Assert.Equal("'\\','", quoter.Quote("',"));
            //Assert.Equal("'`'", quoter.Quote("`"));
            Assert.Equal("'\\`'", quoter.Quote("`"));
            Assert.Equal("'%s'", quoter.Quote("%s"));
            //Assert.Equal("'\\%s'", quoter.Quote("%s"));
            Assert.Equal("'Naughty \\' string'", quoter.Quote("Naughty ' string"));
            Assert.Equal("'@þÿ€'", quoter.Quote("@þÿ€"));
            // Injection patterns
            Assert.Equal("'\\' OR \\'\\'=\\''", quoter.Quote("' OR ''='"));
            Assert.Equal("'1\\' or \\'1\\' = \\'1'", quoter.Quote("1' or '1' = '1"));
            Assert.Equal("'1\\' or \\'1\\' = \\'1\\'))/*'", quoter.Quote("1' or '1' = '1'))/*"));
        }

        [Fact]
        public void TestQuoteName()
        {
            Quoter quoter = new Quoter();

            Assert.Equal("`field`", quoter.QuoteName("field"));
            Assert.Equal("`table`.`field`", quoter.QuoteName("table.field"));
            Assert.Equal("`db`.`table`.`field`", quoter.QuoteName("db.table.field"));

            Assert.Equal("`field` AS `f1`", quoter.QuoteName("field AS f1"));
            Assert.Equal("`table`.`field` AS `f1`", quoter.QuoteName("table.field AS f1"));
            Assert.Equal("`db`.`table`.`field` AS `f1`", quoter.QuoteName("db.table.field AS f1"));

            Assert.Equal("`field` AS `f1`", quoter.QuoteName("field f1"));
            Assert.Equal("`table`.`field` AS `f1`", quoter.QuoteName("table.field f1"));
            Assert.Equal("`db`.`table`.`field` AS `f1`", quoter.QuoteName("db.table.field f1"));

            Assert.Equal("*", quoter.QuoteName("*"));
            Assert.Equal("`table`.*", quoter.QuoteName("table.*"));
            Assert.Equal("`db`.`table`.*", quoter.QuoteName("db.table.*"));
        }
    }
}
