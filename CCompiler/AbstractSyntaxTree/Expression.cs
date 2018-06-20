namespace CCompiler.AbstractSyntaxTree
{
  using System;

  public class Expression
  {
    public readonly int i;

    public Expression(int i)
    {
      this.i = i;
    }

    public override bool Equals(object obj) =>
      obj is Expression expression && i == expression.i;

    public override int GetHashCode() =>
      HashCode.Combine(i);
  }
}
