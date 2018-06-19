using System.Collections.Generic;
using CCompiler;
using Xunit;
using static CCompiler.TokenType;

namespace Tests
{
  public class LexTests
  {
    [Fact]
    public void EmptyStringReturnsEmptyList()
    {
      Assert.Equal(CC.Lex(""), new List<Token>());
    }

    [Fact]
    public void SingleTokenReturnsToken()
    {
      Assert.Equal(CC.Lex("{"), new List<Token>() {new Token("{", OpenBrace)});
    }

    [Fact]
    public void WhitespacesAreNeverReturned()
    {
      Assert.Equal(CC.Lex(" {"), new List<Token>() {new Token("{", OpenBrace)});
    }

    [Fact]
    public void KeywordsAreReturned()
    {
      Assert.Equal(CC.Lex("return;"), new List<Token>()
      {
        new Token("return", Keyword),
        new Token(";", Semicolon),
      });
    }

    [Fact]
    public void LexReturn2()
    {
      Assert.Equal(CC.Lex("int main() { return 2; }"),
        new List<Token>()
        {
          new Token("int", Keyword),
          new Token("main", Identifier),
          new Token("(", OpenParen),
          new Token(")", CloseParen),
          new Token("{", OpenBrace),
          new Token("return", Keyword),
          new Token("2", Integer),
          new Token(";", Semicolon),
          new Token("}", CloseBrace),
        });
    }

    [Fact]
    public void LexReturn2MinimumSpaces()
    {
      Assert.Equal(CC.Lex("int main(){return 2;}"),
        new List<Token>()
        {
          new Token("int", Keyword),
          new Token("main", Identifier),
          new Token("(", OpenParen),
          new Token(")", CloseParen),
          new Token("{", OpenBrace),
          new Token("return", Keyword),
          new Token("2", Integer),
          new Token(";", Semicolon),
          new Token("}", CloseBrace),
        });
    }
  }
}
