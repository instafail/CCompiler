namespace CCompiler.AbstractSyntaxTree
{
  public class Statement
  {
    readonly public Expression returnExp;

    public Statement()
    {
    }

    public Statement(Expression returnExp) =>
      this.returnExp = returnExp;
  }
}
