using System.IO;

namespace CCompiler
{
  class ProgramMain
  {
    static void Main(string[] args)
    {
      var source = File.ReadAllText(args[0]);
      var asm = CC.Generate(CC.LexAndParse(source));
      File.WriteAllText("out.s", asm);
    }
  }
}
