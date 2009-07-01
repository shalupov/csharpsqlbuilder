namespace SqlBuilder {
  public class SubqueryExpression : Expression {
    private readonly bool myIsSurrounBrackets;
    private readonly SelectStatement myStatement;

    public SubqueryExpression(SelectStatement ss) {
      myStatement = ss;
      myIsSurrounBrackets = true;
    }

    public SubqueryExpression(SelectStatement ss, bool isSurroundBrackets) {
      myStatement = ss;
      myIsSurrounBrackets = isSurroundBrackets;
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
      if (myIsSurrounBrackets)
        return "(" + myStatement.ToSQL() + ")";
      return myStatement.ToSQL();
    }
  }
}