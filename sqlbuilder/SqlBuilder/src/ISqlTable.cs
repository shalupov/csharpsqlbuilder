namespace SqlBuilder {
  public interface ISqlTable {
    string From { get; }
    string Name { get; }
    ISqlTable NewName();
  }
}
