namespace SqlBuilder {
  public class UnionExpression : Expression {
    private readonly SelectStatement myFirstSelectStatement;
    private readonly SelectStatement mySecondSelectStatement;

    public UnionExpression(SelectStatement s1, SelectStatement s2) {
      myFirstSelectStatement = s1;
      mySecondSelectStatement = s2;
    }

    public override Expression Clone() {
      return new UnionExpression(myFirstSelectStatement, mySecondSelectStatement);
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
    }

    public override string ToSQL() {
      return ("UNION " +
              myFirstSelectStatement.ToSQL().SurroundWithBrackets() +
              mySecondSelectStatement.ToSQL().SurroundWithBrackets()
             ).SurroundWithBrackets();
    }
  }
}