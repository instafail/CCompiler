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
      ';', '{', '}', '(', ')'   // Char.IsWhiteSpace(c) = false
    };

    private static readonly HashSet<string> Keywords = new HashSet<string>
    {
      "int",
      "return"
    };

    private readonly List<Token> tokenList = new List<Token>();
    private readonly StringBuilder buffer = new StringBuilder();
    private readonly Char[] fileBuffer = new char[4096];

    private LexerMode mode;
    private int lineNumber;
    private string filePath;

    public List<Token> LexString(string sourceCode) =>
      this.LexString(sourceCode, string.Empty);

    public List<Token> LexString(string sourceCode, string filePath)
    {
      this.Init(filePath);
      this.LexSourceCode(sourceCode.AsSpan());
      this.EndOfLex();
      return this.tokenList;
    }

    public List<Token> LexFile(string filePath)
    {
      using (var fsStream = File.OpenText(filePath))
      {
        return this.LexFile(fsStream, filePath);
      }
    }

    public List<Token> LexFile(StreamReader fsStream, string filePath)
    {
      this.Init(filePath);
      while (!fsStream.EndOfStream)
      {
        var read = fsStream.ReadBlock(this.fileBuffer);
        this.LexSourceCode(this.fileBuffer.AsSpan(0, read));
      }

      this.EndOfLex();
      return this.tokenList;
    }

    private void LexSourceCode(ReadOnlySpan<char> sourceCode)
    {
      foreach (var c in sourceCode)
      {
        if (c == '\n')
        {
          this.lineNumber++;
        }

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
      }
    }

    private void Init(string filePath)
    {
      this.filePath = filePath;
      this.lineNumber = 1;
      this.mode = LexerMode.Normal;
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
