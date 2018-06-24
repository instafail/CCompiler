namespace CCompiler
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Text.RegularExpressions;

  public class Lexer
  {
    [Flags]
    private enum LexerMode
    {
      Normal = 1 << 0,

      String = 1 << 1,
      StringEscape = 1 << 2,

      CommentStart = 1 << 3,
      Comment = 1 << 4,
      CommentEnd = 1 << 5,
    }

    private static readonly HashSet<char> wordBreakers = new HashSet<char> {
      ' ', '\n', '\r', '\t',    // Char.IsWhiteSpace(c) = true
      ';', '{', '}', '(', ')'   // Char.IsWhiteSpace(c) = false
    };

    private static readonly HashSet<string> Keywords = new HashSet<string>
    {
      "int",
      "return"
    };

    public List<Token> Lex(string sourceCode)
    {
      var ret = new List<Token>();
      var mode = LexerMode.Normal;
      var buffer = new StringBuilder();
      int lineNumber = 0;

      void ClassifyStrBuilder()
      {
        if (buffer.Length != 0)
        {
          ret.Add(Classify(buffer.ToString(), lineNumber));
          buffer.Clear();
        }
      }
      
      foreach (var c in sourceCode)
      {
        if(c == '\n')
        {
          lineNumber++;
        }

        switch (mode)
        {
          case LexerMode.Normal:
            if (wordBreakers.Contains(c))
            {
              ClassifyStrBuilder();
              if (!Char.IsWhiteSpace(c))
              {
                ret.Add(Classify(c, lineNumber));
              }
              break;
            }

            if (c == '"')
            {
              ClassifyStrBuilder();
              mode = LexerMode.String;
            }
            else if (c == '/')
            {
              ClassifyStrBuilder();
              mode = LexerMode.CommentStart;
            }
            else
            {
              buffer.Append(c);
            }

            break;

          case LexerMode.String:
            if (c == '\\')
              mode = LexerMode.StringEscape;
            else if (c == '"')
              throw new NotImplementedException("add string token");
            else
              buffer.Append(c);
            break;

          case LexerMode.StringEscape:
            throw new NotImplementedException("Escape Stirng");

          case LexerMode.CommentStart:
            if (c == '*')
            {
              mode = LexerMode.Comment;
              break;
            }
            else
            {
              mode = LexerMode.Normal;
              goto case LexerMode.Normal;
            }

          case LexerMode.Comment:
            if (c == '*')
              mode = LexerMode.CommentEnd;
            break;

          case LexerMode.CommentEnd:
            mode = c == '/' ? LexerMode.Normal : LexerMode.Comment;
            break;

          default:
            throw new Exception("?");
        }
      }

      ClassifyStrBuilder();

      if (((LexerMode.Comment | LexerMode.CommentEnd) & mode) != 0)
      {
        throw new Exception("unterminated comment");
      }

      if (((LexerMode.String | LexerMode.StringEscape) & mode) != 0)
      {
        throw new Exception("unterminated string");
      }

      return ret;
    }

    private Token Classify(char token, int fileIndex) =>
      Classify(token.ToString(), fileIndex);

    private Token Classify(string token, int lineNumber)
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
