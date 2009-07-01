namespace SqlBuilder {
  public class DateInterval : Expression {
    public DateInterval(DateIntervalType type, int value) {
      Type = type;
      Value = value;
    }

    public DateIntervalType Type { get; set; }
    public int Value { get; set; }

    public override Expression Clone() {
      return new DateInterval(Type, Value);
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
    }

    public override string ToSQL() {
      return string.Format("INTERVAL {0} {1}", Value, Type.ToString().ToUpper());
    }
  }
}