using System;
using System.Collections.Generic;
using CCompiler.AbstractSyntaxTree;

namespace CCompiler
{
  public static class CC
  {

    public static Program Parse(List<Token> list)
    {
      if (list == null)
        throw new ArgumentNullException($"{nameof(list)} is Null");

      var parser = new Parser(list);

      return parser.Pars();
    }

    public static string Generate(Program p) =>
      Generater.Generate(p);

    public static Program LexAndParse(string source)
    {
      var tokens = Lexer.LexString(source);
      return Parse(tokens);
    }
  }
}
