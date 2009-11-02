using System.Collections.Generic;
using System.Text;

namespace SqlBuilder {
  public class UnionExpression {
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

    public UnionExpression Clone() {
      return new UnionExpression(myFirstSelectStatement, mySecondSelectStatement, myOrderBy);
    }

    public UnionExpression OrderBy(SqlColumn column, OrderByType type) {
      var clone = Clone();
      clone.myOrderBy.Add(new OrderByStatement {Column = column, Type = type});
      return clone;
    }

    public UnionExpression OrderBy(params SqlColumn[] columns) {
      var clone = Clone();
      clone.myOrderBy.AddRange(columns.Map(x => new OrderByStatement {Column = x, Type = OrderByType.Ascending}));
      return clone;
    }

    private string OrderByToSQL() {
      var result = new StringBuilder();
      if (myOrderBy.Count > 0) {
        result.Append(" ORDER BY ");
        result.Append(", ".Join(myOrderBy.Map(x => x.ToSQL())));
      }
      return result.ToString();
    }

    public string ToSQL() {
      return myFirstSelectStatement.ToSQL().SurroundWithBrackets() +
             " UNION " +
             mySecondSelectStatement.ToSQL().SurroundWithBrackets() +
             OrderByToSQL();
    }
  }
}