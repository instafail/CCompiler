namespace CCompiler
{
  public class Token
  {
    readonly public TokenType type;
    readonly public string text;
    
    public Token(string text , TokenType type )
    {
      this.text = text;
      this.type = type;
    }

    protected bool Equals(Token other)
    {
      return type == other.type && string.Equals(text, other.text);
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
        return ((int) type * 397) ^ (text?.GetHashCode() ?? 0);
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
