using System.Collections.Generic;

namespace CCompiler.AbstractSyntaxTree
{
  public class Program
  {
    public readonly List<Function> functionList = new List<Function>();

    public Program()
    {
    }

    public Program(Function function) =>
      this.functionList.Add(function);

    public Program(List<Function> functionList) =>
      this.functionList.AddRange(functionList);

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj) =>
      obj?.GetType() == typeof(Program);
  }
}
