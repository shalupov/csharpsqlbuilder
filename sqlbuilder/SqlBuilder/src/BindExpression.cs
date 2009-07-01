namespace SqlBuilder {
  public class BindExpression : Expression {
    public Expression Argument { get; set; }
    public SqlColumn BindColumn { get; set; }

    public override Expression Clone() {
      return new BindExpression() { BindColumn = BindColumn, Argument = Argument };
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
      Argument.Accept(visitor);
    }

    public override string ToSQL() {
      return Argument.ToSQL();
    }
  }
}