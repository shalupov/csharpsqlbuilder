namespace SqlBuilder {
  public interface IVisitor {
    void Visit(Expression expression);
  }
}