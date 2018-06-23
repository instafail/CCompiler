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

      Stirng = 1 << 1,
      StirngEscape = 1 << 2,

      CommentStart = 1 << 3,
      Comment = 1 << 4,
      CommentEed = 1 << 5,
    }

    private static readonly HashSet<char> endWordBreakers = new HashSet<char> { ' ', '\n', '\r' };
    private static readonly HashSet<char> wordBreakers = new HashSet<char> { ';', '{', '}', '(', ')' };

    private static readonly HashSet<string> Keywords = new HashSet<string>
    {
      "int",
      "return"
    };

    public List<Token> Lex(string sourceCode)
    {
      var ret = new List<Token>();
      var mode = LexerMode.Normal;
      var strBuilder = new StringBuilder();
      int fileIndex = -1;

      void ClassifyStrBuilder()
      {
        if (strBuilder.Length != 0)
        {
          ret.Add(Classify(strBuilder.ToString(), fileIndex));
          strBuilder.Clear();
        }
      }

      foreach (var c in sourceCode)
      {
        fileIndex++;
        switch (mode)
        {
          case LexerMode.Normal:
            if (endWordBreakers.Contains(c))
            {
              ClassifyStrBuilder();
              break;
            }

            if (wordBreakers.Contains(c))
            {
              ClassifyStrBuilder();
              ret.Add(Classify(c, fileIndex));
              break;
            }

            if (c == '"')
            {
              ClassifyStrBuilder();
              mode = LexerMode.Stirng;
            }
            else if (c == '/')
            {
              ClassifyStrBuilder();
              mode = LexerMode.CommentStart;
            }
            else
            {
              strBuilder.Append(c);
            }
            break;

          case LexerMode.Stirng:
            if (c == '\\')
              mode = LexerMode.StirngEscape;
            else if (c == '"')
              throw new NotImplementedException("add string token");
            else
              strBuilder.Append(c);
            break;

          case LexerMode.StirngEscape:
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
              mode = LexerMode.CommentEed;
            break;

          case LexerMode.CommentEed:
            mode = c == '/' ? LexerMode.Normal : LexerMode.Comment;
            break;

          default:
            throw new Exception("?");
        }
      }

      ClassifyStrBuilder();

      if (((LexerMode.Comment | LexerMode.CommentEed) & mode) != 0)
      {
        throw new Exception("unterminated comment");
      }

      if(((LexerMode.Stirng | LexerMode.StirngEscape) & mode) != 0)
      {
        throw new Exception("unterminated string");
      }

      return ret;
    }

    private Token Classify(char token, int fileIndex) =>
      Classify(token.ToString(), fileIndex);

    private Token Classify(string token, int fileIndex)
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

      return new Token(token, type, fileIndex);
    }
  }
}
