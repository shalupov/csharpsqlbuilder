namespace SqlBuilder {
  public class MultipleOperator : Function {
    public MultipleOperator(string name, params Expression[] es) : base(name, es) {}

    public override string ToSQL() {
      return Name.SurroundWithSpaces().Join(myArguments.Map(x => x.ToSQL().SurroundWithBrackets()));
    }
  }
}