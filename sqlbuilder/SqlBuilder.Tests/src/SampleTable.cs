namespace SqlBuilder.Tests {
  public static class Users {
    public static readonly ISqlTable Table = new RealSqlTable("users");

    public static readonly SqlColumn Balance = new SqlColumn("balance", Table);
    public static readonly SqlColumn GroupId = new SqlColumn("group_id", Table);
    public static readonly SqlColumn Id = new SqlColumn("id", Table);
    public static readonly SqlColumn Name = new SqlColumn("name", Table);
  }

  public static class Groups {
    public static readonly ISqlTable Table = new RealSqlTable("groups");

    public static readonly SqlColumn Id = new SqlColumn("id", Table);
    public static readonly SqlColumn Name = new SqlColumn("name", Table);
  }

  public static class Office {
    public static readonly ISqlTable Table;

    public static readonly SqlColumn Id;
    public static readonly SqlColumn Name;

    static Office() {
      Table = new RealSqlTable("office");
      Id = new SqlColumn("id", Table);
      Name = new SqlColumn("name", Table);
    }
    
    public static OfficeTable Clone() {
      return new OfficeTable(Naming.NewTableName());
    }
  }
  
  public class OfficeTable {
    public ISqlTable Table;
    public SqlColumn Id;
    public SqlColumn Name;
    
    public OfficeTable(string tableName) {
      Table = new RealSqlTable("office", tableName);
      Id = new SqlColumn("id", Table); 
      Name = new SqlColumn("name", Table);
    }
  }

  public static class EducationGroup {
    public static readonly ISqlTable Table = new RealSqlTable("ed_group");

    public static readonly SqlColumn Id = new SqlColumn("id", Table);
    public static readonly SqlColumn CacheTitle = new SqlColumn("cache_title", Table);
    public static readonly SqlColumn OfficeId = new SqlColumn("office_id", Table);
  }

  public static class ChargeRecord {
    public static readonly ISqlTable Table = new RealSqlTable("charge_record");

    public static readonly SqlColumn Id = new SqlColumn("id", Table);
    public static readonly SqlColumn Tariff = new SqlColumn("tariff", Table);
    public static readonly SqlColumn RealHours = new SqlColumn("real_hours", Table);
    public static readonly SqlColumn GroupId = new SqlColumn("group_id", Table);
  }


  public class UsersGoodBalanceView {
    public readonly ISqlTable Table;

    public readonly SqlColumn Id;
    public readonly SqlColumn Name;

    private UsersGoodBalanceView() {
      Table = Sql.SubQuery(Sql.Select(
                             Users.Id.Bind(out Id),
                             Users.Name.Bind(out Name)).Where(Users.Balance > Sql.Const(0)));
    }

    public static UsersGoodBalanceView NewSubquery() {
      return new UsersGoodBalanceView();
    }
  }
}