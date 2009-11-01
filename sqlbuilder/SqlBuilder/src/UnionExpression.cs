using System.Collections.Generic;
using System.Text;

namespace SqlBuilder {
  public class UnionExpression : Expression {
    private readonly SelectStatement myFirstSelectStatement;
    private readonly List<OrderByStatement> myOrderBy = new List<OrderByStatement>();
    private readonly SelectStatement mySecondSelectStatement;

    public UnionExpression(SelectStatement s1, SelectStatement s2, List<OrderByStatement> orderBy) {
      myFirstSelectStatement = s1;
      mySecondSelectStatement = s2;
      myOrderBy = orderBy;
    }

    public UnionExpression(SelectStatement s1, SelectStatement s2) {
      myFirstSelectStatement = s1;
      mySecondSelectStatement = s2;
    }

    public override Expression Clone() {
      return new UnionExpression(myFirstSelectStatement, mySecondSelectStatement, myOrderBy);
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
    }

    public UnionExpression OrderBy(SqlColumn column, OrderByType type) {
      var clone = Clone() as UnionExpression;
// ReSharper disable PossibleNullReferenceException
      clone.myOrderBy.Add(new OrderByStatement {Column = column, Type = type});
// ReSharper restore PossibleNullReferenceException
      return clone;
    }

    public UnionExpression OrderBy(SqlColumn column) {
      return OrderBy(column, OrderByType.Ascending);
    }

    private string OrderByToSQL() {
      var result = new StringBuilder();
      if (myOrderBy.Count > 0) {
        result.Append(" ORDER BY ");
        result.Append(", ".Join(myOrderBy.Map(x => x.ToSQL())));
      }
      return result.ToString();
    }

    public override string ToSQL() {
      return myFirstSelectStatement.ToSQL().SurroundWithBrackets() +
             " UNION " +
             mySecondSelectStatement.ToSQL().SurroundWithBrackets() +
             OrderByToSQL();
    }
  }
}