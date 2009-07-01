namespace SqlBuilder {
  public class SubQuery : ISqlTable {
    private readonly SelectStatement mySelect;

    public SubQuery(SelectStatement select) {
      mySelect = select.Clone();
      Name = Naming.NewTableName();

      foreach (Expression column in mySelect.Columns) {
        column.VisitAll<BindExpression>(x => x.BindColumn.Table = this);
      }
    }

    #region ISqlTable Members

    public string Name { get; set; }
    
    public string From {
      get {
        return string.Format("({0}) AS {1}", mySelect.ToSQL(), Name);
      }
    }
    
    public ISqlTable NewName() {
      throw new System.NotImplementedException();
    }

    #endregion

    public SubQuery As(string name) {
      return new SubQuery(mySelect) {Name = name};
    }
  }
}
