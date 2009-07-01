using System.Collections.Generic;

namespace SqlBuilder {
  public class Function : Expression {
    protected readonly List<Expression> myArguments = new List<Expression>();

    protected readonly bool myUseBrackets = true;

    public Function(string name, params Expression[] es) {
      Name = name;
      myArguments.AddRange(es);
    }

    public Function(string name, bool useBrackets, params Expression[] es) {
      Name = name;
      myUseBrackets = useBrackets;
      myArguments.AddRange(es);
    }

    public List<Expression> Arguments {
      get { return myArguments; }
    }

    public string Name { get; set; }

    public override Expression Clone() {
      var fun = new Function(Name, myUseBrackets);
      fun.myArguments.AddRange(
        myArguments.Map(x => x.Clone()));
      return fun;
    }

    public override void Accept(IVisitor visitor) {
      visitor.Visit(this);

      foreach (Expression argument in myArguments) argument.Accept(visitor);
    }

    public override string ToSQL() {
      if (myUseBrackets) {
        return Name +
               "(" +
               ", ".Join(myArguments.Map(x => x.ToSQL().SurroundWithBrackets())) +
               ")";
      }

      return Name +
             "(" +
             ", ".Join(myArguments.Map(x => x.ToSQL())) +
             ")";
    }
  }
}