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

      if(list.Count == 0)
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
      var name = "";

      var t = enumerator.Current;
      if (t != new Token("int", TokenType.Keyword))
      {
        throw new Exception("Returntype must be keyword!");
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

      t = GetNext(enumerator);
      if (t.type != TokenType.OpenBrace)
      {
        throw new Exception("");
      }

      // Parse body
      List<Statement> p = ParseStatements(enumerator);
      ret.Add(new Function(name, p));

      if (enumerator.Current.type != TokenType.CloseBrace)
      {
        throw new Exception("Closing brace not found");
      }
      return ret;
    }

    private static List<Statement> ParseStatements(IEnumerator<Token> enumerator)
    {
      throw new NotImplementedException();
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

    public static List<Token> Lex(string s)
    {
      var ret = new List<Token>();
      var buf = ""; // This may have to become a char[] for performance later...
      var wordBreakers = new HashSet<char>() { '{', '}', '(', ')', ';', ' ' };
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
        type = TokenType.Integer; // 1 2 3 11
      else if (Keywords.Contains(token))
        type = TokenType.Keyword; // int return 
      else
        type = TokenType.Identifier;

      return new Token(token, type);
    }
  }
}
