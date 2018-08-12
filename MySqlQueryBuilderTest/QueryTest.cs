using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MySqlQueryBuilder;
using Xunit;

namespace MySqlQueryBuilderTest
{
    public class QueryTest : BaseTest
    {
        [Fact]
        public void TestSelectAll()
        {
            Query query = this.NewQuery();

            query.From("users");

            query.Select("*");

            string sql = query.GetSql();

            Assert.Equal("SELECT * FROM `users`;", sql);

            query.ClearSelect();
            query.From("database.users AS User");
            query.Select("database.users.*");
            Assert.Equal("SELECT `database`.`users`.* FROM `database`.`users` AS `User`;", query.GetSql());
        }

        [Fact]
        public void TestSelect()
        {
            Query query = this.NewQuery();
            query.From("users");
            query.Select("id", "first_name");

            /* query.Select(new List<string>()
             {
                 "last_name",
                 "email"
             });*/

            query.Select(new[]
            {
                new Field("last_name"),
                new Field("email")
            });


            //string[] array = {"username", "role"};
            //query.Select(array);

            query.Select(new[]
            {
                "username",
                "role"
            });

            string sql = query.GetSql();

            Assert.Equal("SELECT `id`, `first_name`, `last_name`, `email`, `username`, `role` FROM `users`;", sql);
        }

        [Fact]
        public void TestSelectWithAlias()
        {
            Query query = this.NewQuery();

            query.From("users");

            query.Select("field as alias");
            query.Select(new Field("field2", "alias2"));
            query.Select("table.fieldname3 as alias3");
            query.Select("database.table.fieldname4 as alias4");

            string sql = query.GetSql();

            Assert.Equal(
                "SELECT `field` AS `alias`, `field2` AS `alias2`, `table`.`fieldname3` AS `alias3`, `database`.`table`.`fieldname4` AS `alias4` FROM `users`;",
                sql);
        }

        [Fact]
        public void TestSelectWithFuncAndAlias()
        {
            Query query = this.NewQuery();

            query.From("users");
            query.Select(query.NewExpr("COUNT(*) AS counter"));
            string sql = query.GetSql();
            Assert.Equal("SELECT COUNT(*) AS counter FROM `users`;", sql);
        }

        [Fact]
        public void TestSelectWithListDynamic()
        {
            Query query = this.NewQuery();

            query.From("users");
            query.Select(new List<object>
            {
                "id",
                "field as alias",
                query.NewExpr("COUNT(*) AS counter"),
                new Field("field2", "alias2")
            });

            string sql = query.GetSql();
            Assert.Equal("SELECT `id`, `field` AS `alias`, COUNT(*) AS counter, `field2` AS `alias2` FROM `users`;",
                sql);
        }

        [Fact]
        public void TestSelectAndFetch()
        {
            Query query = this.NewQuery();

            query.From("users");
            query.Select(new List<object>
            {
                "id",
                "username",
                "first_name",
                "last_name",
                "email",
                query.NewExpr("DATABASE() AS db_name")
            });

            string sql = query.GetSql();

            var rows = query.Execute().FetchAll();

            foreach (var row in rows)
            {
                var id = row.Get<int>("id", null);
                var firstName = row.Get<string>("first_name", null);
                var nada = row.Get<string>("nada", null);
                var nada2 = row.Get<string>("nada2");
                var nada3 = row.Get<int>("nada3", 1);
                //var dbname = row.Get<string>("dbname");

                Assert.InRange(id, 1, 2);
                Assert.Null(row["first_name"]);
                Assert.Null(firstName);
                Assert.Null(nada);
                Assert.Null(nada2);
                Assert.Equal(1, nada3);
            }


            var users = query.Execute().FetchAll<User>();

            foreach (var user in users)
            {
                var id = user.Id;
                var firstName = user.FirstName;
                var dbname = user.DbName;

                Assert.Null(user.FirstName);
            }
        }

        [Fact]
        public void TestSelectAndSubqueries()
        {
            Query subquery = this.NewQuery();
            subquery.From("roles").Select("id", "title");
            subquery = subquery.ToSubQuery().Alias("role_alias");

            Query query = this.NewQuery();

            query.From("users");
            query.Select(new List<object>
            {
                "users.id",
                subquery,
            });

            string sql = query.GetSql();

            Assert.Equal("SELECT `users`.`id`, (SELECT `id`, `title` FROM `roles`) AS `role_alias` FROM `users`;", sql);
        }

        [Fact]
        public void TestSelectAndSubqueriesAndWhere()
        {
            Query subquery = this.NewQuery();
            subquery = subquery.Select("column1").From("t2").ToSubQuery();

            Query query = this.NewQuery();

            query.Select("*").From("t1");
            query.Where("column1", "=", subquery);

            string sql = query.GetSql();

            Assert.Equal("SELECT * FROM `t1` WHERE `column1` = (SELECT `column1` FROM `t2`);", sql);
        }

        [Fact]
        public void TestSelectAndWhere()
        {
            string[] comparisons = {"=", ">=", "<=", "<>", "!="};

            foreach (string comparison in comparisons)
            {
                Query query = this.NewQuery();

                query.Select("*").From("t1");
                query.Where("f1", comparison, 1);
                query.Where("f6", comparison, 3.14);
                query.Where("f2", comparison, "max");
                query.Where("f3", comparison, true);
                query.Where("f4", comparison, false);
                query.Where("f5", comparison, null);

                string sql = query.GetSql();

                string exected = "SELECT * FROM `t1` WHERE `f1` {0} '1' AND `f6` {1} '3.14' AND `f2` {2} 'max' " +
                                 "AND `f3` {3} '1' AND `f4` {4} '0' AND `f5` IS NULL;";
                exected = String.Format(exected, comparison, comparison, comparison, comparison, comparison);
                Assert.Equal(exected, sql);
            }
        }

        [Fact]
        public void TestSelectAndWhereIn()
        {
            object[] data = {"1", "2", 3.14, 4, true, false, null};

            string[] comparisons = {"IN", "NOT IN"};

            foreach (string comparison in comparisons)
            {
                Query query = this.NewQuery();
                query.Select("*").From("t1");
                query.Where("f1", comparison, data);

                string sql = query.GetSql();

                string exected = "SELECT * FROM `t1` WHERE `f1` {0} ('1', '2', '3.14', '4', '1', '0', NULL);";
                exected = String.Format(exected, comparison, comparison, comparison, comparison, comparison);
                Assert.Equal(exected, sql);
            }
        }
    }
}