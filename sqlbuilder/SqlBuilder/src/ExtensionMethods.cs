using System;
using System.Collections.Generic;

namespace SqlBuilder {
  public static class ExtensionMethods {
    public static IEnumerable<G> Map<F, G>(this IEnumerable<F> source, Converter<F, G> converter) {
      foreach (F item in source) {
        yield return converter(item);
      }
    }

    public static string SurroundWithBrackets(this string s) {
      return "(" + s + ")";
    }
    
    public static string SurroundWithSpaces(this string s) {
      return " " + s + " ";
    }

    public static string Join<T>(this string delimiter, IEnumerable<T> source) {
      var items = new List<string>(Map(source, x => x.ToString()));

      return string.Join(delimiter, items.ToArray());
    }

    public static void VisitAll<T>(this Expression e, Action<T> action) where T : Expression {
      e.Accept(new CustomExpressionVisitor<T>(action));
    }

    public static SubqueryExpression ToExpression(this SelectStatement sql) {
      return new SubqueryExpression(sql);
    }


    #region Nested type: CustomExpressionVisitor

    private class CustomExpressionVisitor<T> : IVisitor where T : Expression {
      private readonly Action<T> myAction;

      public CustomExpressionVisitor(Action<T> action) {
        myAction = action;
      }

      #region IVisitor Members

      public void Visit(Expression expression) {
        if (expression is T) {
          myAction((T) expression);
        }
      }

      #endregion
    }

    #endregion
  }
}