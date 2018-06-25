namespace CCompiler
{
  public class Token
  {
    public readonly TokenType type;
    public readonly string text;
    public readonly int lineNumber;
    public readonly string filePath;

    public Token(string text, TokenType type)
      : this(text, type, string.Empty, -1)
    {
    }

    public Token(string text, TokenType type, string filePath, int lineNumber)
    {
      this.text = text;
      this.type = type;
      this.filePath = filePath;
      this.lineNumber = lineNumber;
    }

    protected bool Equals(Token other) =>
      type == other.type && string.Equals(text, other.text);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Token)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((int)type * 397) ^ (text?.GetHashCode() ?? 0);
      }
    }

    public override string ToString() =>
      $"{text} ({filePath}:{lineNumber}) : {type}";
  }

  public enum TokenType
  {
    Keyword,

    OpenBrace,
    CloseBrace,

    OpenParen,
    CloseParen,

    Semicolon,

    Integer,

    Identifier,
    Negation,
  }
}
