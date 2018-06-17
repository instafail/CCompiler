namespace CCompiler
{
  public class Token
  {
    // These should most likely be readonly, immutable style
    // Have to add ctor then?
    public TokenType Type { get; set; }
    public string Text { get; set; }

    protected bool Equals(Token other)
    {
      return Type == other.Type && string.Equals(Text, other.Text);
    }

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
        return ((int) Type * 397) ^ (Text != null ? Text.GetHashCode() : 0);
      }
    }
  }

  public enum TokenType {
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
