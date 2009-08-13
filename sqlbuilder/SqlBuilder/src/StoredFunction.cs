using System.Collections.Generic;

namespace SqlBuilder {
  public class StoredFunction : Expression {
    protected readonly List<Expression> myArguments = new List<Expression>();
    protected string myName;

    public StoredFunction(params Expression[] args) {
      myArguments.AddRange(args);
    }

    public StoredFunction(string name, params Expression[] args) {
      myName = name;
      myArguments.AddRange(args);
    }

    public override Expression Clone() {
      return new StoredFunction(myName, myArguments.ToArray());
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);
      foreach (Expression argument in myArguments) argument.Accept(visitor);
    }

    public override string ToSQL() {
      return myName + ", ".Join(myArguments.Map(x => x.ToSQL())).SurroundWithBrackets();
    }
  }
}