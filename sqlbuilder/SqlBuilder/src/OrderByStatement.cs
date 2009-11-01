using System;

namespace SqlBuilder {
  public class OrderByStatement {
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
}