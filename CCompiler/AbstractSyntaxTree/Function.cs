namespace CCompiler.AbstractSyntaxTree
{
  using System;
  using System.Linq;
  using System.Collections.Generic;

  public class Function : IPrintable
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

    public override bool Equals(object obj)
    {
      return obj is Function function &&
             name == function.name && statementList.SequenceEqual(function.statementList);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(name, statementList);
    }

    public string Print()
    {
      var ret = "Function: " + name + " \n";
      foreach (var statement in statementList)
      {
        var fString = statement.Print().Split('\n');
        ret = fString.Aggregate(ret, (current, line) => current + ("\t" + line + "\n"));
      }

      return ret;
    }
  }
}
