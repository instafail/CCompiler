namespace CCompiler
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Text;
  using System.Text.RegularExpressions;
  public class Lexer
  {
    private static readonly HashSet<char> wordBreakers = new HashSet<char> {
      ' ', '\n', '\r', '\t',    // Char.IsWhiteSpace(c) = true
      ';', '{', '}', '(', ')', '-',   // Char.IsWhiteSpace(c) = false
    };

    private static readonly HashSet<string> Keywords = new HashSet<string>
    {
      "int",
      "return"
    };

    private readonly List<Token> tokenList = new List<Token>();
    private readonly StringBuilder buffer = new StringBuilder();
    private readonly Char[] fileBuffer = new char[4096];

    private LexerMode mode = LexerMode.Normal;
    private int lineNumber = 1;
    private string filePath;

    private Lexer(string filePath) => 
      this.filePath = filePath;

    public static List<Token> LexString(string sourceCode) =>
      LexString(sourceCode, string.Empty);

    public static List<Token> LexString(string sourceCode, string filePath)
    {
      var lexer = new Lexer(filePath);
      lexer.LexSourceCode(sourceCode.AsSpan());
      lexer.EndOfLex();
      return lexer.tokenList;
    }

    public static List<Token> LexFile(string filePath)
    {
      using (var fsStream = File.OpenText(filePath))
      {
        return LexFile(fsStream, filePath);
      }
    }

    public static List<Token> LexFile(StreamReader fsStream, string filePath)
    {
      var lexer = new Lexer(filePath);
      while (!fsStream.EndOfStream)
      {
        var fileBuffer = lexer.fileBuffer;
        var read = fsStream.ReadBlock(fileBuffer);
        lexer.LexSourceCode(fileBuffer.AsSpan(0, read));
      }

      lexer.EndOfLex();
      return lexer.tokenList;
    }

    private void LexSourceCode(ReadOnlySpan<char> sourceCode)
    {
      foreach (var c in sourceCode)
      {
        switch (this.mode)
        {
          case LexerMode.Normal:
            if (wordBreakers.Contains(c))
            {
              this.ClassifyStrBuilder();
              if (!Char.IsWhiteSpace(c))
              {
                this.tokenList.Add(Classify(c));
              }
              break;
            }

            if (c == '"')
            {
              this.ClassifyStrBuilder();
              this.mode = LexerMode.String;
            }
            else if (c == '/')
            {
              this.ClassifyStrBuilder();
              this.mode = LexerMode.CommentStart;
            }
            else
            {
              this.buffer.Append(c);
            }

            break;

          case LexerMode.String:
            if (c == '\\')
              this.mode = LexerMode.StringEscape;
            else if (c == '"')
              throw new NotImplementedException("add string token");
            else
              this.buffer.Append(c);
            break;

          case LexerMode.StringEscape:
            throw new NotImplementedException("Escape Stirng");

          case LexerMode.CommentStart:
            if (c == '*')
            {
              this.mode = LexerMode.Comment;
              break;
            }
            else
            {
              this.mode = LexerMode.Normal;
              goto case LexerMode.Normal;
            }

          case LexerMode.Comment:
            if (c == '*')
              this.mode = LexerMode.CommentEnd;
            break;

          case LexerMode.CommentEnd:
            this.mode = c == '/' ? LexerMode.Normal : LexerMode.Comment;
            break;

          default:
            throw new Exception("?");
        }

        if (c == '\n')
        {
          this.lineNumber++;
        }
      }
    }

    private void EndOfLex()
    {
      this.ClassifyStrBuilder();

      if (((LexerMode.Comment | LexerMode.CommentEnd) & mode) != 0)
      {
        throw new Exception("unterminated comment");
      }

      if (((LexerMode.String | LexerMode.StringEscape) & mode) != 0)
      {
        throw new Exception("unterminated string");
      }
    }

    private void ClassifyStrBuilder()
    {
      if (this.buffer.Length != 0)
      {
        this.tokenList.Add(Classify(this.buffer.ToString()));
        this.buffer.Clear();
      }
    }

    private Token Classify(char token) =>
      Classify(token.ToString());

    private Token Classify(string token)
    {
      TokenType type;

      void ClassifyInner()
      {
        if (Regex.IsMatch(token, "^[0-9]+$")) // Static compile of regex?
          type = TokenType.Integer; // 1 2 3 11
        else if (Keywords.Contains(token))
          type = TokenType.Keyword; // int return 
        else
          type = TokenType.Identifier;
      }

      if (token.Length == 1)
      {
        switch (token[0])
        {
          case '{':
            type = TokenType.OpenBrace;
            break;
          case '}':
            type = TokenType.CloseBrace;
            break;
          case '(':
            type = TokenType.OpenParen;
            break;
          case ')':
            type = TokenType.CloseParen;
            break;
          case ';':
            type = TokenType.Semicolon;
            break;
          case '-':
            type = TokenType.Negation;
            break;

          default:
            ClassifyInner();
            break;
        }
      }
      else
      {
        ClassifyInner();
      }

      return new Token(token, type, this.filePath, this.lineNumber);
    }
  }
}
