namespace SqlBuilder {
  public class Constant : Expression {
    public object Value { get; set; }

    public override Expression Clone() {
      return new Constant {Value = Value};
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
    }

    public override string ToSQL() {
      // TODO Quote
      if (Value is string || Value is char) {
        return string.Format("'{0}'", Value);
      }

      return Value.ToString();
    }
  }
}