namespace SqlBuilder {
  public static class Sql {
    public static Expression Like(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "LIKE", Argument1 = e1, Argument2 = e2};
    }

    public static Expression As(string name, Expression e) {
      return new AsExpression {Name = name, Argument = e};
    }

    public static Expression Bind(out SqlColumn column, Expression e) {
      column = new SqlColumn();
      return new BindExpression {BindColumn = column, Argument = e};
    }

    public static Expression Eq(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "=", Argument1 = e1, Argument2 = e2};
    }

    public static Expression Neq(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "<>", Argument1 = e1, Argument2 = e2};
    }

    public static Expression In(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "IN", Argument1 = e1, Argument2 = e2};
    }

    public static Expression And(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "AND", Argument1 = e1, Argument2 = e2};
    }

    public static Expression Or(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "OR", Argument1 = e1, Argument2 = e2};
    }

    public static Expression More(Expression e1, Expression e2) {
      return new BinaryOperator {Op = ">", Argument1 = e1, Argument2 = e2};
    }

    public static Expression MoreOrEqual(Expression e1, Expression e2) {
      return new BinaryOperator {Op = ">=", Argument1 = e1, Argument2 = e2};
    }

    public static Expression Less(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "<", Argument1 = e1, Argument2 = e2};
    }

    public static Expression LessOrEqual(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "<=", Argument1 = e1, Argument2 = e2};
    }

    public static Expression Plus(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "+", Argument1 = e1, Argument2 = e2};
    }

    public static Expression Minus(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "-", Argument1 = e1, Argument2 = e2};
    }

    public static Expression Multiply(Expression e1, Expression e2) {
      return new BinaryOperator {Op = "*", Argument1 = e1, Argument2 = e2};
    }

    public static Expression IfNull(Expression e1, Expression e2) {
      return new Function("IFNULL", e1, e2);
    }

    public static Expression Concat(params Expression[] es) {
      return new Function("CONCAT", es);
    }

    public static Expression ConcatWS(params Expression[] es) {
      return new Function("CONCAT_WS", es);
    }

    public static Expression Max(Expression e) {
      return new Function("MAX", e);
    }

    public static Expression Min(Expression e) {
      return new Function("MIN", e);
    }

    public static Expression Sum(Expression e) {
      return new Function("SUM", e);
    }

    public static Expression Count(Expression e) {
      return new Function("COUNT", e);
    }

    public static Expression GroupConcat(Expression e) {
      return new Function("GROUP_CONCAT", e);
    }

    public static Expression IsNull(Expression e) {
      return new Function("ISNULL", e);
    }

    public static Expression Avg(Expression e) {
      return new Function("AVG", e);
    }

    public static Expression Day(Expression e) {
      return new Function("DAY", e);
    }

    public static Expression Month(Expression e) {
      return new Function("MONTH", e);
    }

    public static Expression Year(Expression e) {
      return new Function("YEAR", e);
    }

    public static Expression Const(object value) {
      return new Constant {Value = value};
    }

    public static Expression DateInterval(DateIntervalType type, int value) {
      return new DateInterval(type, value);
    }

    public static Expression DateAdd(Expression e, DateInterval interval) {
      return new Function("DATE_ADD", false, e, interval);
    }

    public static SelectStatement Select(params Expression[] columns) {
      var ss = new SelectStatement();
      foreach (Expression column in columns) ss.AddColumn(column);
      return ss;
    }

    public static SubQuery SubQuery(SelectStatement statement) {
      return new SubQuery(statement);
    }

    public static SubqueryExpression SubqueryExpression(SelectStatement statement) {
      return new SubqueryExpression(statement);
    }
  }
}