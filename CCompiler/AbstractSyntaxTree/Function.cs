using System.Collections.Generic;

namespace CCompiler.AbstractSyntaxTree
{
  public class Function
  {
    readonly public string name;
    readonly public List<Statement> statementList = new List<Statement>();
    
    public Function()
    {
     
    }

    public Function(string name, Statement statement)
    {
      this.name = name;
      this.statementList.Add(statement);
    }

    public Function(string name , List<Statement> statementList)
    {
      this.name = name;
      this.statementList.AddRange(statementList);
    }

  }
}
