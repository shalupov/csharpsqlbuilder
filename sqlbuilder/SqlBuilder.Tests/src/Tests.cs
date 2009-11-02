using NUnit.Framework;

namespace SqlBuilder.Tests {
  [TestFixture]
  public class Tests {
    #region Setup/Teardown

    [SetUp]
    public void SetUp() {
      Naming.Reset();
    }

    #endregion

    [Test]
    public void AddColumns() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users");
    }

    [Test]
    public void As() {
      SelectStatement sql = Sql.Select(Users.Name.As("myname"));
      Assert.AreEqual("SELECT users.name AS myname FROM users",
                      sql.ToSQL());
    }

    [Test]
    public void Bind() {
      SqlColumn col;
      SelectStatement sql = Sql.Select(Users.Name.Bind(out col));

      Assert.AreEqual("SELECT users.name AS col_1 FROM users",
                      sql.ToSQL());
      Assert.AreEqual(null, col.Table);
      Assert.AreEqual("col_1", col.Name);
    }

    [Test]
    public void BindAs() {
      SqlColumn col;
      SelectStatement sql = Sql.Select(Users.Name.As("cc").Bind(out col));

      Assert.AreEqual("SELECT users.name AS cc FROM users",
                      sql.ToSQL());
      Assert.AreEqual(null, col.Table);
      Assert.AreEqual("cc", col.Name);
    }

    [Test]
    public void BindAs2() {
      SqlColumn col;
      SelectStatement sql = Sql.Select(Users.Name.Bind(out col).As("cc"));

      Assert.AreEqual("SELECT users.name AS cc FROM users",
                      sql.ToSQL());
      Assert.AreEqual(null, col.Table);
      Assert.AreEqual("cc", col.Name);
    }

    [Test]
    public void Concat() {
      SelectStatement sql = Sql.Select(
        Sql.Concat(Users.Name, Sql.Const(' '), Users.Balance));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT CONCAT((users.name), (' '), (users.balance)) FROM users");
    }
    
    [Test]
    public void ConcatWS() {
      SelectStatement sql = Sql.Select(
        Sql.ConcatWS(Sql.Const(' '), Users.Name, Users.Balance));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT CONCAT_WS((' '), (users.name), (users.balance)) FROM users");
    }

    [Test]
    public void GroupBy() {
      SelectStatement sql = Sql.Select(
        Sql.Sum(Users.Balance),
        Sql.Avg(Users.Balance)).GroupBy(Users.GroupId);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT SUM((users.balance)), AVG((users.balance)) FROM users GROUP BY users.group_id");
    }

    [Test]
    public void GroupBy2() {
      SelectStatement sql = Sql.Select(
        Sql.Sum(Users.Balance),
        Sql.Avg(Users.Balance)).GroupBy(Users.GroupId, Users.Name);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT SUM((users.balance)), AVG((users.balance)) FROM users GROUP BY users.group_id, users.name");
    }

    [Test]
    public void Like() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Name.Like(Sql.Const("%bla%")));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.name LIKE '%bla%')");
    }

    [Test]
    public void LikeSub() {
      UsersGoodBalanceView uv = UsersGoodBalanceView.NewSubquery();

      SelectStatement sql = Sql.Select(uv.Name)
        .Where(uv.Name.Like(Sql.Const("%bla%")));
      Assert.AreEqual("SELECT t_1.col_2 FROM (SELECT users.id AS col_1, users.name AS col_2 FROM users WHERE (users.balance > 0)) AS t_1 WHERE (t_1.col_2 LIKE '%bla%')",
                      sql.ToSQL()
        );
    }

    [Test]
    public void MinMaxSumAvg() {
      SelectStatement sql = Sql.Select(
        Sql.Min(Users.Balance),
        Sql.Max(Users.Balance),
        Sql.Sum(Users.Balance),
        Sql.Avg(Users.Balance));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT MIN((users.balance)), MAX((users.balance)), SUM((users.balance)), AVG((users.balance)) FROM users");
    }

    [Test]
    public void Minus() {
      SelectStatement sql = Sql.Select(Users.Id - Sql.Const(2));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id - 2 FROM users");
    }

    [Test]
    public void Minus2() {
      SelectStatement sql = Sql.Select(Users.Id - 2);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id - 2 FROM users");
    }

    [Test]
    public void Divide() {
      SelectStatement sql = Sql.Select(Users.Id/2);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id / 2 FROM users");
    }

    [Test]
    public void Plus() {
      SelectStatement sql = Sql.Select(Users.Id + Sql.Const(2));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id + 2 FROM users");
    }

    [Test]
    public void Plus2() {
      SelectStatement sql = Sql.Select(Users.Id + 2);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id + 2 FROM users");
    }

    [Test]
    public void More() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance + Users.GroupId > Sql.Const(5));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance + users.group_id > 5)");
    }

    [Test]
    public void More2() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance + Users.GroupId > 5);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance + users.group_id > 5)");
    }

    [Test]
    public void Less() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance < Sql.Const(5));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance < 5)");
    }

    [Test]
    public void Less2() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance < 5);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance < 5)");
    }

    [Test]
    public void MoreEqual() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance + Users.GroupId >= Sql.Const(5));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance + users.group_id >= 5)");
    }

    [Test]
    public void MoreEqual2() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance + Users.GroupId >= 5);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance + users.group_id >= 5)");
    }

    [Test]
    public void LessEqual() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance <= Sql.Const(5));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance <= 5)");
    }

    [Test]
    public void LessEqual2() {
      SelectStatement sql = Sql.Select(Users.Id)
        .Where(Users.Balance <= 5);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id FROM users WHERE (users.balance <= 5)");
    }

    [Test]
    public void Equal() {
      SelectStatement sql = Sql.Select(Users.Id).Where(Users.GroupId == 5);
      Assert.AreEqual("SELECT users.id FROM users WHERE (users.group_id = 5)", sql.ToSQL());
    }

    [Test]
    public void Equal2() {
      SelectStatement sql = Sql.Select(Users.Id).Where(Users.GroupId == Sql.Const(5));
      Assert.AreEqual("SELECT users.id FROM users WHERE (users.group_id = 5)", sql.ToSQL());
    }

    [Test]
    public void Equal3() {
      SelectStatement sql = Sql.Select(Users.Id).Where(Users.Name == "Vasja");
      Assert.AreEqual("SELECT users.id FROM users WHERE (users.name = 'Vasja')", sql.ToSQL());
    }

    [Test]
    public void NotEqual() {
      SelectStatement sql = Sql.Select(Users.Id).Where(Users.GroupId != 5);
      Assert.AreEqual("SELECT users.id FROM users WHERE (users.group_id <> 5)", sql.ToSQL());
    }

    [Test]
    public void NotEqual2() {
      SelectStatement sql = Sql.Select(Users.Id).Where(Users.GroupId != Sql.Const(5));
      Assert.AreEqual("SELECT users.id FROM users WHERE (users.group_id <> 5)", sql.ToSQL());
    }

    [Test]
    public void NotEqual3() {
      SelectStatement sql = Sql.Select(Users.Id).Where(Users.Name != "Vasja");
      Assert.AreEqual("SELECT users.id FROM users WHERE (users.name <> 'Vasja')", sql.ToSQL());
    }

    [Test]
    public void Smoke1() {
      SelectStatement sql = Sql.Select(Users.Id, Users.Balance).
        Where(Users.Balance == Sql.Const(0));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance FROM users WHERE (users.balance = 0)");
    }

    [Test]
    public void SmokeJoin() {
      SelectStatement sql = Sql.Select(Users.Name, Groups.Name)
        .Join(JoinType.LeftJoin, Groups.Table, Users.GroupId == Groups.Id);
      Assert.AreEqual("SELECT users.name, groups.name FROM users LEFT JOIN groups ON (users.group_id = groups.id)",
                      sql.ToSQL());
    }

    [Test]
    public void SubQuery() {
      SubQuery sq = Sql.SubQuery(Sql.Select(Users.Id, Users.Name));

      Assert.AreEqual("(SELECT users.id, users.name FROM users) AS t_1", sq.From);
      Assert.AreEqual("t_1", sq.Name);
    }

    [Test]
    public void SubQueryAs() {
      SubQuery sq = Sql.SubQuery(Sql.Select(Users.Id, Users.Name)).As("one");

      Assert.AreEqual("(SELECT users.id, users.name FROM users) AS one",
                      sq.From);
      Assert.AreEqual("one", sq.Name);
    }

    [Test]
    public void SelectWithJoin() {
      SelectStatement sql = Sql.Select(
        Sql.Sum(ChargeRecord.Tariff * ChargeRecord.RealHours).As("sum"),
        EducationGroup.CacheTitle.As("title"))
        .GroupBy(EducationGroup.Id)
        .Join(JoinType.LeftJoin, ChargeRecord.Table, ChargeRecord.GroupId == EducationGroup.Id);

      Assert.AreEqual(@"SELECT SUM((charge_record.tariff * charge_record.real_hours)) AS sum, ed_group.cache_title AS title FROM ed_group LEFT JOIN charge_record ON (charge_record.group_id = ed_group.id) GROUP BY ed_group.id",
                      sql.ToSQL());
    }

    [Test]
    public void SeparatedSelectWithJoin() {
      SelectStatement sql = Sql.Select(EducationGroup.CacheTitle,
        Sql.Sum(ChargeRecord.Tariff * ChargeRecord.RealHours).As("sum"))
        .GroupBy(EducationGroup.Id);
      sql = sql.Join(JoinType.InnerJoin, ChargeRecord.Table, ChargeRecord.GroupId == EducationGroup.Id);

      Assert.AreEqual("SELECT ed_group.cache_title, SUM((charge_record.tariff * charge_record.real_hours)) AS sum FROM ed_group INNER JOIN charge_record ON (charge_record.group_id = ed_group.id) GROUP BY ed_group.id",
                      sql.ToSQL());
    }

    [Test]
    public void SelectWithTwoJoins() {
      SelectStatement sql = Sql.Select(
        Office.Name.As("Name"),
        Sql.Sum(ChargeRecord.Tariff * ChargeRecord.RealHours).As("sum")
        )
        .GroupBy(Office.Name)
        .Join(JoinType.InnerJoin, EducationGroup.Table, Office.Id == EducationGroup.OfficeId);
      sql = sql.Join(JoinType.InnerJoin, ChargeRecord.Table, EducationGroup.Id == ChargeRecord.GroupId);

      Assert.AreEqual("SELECT office.name AS Name, SUM((charge_record.tariff * charge_record.real_hours)) AS sum FROM office INNER JOIN ed_group ON (office.id = ed_group.office_id) INNER JOIN charge_record ON (ed_group.id = charge_record.group_id) GROUP BY office.name",
                      sql.ToSQL());
    }

    [Test]
    public void OrderByAsc() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .OrderBy(Users.Name, OrderByType.Ascending);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users ORDER BY users.name ASC");
    }
    
    [Test]
    public void OrderByAscByDefault() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .OrderBy(Users.Name);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users ORDER BY users.name ASC");
    }

    [Test]
    public void OrderByDesc() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .OrderBy(Users.Name, OrderByType.Descending);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users ORDER BY users.name DESC");
    }

    [Test]
    public void TwoOrderBy() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .OrderBy(Users.Name, OrderByType.Descending)
        .OrderBy(Users.Id, OrderByType.Ascending);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users ORDER BY users.name DESC, users.id ASC");
    }

    [Test]
    public void Limits() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .Limit(1);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users LIMIT 1");
    }

    [Test]
    public void LimitsAndWhere() {
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .Where(Users.Balance > Sql.Const("0"))
        .Limit(1);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance, users.group_id FROM users WHERE (users.balance > '0') LIMIT 1");
    }

    [Test]
    public void SubQueryStatement() {
      var subQuery = new SubqueryExpression(Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId));

      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .Where(Users.Balance == subQuery);
      Assert.AreEqual(sql.ToSQL(),
        "SELECT users.id, users.balance, users.group_id FROM users WHERE (users.balance = (SELECT users.id, users.balance, users.group_id FROM users))");
    }

[Test]
    public void SubQueryStatementWithouBrackets() {
      var subQuery = new SubqueryExpression(Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId), false);

      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .Where(Sql.Exists(subQuery));
      Assert.AreEqual(sql.ToSQL(),
        "SELECT users.id, users.balance, users.group_id FROM users WHERE (EXISTS(SELECT users.id, users.balance, users.group_id FROM users))");
    }

    [Test]
    public void SubQueryStatementWithJoin() {
      var subQuery = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId).ToExpression();

      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .Join(JoinType.InnerJoin, ChargeRecord.Table,
          ChargeRecord.GroupId == subQuery);
      
      Assert.AreEqual(sql.ToSQL(),
        "SELECT users.id, users.balance, users.group_id FROM users INNER JOIN charge_record ON (charge_record.group_id = (SELECT users.id, users.balance, users.group_id FROM users))");
    }

    [Test]
    public void And() {
      SelectStatement sql = Sql.Select(Users.Name.As("myname"))
        .Where(Sql.And(Users.Name == ChargeRecord.RealHours, ChargeRecord.Id < Users.GroupId));
      Assert.AreEqual("SELECT users.name AS myname FROM users WHERE ((users.name = charge_record.real_hours) AND (charge_record.id < users.group_id))",
                      sql.ToSQL());
    }
    
    [Test]
    public void Or() {
      SelectStatement sql = Sql.Select(Users.Name.As("myname"))
        .Where(Sql.Or(Users.Name == ChargeRecord.RealHours, ChargeRecord.Id < Users.GroupId));
      Assert.AreEqual("SELECT users.name AS myname FROM users WHERE ((users.name = charge_record.real_hours) OR (charge_record.id < users.group_id))",
                      sql.ToSQL());
    }

    [Test]
    public void In() {
      var subQuery = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId).ToExpression();

      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance, Users.GroupId)
        .Where(Sql.In(Users.Balance,subQuery));

      Assert.AreEqual(sql.ToSQL(),
        "SELECT users.id, users.balance, users.group_id FROM users WHERE (users.balance IN (SELECT users.id, users.balance, users.group_id FROM users))");
    }

    [Test]
    public void Having() {
      SqlColumn balanceCol;
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance.As("balance").Bind(out balanceCol), Users.GroupId)
        .Having(balanceCol == 0);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance AS balance, users.group_id FROM users HAVING (balance = 0)");
    }

    [Test]
    public void HavingAndWhere() {
      SqlColumn balanceCol;
      SelectStatement sql = Sql.Select(Users.Id)
        .AddColumns(Users.Balance.Bind(out balanceCol), Users.GroupId)
        .Where(Users.Balance == 0)
        .Having(balanceCol == 0);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.id, users.balance AS col_2, users.group_id FROM users WHERE (users.balance = 0) HAVING (col_2 = 0)");
    }

    [Test]
    public void Count() {
      SelectStatement sql = Sql.Select(
        Sql.Count(Users.Name));
      Assert.AreEqual(sql.ToSQL(), "SELECT COUNT((users.name)) FROM users");
    }

    [Test]
    public void GroupConcat() {
      SelectStatement sql = Sql.Select(
        Sql.GroupConcat(Users.Name));
      Assert.AreEqual(sql.ToSQL(), "SELECT GROUP_CONCAT((users.name)) FROM users");
    }

    [Test]
    public void IsNull() {
      SelectStatement sql = Sql.Select(
        Users.Name).Where(Sql.IsNull(Users.GroupId));
      Assert.AreEqual(sql.ToSQL(), "SELECT users.name FROM users WHERE (ISNULL((users.group_id)))");
    }
    
    [Test]
    public void Alias() {
      var off = Office.Clone();
      SelectStatement sql = Sql.Select(
        Users.Name, off.Id)
        .Join(JoinType.InnerJoin, off.Table, off.Name == Users.Id);
      Assert.AreEqual(sql.ToSQL(), "SELECT users.name, t_1.id FROM users INNER JOIN office AS t_1 ON (t_1.name = users.id)");
    }
    
    [Test]
    public void Alias2() {
      var off = Office.Clone();
      var off2 = Office.Clone();
      SelectStatement sql = Sql.Select(
        Users.Name, off.Id)
        .Join(JoinType.InnerJoin, off.Table, off.Name == Users.Id)
        .Join(JoinType.InnerJoin, off2.Table, off2.Id == Users.Id);
      Assert.AreEqual(sql.ToSQL(), "SELECT users.name, t_1.id FROM users INNER JOIN office AS t_1 ON (t_1.name = users.id) INNER JOIN office AS t_2 ON (t_2.id = users.id)");
    }
    
    [Test]
    public void IfNull() {
      SelectStatement sql = Sql.Select(
        Users.Name).Where(Sql.IfNull(Users.GroupId, Users.Balance));
      Assert.AreEqual(sql.ToSQL(), "SELECT users.name FROM users WHERE (IFNULL((users.group_id), (users.balance)))");
    }
    
    [Test]
    public void If() {
      SelectStatement sql = Sql.Select(
        Users.Name).Where(Sql.If(Sql.Const(1) > Sql.Const(0), Users.GroupId, Users.Balance));
      Assert.AreEqual(sql.ToSQL(), "SELECT users.name FROM users WHERE (IF((1 > 0), (users.group_id), (users.balance)))");
    }
    
    [Test]
    public void Day() {
      SelectStatement sql = Sql.Select(Sql.Day(Users.Id));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT DAY((users.id)) FROM users");
    }
    
    [Test]
    public void Month() {
      SelectStatement sql = Sql.Select(Sql.Month(Users.Id));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT MONTH((users.id)) FROM users");
    }
    
    [Test]
    public void Year() {
      SelectStatement sql = Sql.Select(Sql.Year(Users.Id));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT YEAR((users.id)) FROM users");
    }
    
    [Test]
    public void DateAddMonths() {
      SelectStatement sql = Sql.Select(
        Sql.DateAdd(Users.Id, new DateInterval(DateIntervalType.Month, 1))
        );
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT DATE_ADD(users.id, INTERVAL 1 MONTH) FROM users");
    }
    
    [Test]
    public void DateAddYears() {
      SelectStatement sql = Sql.Select(
        Sql.DateAdd(Users.Id, new DateInterval(DateIntervalType.Year, 120))
        );
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT DATE_ADD(users.id, INTERVAL 120 YEAR) FROM users");
    }

    [Test]
    public void Or2() {
      SelectStatement sql = Sql.Select(Users.Name.As("myname"))
        .Where(Sql.Or(
                 Users.Name == ChargeRecord.RealHours,
                 ChargeRecord.Id < Users.GroupId,
                 Users.GroupId > ChargeRecord.GroupId));
      Assert.AreEqual(
        "SELECT users.name AS myname FROM users WHERE ((users.name = charge_record.real_hours) OR (charge_record.id < users.group_id) OR (users.group_id > charge_record.group_id))",
        sql.ToSQL());
    }
    
    [Test]
    public void And2() {
      SelectStatement sql = Sql.Select(Users.Name.As("myname"))
        .Where(Sql.And(
                 Users.Name == ChargeRecord.RealHours,
                 ChargeRecord.Id < Users.GroupId,
                 Users.GroupId > ChargeRecord.GroupId));
      Assert.AreEqual(
        "SELECT users.name AS myname FROM users WHERE ((users.name = charge_record.real_hours) AND (charge_record.id < users.group_id) AND (users.group_id > charge_record.group_id))",
        sql.ToSQL());
    }
    
    [Test]
    public void Left() {
      SelectStatement sql = Sql.Select(
        Sql.Left(Users.Id, 1)
        );
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT LEFT((users.id), (1)) FROM users");
    }
    
    [Test]
    public void Right() {
      SelectStatement sql = Sql.Select(
        Sql.Right(Users.Id, 1)
        );
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT RIGHT((users.id), (1)) FROM users");
    }
    
    [Test]
    public void Substring() {
      SelectStatement sql = Sql.Select(
        Sql.Substring(Users.Id, 1, 3)
        );
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT SUBSTRING((users.id), (1), (3)) FROM users");
    }

    [Test]
    public void Exists() {
      SelectStatement sql = Sql.Select(Users.Name).Where(Sql.Exists(Users.Id));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.name FROM users WHERE (EXISTS(users.id))");
    }
    
    [Test]
    public void NotExists() {
      SelectStatement sql = Sql.Select(Users.Name).Where(Sql.NotExists(Users.Id));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT users.name FROM users WHERE (NOT(EXISTS(users.id)))");
    }
    
    [Test]
    public void CastToString() {
      SelectStatement sql = Sql.Select(Sql.CastToString(Users.Name));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT CAST(users.name AS CHAR) FROM users");
    }
    
    [Test]
    public void Lpad() {
      SelectStatement sql = Sql.Select(Sql.Lpad(Users.Name, 2, "0"));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT LPAD(users.name, 2, '0') FROM users");
    }

    [Test]
    public void SimpleStoredFunction() {
      SelectStatement sql = Sql.Select(
        new SimpleFunction(Users.Id));
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT Simple(users.id) FROM users");
    }

    [Test]
    public void SimpleStoredFunctionWithManyArgs() {
      SelectStatement sql = Sql.Select(
        new SimpleFunction(Users.Id, Office.Name))
        .Join(Office.Table, Office.Id == Users.GroupId);
      Assert.AreEqual(sql.ToSQL(),
                      "SELECT Simple(users.id, office.name) FROM users INNER JOIN office ON (office.id = users.group_id)");
    }
    
    [Test]
    public void SmokeUnion() {
      SelectStatement sql = Sql.Select(
        new SimpleFunction(Users.Id, Office.Name))
        .Join(Office.Table, Office.Id == Users.GroupId);
      SelectStatement sql2 = Sql.Select(Users.Id);
      UnionExpression union = Sql.Union(sql, sql2);
      Assert.AreEqual(union.ToSQL(),  
                      "(SELECT Simple(users.id, office.name) FROM users INNER JOIN office ON (office.id = users.group_id))" +
                      " UNION " +
                      "(SELECT users.id FROM users)"
                      );
    }
    
    [Test]
    public void UnionWithOrderBy() {
      SqlColumn id;
      SelectStatement sql = Sql.Select(
        new SimpleFunction(Users.Id, Office.Name))
        .Join(Office.Table, Office.Id == Users.GroupId);
      SelectStatement sql2 = Sql.Select(Users.Id.As("id").Bind(out id));
      UnionExpression union = Sql.Union(sql, sql2).OrderBy(id);
      Assert.AreEqual(union.ToSQL(),
                      "(SELECT Simple(users.id, office.name) FROM users INNER JOIN office ON (office.id = users.group_id))" +
                      " UNION " +
                      "(SELECT users.id AS id FROM users) ORDER BY id ASC"
                      );
    }
    
    [Test]
    public void UnionWithMultiplyOrderBy() {
      SqlColumn id, name;
      SelectStatement sql = Sql.Select(
        new SimpleFunction(Users.Id, Office.Name), Users.Name.As("name").Bind(out name))
        .Join(Office.Table, Office.Id == Users.GroupId);
      SelectStatement sql2 = Sql.Select(Users.Id.As("id").Bind(out id), Users.Name.As("name").Bind(out name));
      UnionExpression union = Sql.Union(sql, sql2).OrderBy(id, name);
      Assert.AreEqual(union.ToSQL(),
                      "(SELECT Simple(users.id, office.name), users.name AS name FROM users INNER JOIN office ON (office.id = users.group_id))" +
                      " UNION " +
                      "(SELECT users.id AS id, users.name AS name FROM users) ORDER BY id ASC, name ASC"
                      );
    }
  }
}
