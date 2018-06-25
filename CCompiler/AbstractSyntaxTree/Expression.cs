namespace CCompiler.AbstractSyntaxTree
{
  using System;
  using System.Collections.Generic;

  public abstract class Expression : IPrintable
  {
    public abstract string Print();
  }


  public class UnaryOp : Expression
  {
    public enum Type
    {
      Negation,
      Complement,
      BooleanNegation,
    }

    public static UnaryOp Negation(Expression expression) =>
      new UnaryOp(Type.Negation, expression); 

    public Expression expression;
    public Type type;

    public UnaryOp(Type type, Expression expression)
    {
      this.type = type;
      this.expression = expression;
    }

    public override string Print() =>
      "UnaryOp (" + type + "): \n";

    public override bool Equals(object obj) =>
      obj is UnaryOp op &&
             EqualityComparer<Expression>.Default.Equals(expression, op.expression) &&
             type == op.type;

    public override int GetHashCode() =>
      HashCode.Combine(expression, type);
  }

  public class Constant : Expression
  {
    public readonly int i;

    public Constant(int i) => 
      this.i = i;

    public override bool Equals(object obj) =>
      obj is Constant constant &&
             i == constant.i;

    public override int GetHashCode() =>
      HashCode.Combine(i);

    public override string Print() =>
      "Constant: " + i.ToString() + "\n";
  }

}
