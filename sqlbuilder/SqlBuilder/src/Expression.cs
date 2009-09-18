using System;

namespace SqlBuilder {
  public abstract class Expression {
    public abstract Expression Clone();
    public abstract void Accept(IVisitor visitor);
    public abstract string ToSQL();

    public Expression Like(Expression e) {
      return Sql.Like(this, e);
    }

    public Expression As(string name) {
      return Sql.As(name, this);
    }

    public Expression Bind(out SqlColumn column) {
      return Sql.Bind(out column, this);
    }

    public static Expression operator ==(Expression e1, Expression e2) {
      return Sql.Eq(e1, e2);
    }

    public static Expression operator !=(Expression e1, Expression e2) {
      return Sql.Neq(e1, e2);
    }

    public static Expression operator ==(Expression e1, int e2) {
      return Sql.Eq(e1, Sql.Const(e2));
    }

    public static Expression operator !=(Expression e1, int e2) {
      return Sql.Neq(e1, Sql.Const(e2));
    }

    public static Expression operator ==(Expression e1, string e2) {
      return Sql.Eq(e1, Sql.Const(e2));
    }

    public static Expression operator ==(Expression e1, SubqueryExpression e2) {
      return Sql.Eq(e1, e2);
    }

    public static Expression operator !=(Expression e1, SubqueryExpression e2) {
      return Sql.Neq(e1, e2);
    }

    public static Expression operator !=(Expression e1, string e2) {
      return Sql.Neq(e1, Sql.Const(e2));
    }

    public static Expression operator +(Expression e1, Expression e2) {
      return Sql.Plus(e1, e2);
    }

    public static Expression operator +(Expression e1, int e2) {
      return Sql.Plus(e1, Sql.Const(e2));
    }

    public static Expression operator -(Expression e1, Expression e2) {
      return Sql.Minus(e1, e2);
    }

    public static Expression operator -(Expression e1, int e2) {
      return Sql.Minus(e1, Sql.Const(e2));
    }

    public static Expression operator *(Expression e1, Expression e2) {
      return Sql.Multiply(e1, e2);
    }

    public static Expression operator *(Expression e1, int e2) {
      return Sql.Multiply(e1, Sql.Const(e2));
    }

    public static Expression operator /(Expression e1, Expression e2) {
      return Sql.Divide(e1, e2);
    }

    public static Expression operator /(Expression e1, int e2) {
      return Sql.Divide(e1, Sql.Const(e2));
    }

    public static Expression operator >=(Expression e1, Expression e2) {
      return Sql.MoreOrEqual(e1, e2);
    }

    public static Expression operator >=(Expression e1, int e2) {
      return Sql.MoreOrEqual(e1, Sql.Const(e2));
    }

    public static Expression operator >=(Expression e1, string e2) {
      return Sql.MoreOrEqual(e1, Sql.Const(e2));
    }

    public static Expression operator <=(Expression e1, Expression e2) {
      return Sql.LessOrEqual(e1, e2);
    }

    public static Expression operator <=(Expression e1, int e2) {
      return Sql.LessOrEqual(e1, Sql.Const(e2));
    }

    public static Expression operator <=(Expression e1, string e2) {
      return Sql.LessOrEqual(e1, Sql.Const(e2));
    }

    public static Expression operator >(Expression e1, Expression e2) {
      return Sql.More(e1, e2);
    }

    public static Expression operator >(Expression e1, int e2) {
      return Sql.More(e1, Sql.Const(e2));
    }

    public static Expression operator >(Expression e1, string e2) {
      return Sql.More(e1, Sql.Const(e2));
    }

    public static Expression operator <(Expression e1, Expression e2) {
      return Sql.Less(e1, e2);
    }

    public static Expression operator <(Expression e1, int e2) {
      return Sql.Less(e1, Sql.Const(e2));
    }

    public static Expression operator <(Expression e1, string e2) {
      return Sql.Less(e1, Sql.Const(e2));
    }

    public static Expression And(Expression e1, Expression e2) {
      return Sql.And(e1, e2);
    }
    
    public static Expression Or(Expression e1, Expression e2) {
      return Sql.Or(e1, e2);
    }

    public static Expression In(Expression e1, Expression e2) {
      return Sql.In(e1, e2);
    }

    public override bool Equals(object obj) {
      throw new NotSupportedException();
    }

    public override int GetHashCode() {
      throw new NotSupportedException();
    }
  }
}