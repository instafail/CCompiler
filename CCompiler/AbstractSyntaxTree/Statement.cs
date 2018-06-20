using System;
using System.Collections.Generic;

namespace CCompiler.AbstractSyntaxTree
{
  public class Statement
  {
    public readonly Expression returnExp;

    public Statement()
    {
    }

    public Statement(Expression returnExp) =>
      this.returnExp = returnExp;

    public override bool Equals(object obj) => 
      obj is Statement statement &&
             EqualityComparer<Expression>.Default.Equals(returnExp, statement.returnExp);

    public override int GetHashCode() =>
      HashCode.Combine(returnExp);
  }
}
