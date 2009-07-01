namespace SqlBuilder {
  public class SqlColumn : Expression {
    public SqlColumn() {}

    public SqlColumn(string name, ISqlTable table) {
      Name = name;
      Table = table;
    }

    public string Name { get; set; }
    public ISqlTable Table { get; set; }

    public override Expression Clone() {
      return new SqlColumn {Name = Name, Table = Table};
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
    }

    public bool Equals(SqlColumn obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      return base.Equals(obj) && Equals(obj.Name, Name) && Equals(obj.Table, Table);
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      return Equals(obj as SqlColumn);
    }

    public override string ToSQL() {
      if (Table == null)
        return Name;
      return string.Format(Table.Name + "." + Name);
    }
  }
}