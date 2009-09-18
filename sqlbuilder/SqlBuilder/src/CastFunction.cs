namespace SqlBuilder {
  public class CastFunction : Function {
    public string ConvertToType { get; set; }
    
    public CastFunction(string type, Expression e): base("CAST", e) {
      ConvertToType = type;
    }

    public override string ToSQL() {
      return Name + (" ".Join(myArguments.Map(x => x.ToSQL())) + " AS " + ConvertToType).SurroundWithBrackets();
    }
  }
}