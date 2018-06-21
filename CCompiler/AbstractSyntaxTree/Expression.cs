namespace CCompiler.AbstractSyntaxTree
{
  using System;

  public class Expression : IPrintable
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

    public string Print()
    {
      return "Expression: " + i.ToString() + "\n";
    }
  }
}
