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

      if (list.Count == 0)
      {
        return new Program();
      }

      var enumerator = list.GetEnumerator();
      enumerator.MoveNext();

      var program = new Program(ParseFunctions(enumerator));

      return program;
    }

    private static List<Function> ParseFunctions(IEnumerator<Token> enumerator)
    {
      var ret = new List<Function>();
      string name;

      var t = enumerator.Current;
      if (!Equals(t, new Token("int", TokenType.Keyword)))
      {
        throw new Exception("Return must be keyword!");
      }

      t = GetNext(enumerator);
      if (t.type == TokenType.Identifier)
      {
        name = t.text;
      }
      else
      {
        throw new Exception("Function name must be an identifier");
      }

      t = GetNext(enumerator);
      if (t.type != TokenType.OpenParen)
      {
        throw new Exception("");
      }

      t = GetNext(enumerator);
      if (t.type != TokenType.CloseParen)
      {
        throw new Exception("");
      }


      var p = ParseStatements(enumerator);
      ret.Add(new Function(name, p));

      return ret;
    }

    private static List<Statement> ParseStatements(IEnumerator<Token> enumerator)
    {
      var ret = new List<Statement>();
      var t = GetNext(enumerator);
      if (t.type != TokenType.OpenBrace)
      {
        throw new Exception("Open brace not found");
      }

      t = GetNext(enumerator);
      if (t.type == TokenType.Keyword && t.text == "return")
      {
        ret.Add(new Statement(ParseExpression(enumerator)));
      }
      else
      {
        throw new Exception("Not a recognized keyword");
      }

      if (GetNext(enumerator).type != TokenType.Semicolon)
      {
        throw new Exception("Missing semicolon at statement end");
      }

      t = GetNext(enumerator);
      if (t.type != TokenType.CloseBrace)
      {
        throw new Exception("Closing brace not found");
      }

      return ret;
    }

    private static Expression ParseExpression(IEnumerator<Token> enumerator)
    {
      try
      {
        return new Expression(int.Parse(GetNext(enumerator).text));
      }
      catch (FormatException)
      {
        throw new Exception("Bad integer constant");
      }
    }

    /// Modifies the enumerator!
    private static Token GetNext(IEnumerator<Token> enumerator)
    {
      if (enumerator.MoveNext())
      {
        return enumerator.Current;
      }
      else
      {
        throw new Exception("Unexpected end of tokens");
      }
    }

    private static string GenerateExpression(Expression e)
    {
      return "$" + e.i;
    }

    private static string GenerateStatement(Statement s)
    {
      return "\tmovl " + GenerateExpression(s.returnExp) + ", %eax\n" +
             "\tret\n";
    }

    public static string GenerateFunction(Function f)
    {
      return "\t.globl\t_main\n" +
             "_main:\n" + GenerateStatement(f.statementList[0]);
    }

    public static string Generate(Program p)
    {
      return GenerateFunction(p.functionList[0]);
    }

    public static Program LexAndParse(string source)
    {
      var lexer = new Lexer();
      var tokens = lexer.LexString(source);
      return Parse(tokens);
    }
  }
}
