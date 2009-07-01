namespace SqlBuilder {
  public class AsExpression : Expression {
    public Expression Argument { get; set; }
    public string Name { get; set; }

    public override Expression Clone() {
      return new AsExpression { Name = Name, Argument = Argument };
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