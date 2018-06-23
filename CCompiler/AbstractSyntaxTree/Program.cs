using System;
using System.Linq;
using System.Collections.Generic;

namespace CCompiler.AbstractSyntaxTree
{
  public class Program : IPrintable
  {
    public readonly List<Function> functionList = new List<Function>();

    public Program()
    {
    }

    public Program(Function function) =>
      this.functionList.Add(function);

    public Program(List<Function> functionList) =>
      this.functionList.AddRange(functionList);

    public override bool Equals(object obj) =>
      obj is Program program && functionList.SequenceEqual(program.functionList);
    
    public string Print()
    {
      var ret = "Program: \n";
      foreach (var function in functionList)
      {
        var fString = function.Print().Split('\n');
        ret = fString.Aggregate(ret, (current, line) => current + ("\t" + line + "\n"));
      }

      return ret.TrimEnd();
    }

    public override int GetHashCode() =>
      HashCode.Combine(functionList);
  }
}
