namespace SqlBuilder {
  public class RealSqlTable : ISqlTable {
    private readonly string myRealName;
    private string myName;

    public RealSqlTable(string name) {
      myName = name;
      myRealName = name;
    }

    public RealSqlTable(string realName, string name) {
      myName = name;
      myRealName = realName;
    }

    #region ISqlTable Members

    public string Name {
      get { return myName; }
    }
    
    public string From {
      get {
        if (myRealName != myName)
          return string.Format("{0} AS {1}", myRealName, myName);
        return myName;
      }
    }
    
    public ISqlTable NewName() {
      RealSqlTable table = Clone();
      table.myName = Naming.NewTableName();
      return table;
    }

    #endregion

    public RealSqlTable Clone() {
      var table = new RealSqlTable(myRealName, myName);
      return table;
    }
  }
}