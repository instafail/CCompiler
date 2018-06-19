using CCompiler.AbstractSyntaxTree;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CCompiler
{
  public static class CC
  {
    private static readonly HashSet<string> Keywords = new HashSet<string>()
    {
      "int",
      "return"
    };

    public static Program Parse(List<Token> list)
    {
      if (list == null)
        throw new ArgumentNullException($"{nameof(list)} is Null");

      var program = new Program();


      return program;
    }

    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }

    public static List<Token> Lex(string s)
    {
      var ret = new List<Token>();
      var buf = ""; // This may have to become a char[] for performance later...
      var wordBreakers = new HashSet<char>() {'{', '}', '(', ')', ';', ' '};
      foreach (var c in s)
      {
        if (wordBreakers.Contains(c))
        {
          if (!string.IsNullOrWhiteSpace(buf))
            ret.Add(Classify(buf));
          if (!char.IsWhiteSpace(c))
          {
            if (wordBreakers.Contains(c))
            {
              ret.Add(Classify(c));
              buf = "";
            }
            else
              buf = c.ToString();
          }
          else
            buf = "";
        }
        else
        {
          buf += c.ToString();
        }
      }

      if (!string.IsNullOrWhiteSpace(buf))
        ret.Add(Classify(buf));

      return ret;
    }

    private static Token Classify(char token)
    {
      return Classify(token.ToString());
    }

    private static Token Classify(string token)
    {
      TokenType type;
      // This could probably be made into a Map, then iterated over.
      // Needs to be OrderedMap, since keywords should be matched before identifiers
      if (token.Equals("{"))
        type = TokenType.OpenBrace;
      else if (token.Equals("}"))
        type = TokenType.CloseBrace;
      else if (token.Equals("("))
        type = TokenType.OpenParen;
      else if (token.Equals(")"))
        type = TokenType.CloseParen;
      else if (token.Equals(";"))
        type = TokenType.Semicolon;
      else if (Regex.IsMatch(token, "^[0-9]+$")) // Static compile of regex?
        type = TokenType.Integer;
      else if (Keywords.Contains(token))
        type = TokenType.Keyword;
      else
        type = TokenType.Identifier;

      return new Token(token, type);
    }
  }
}
