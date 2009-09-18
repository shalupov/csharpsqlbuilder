namespace SqlBuilder {
  public class BinaryOperator : Expression {
    public Expression Argument1 { get; set; }
    public Expression Argument2 { get; set; }

    public string Op { get; set; }

    public override Expression Clone() {
      return new BinaryOperator {
                                  Op = Op,
                                  Argument1 = Argument1,
                                  Argument2 = Argument2
                                }
        ;
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);

      Argument1.Accept(visitor);
      Argument2.Accept(visitor);
    }

    public override string ToSQL() {
      return Argument1.ToSQL() + " " + Op + " " + Argument2.ToSQL();
    }
  }
}