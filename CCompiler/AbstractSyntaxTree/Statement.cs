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
  }
}
