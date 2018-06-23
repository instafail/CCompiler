namespace CCompiler
{
  public class Token
  {
    public readonly TokenType type;
    public readonly string text;

    /// <summary>
    /// origin index in file , do not have this in Equals
    /// </summary>
    public readonly int fileIndex;

    public Token(string text, TokenType type)
      : this(text, type, -1)
    {
    }

    public Token(string text, TokenType type, int fileIndex)
    {
      this.text = text;
      this.type = type;
      this.fileIndex = fileIndex;
    }

    protected bool Equals(Token other) =>
      type == other.type && string.Equals(text, other.text);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Token) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((int) type * 397) ^ (text?.GetHashCode() ?? 0);
      }
    }

    public override string ToString() =>
      $"{text} ({fileIndex}) : {type}";
  }

  public enum TokenType
  {
    Keyword,
    Identifier,
    OpenBrace,
    CloseBrace,
    OpenParen,
    CloseParen,
    Semicolon,
    Integer
  }
}
