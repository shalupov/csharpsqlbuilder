namespace SqlBuilder {
  public class SubqueryExpression : Expression {
    private readonly SelectStatement myStatement;

    public SubqueryExpression(SelectStatement ss) {
      myStatement = ss;
    }

    public SelectStatement Statement {
      get { return myStatement; }
    }

    public override Expression Clone() {
      return new SubqueryExpression(myStatement);
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
    }

    public override string ToSQL() {
      return "(" + myStatement.ToSQL() + ")";
    }
  }
}