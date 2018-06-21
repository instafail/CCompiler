using System;
using System.IO;

namespace CCompiler
{
  class ProgramMain
  {
    static void Main(string[] args)
    {
      var source = File.ReadAllText(args[0]);
      var ast = CC.LexAndParse(source);
      Console.WriteLine(ast.Print());
      var asm = CC.Generate(ast);
      File.WriteAllText("out.s", asm);
    }
  }
}
