using System.Collections.Generic;

namespace CCompiler.AbstractSyntaxTree
{
  public class Function
  {
    public readonly string name;
    public readonly List<Statement> statementList = new List<Statement>();

    public Function()
    {
    }

    public Function(string name, Statement statement)
    {
      this.name = name;
      this.statementList.Add(statement);
    }

    public Function(string name, List<Statement> statementList)
    {
      this.name = name;
      this.statementList.AddRange(statementList);
    }
  }
}
