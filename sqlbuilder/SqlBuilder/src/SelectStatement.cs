using System;
using System.Collections.Generic;
using System.Text;

namespace SqlBuilder {
  public class SelectStatement {
    private readonly List<Expression> myAndHaving = new List<Expression>();
    private readonly List<Expression> myAndWhere = new List<Expression>();

    private readonly List<KeyValuePair<string, Expression>> myColumns =
      new List<KeyValuePair<string, Expression>>();

    private readonly List<Expression> myGroupBy = new List<Expression>();
    private readonly List<JoinPart> myJoins = new List<JoinPart>();
    private readonly List<LimitStatement> myLimits = new List<LimitStatement>();
    private readonly List<OrderByStatement> myOrderBy = new List<OrderByStatement>();

    public IEnumerable<Expression> Columns {
      get { return myColumns.Map(x => x.Value); }
    }

    public List<KeyValuePair<string, Expression>> FullColumns {
      get { return myColumns; }
    }

    public string ToSQL() {
      var result = new StringBuilder("SELECT ");
      result.Append(", ".Join(
                      myColumns.Map(x => {
                                      if (x.Key == null) return x.Value.ToSQL();
                                      else return x.Value.ToSQL() + " AS " + x.Key;
                                    })));

      HashSet<ISqlTable> tables = GatherFromTables();
      tables.ExceptWith(myJoins.Map(x => x.Table));

      if (tables.Count > 0) {
        result.Append(" FROM ");
        result.Append(", ".Join(tables.Map(x => x.From)));
      }

      if (myJoins.Count > 0) {
        result.Append(" ");
        result.Append(" ".Join(myJoins.Map(x => x.ToSQL())));
      }

      if (myAndWhere.Count > 0) {
        result.Append(" WHERE ");
        result.Append(" AND ".Join(myAndWhere.Map(x => "(" + x.ToSQL() + ")")));
      }

      if (myGroupBy.Count > 0) {
        result.Append(" GROUP BY ");
        result.Append(", ".Join(myGroupBy.Map(x => x.ToSQL())));
      }

      if (myAndHaving.Count > 0) {
        result.Append(" HAVING ");
        result.Append(" AND ".Join(myAndHaving.Map(x => "(" + x.ToSQL() + ")")));
      }

      if (myOrderBy.Count > 0) {
        result.Append(" ORDER BY ");
        result.Append(", ".Join(myOrderBy.Map(x => x.ToSQL())));
      }

      if (myLimits.Count > 0) {
        result.Append(" ");
        result.Append(", ".Join(myLimits.Map(x => x.ToSQL())));
      }

      return result.ToString();
    }

    private HashSet<ISqlTable> GatherFromTables() {
      var gatherer = new TableGatherer();

      foreach (var column in myColumns) gatherer.Gather(column.Value);

      return gatherer.Tables;
    }

    public void AddColumn(Expression col) {
      string asName = FindAs(col);

      string colName = asName;
      if (asName == null) colName = "col_" + (myColumns.Count + 1);

      bool binded = false;
      col.VisitAll<BindExpression>(x => {
                                     x.BindColumn.Name = colName;
                                     binded = true;
                                   });

      myColumns.Add(new KeyValuePair<string, Expression>(
                      (binded || asName != null) ? colName : null,
                      col));
    }

    private static string FindAs(Expression e) {
      string result = null;
      e.VisitAll<AsExpression>(x => result = x.Name);

      return result;
    }

    public SelectStatement Where(params Expression[] es) {
      SelectStatement clone = Clone();
      clone.myAndWhere.AddRange(es);
      return clone;
    }

    public SelectStatement Having(params Expression[] es) {
      SelectStatement clone = Clone();
      clone.myAndHaving.AddRange(es);
      return clone;
    }

    public SelectStatement Join(JoinType type, ISqlTable table, params Expression[] cond) {
      SelectStatement clone = Clone();
      clone.myJoins.Add(new JoinPart
                        {JoinType = type, Table = table, Conditions = new List<Expression>(cond)});
      return clone;
    }

    public SelectStatement GroupBy(params Expression[] es) {
      SelectStatement clone = Clone();
      clone.myGroupBy.AddRange(es);
      return clone;
    }

    public SelectStatement OrderBy(SqlColumn column, OrderByType type) {
      SelectStatement clone = Clone();
      clone.myOrderBy.Add(new OrderByStatement {Column = column, Type = type});
      return clone;
    }

    public SelectStatement Limit(int limit) {
      SelectStatement clone = Clone();
      clone.myLimits.Add(new LimitStatement {Limit = limit});
      return clone;
    }

    public SelectStatement AddColumns(params Expression[] cols) {
      SelectStatement clone = Clone();
      foreach (Expression col in cols) clone.AddColumn(col);
      return clone;
    }

    internal SelectStatement Clone() {
      var ss = new SelectStatement();
      ss.myAndWhere.AddRange(myAndWhere);
      ss.myColumns.AddRange(myColumns);
      ss.myGroupBy.AddRange(myGroupBy);
      ss.myJoins.AddRange(myJoins);
      ss.myOrderBy.AddRange(myOrderBy);
      return ss;
    }

    #region Nested type: JoinPart

    private class JoinPart {
      public JoinType JoinType { get; set; }
      public ISqlTable Table { get; set; }
      public List<Expression> Conditions { get; set; }

      private static string JoinType2String(JoinType type) {
        switch (type) {
          case JoinType.Join:
            return "JOIN";
          case JoinType.LeftJoin:
            return "LEFT JOIN";
          case JoinType.RightJoin:
            return "RIGHT JOIN";
          case JoinType.InnerJoin:
            return "INNER JOIN";
          default:
            throw new ApplicationException("Uncovered join type: " + type);
        }
      }

      public string ToSQL() {
        return JoinType2String(JoinType) + " "
               + Table.From + " ON " + "(" +
               " AND ".Join(Conditions.Map(x => x.ToSQL())) +
               ")";
      }
    }

    #endregion

    #region Nested type: LimitStatement

    private class LimitStatement {
      public int Limit { get; set; }

      public string ToSQL() {
        return "LIMIT " + Limit;
      }
    }

    #endregion

    #region Nested type: OrderByStatement

    private class OrderByStatement {
      public SqlColumn Column { get; set; }
      public OrderByType Type { get; set; }

      private static string OrderByType2String(OrderByType type) {
        switch (type) {
          case OrderByType.Ascending:
            return "ASC";
          case OrderByType.Descending:
            return "DESC";
          default:
            throw new ApplicationException("Uncovered order by type: " + type);
        }
      }

      public string ToSQL() {
        return Column.ToSQL() + " "
               + OrderByType2String(Type);
      }
    }

    #endregion

    #region Nested type: TableGatherer

    private class TableGatherer : IVisitor {
      private readonly HashSet<ISqlTable> myTables = new HashSet<ISqlTable>();

      public HashSet<ISqlTable> Tables {
        get { return myTables; }
      }

      #region IVisitor Members

      public void Visit(Expression expression) {
        if (expression is SqlColumn) myTables.Add(((SqlColumn) expression).Table);
      }

      #endregion

      public void Gather(Expression expression) {
        expression.Accept(this);
      }
    }

    #endregion
  }
}