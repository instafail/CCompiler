using System.Collections.Generic;
using CCompiler;
using Xunit;

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
      Assert.Equal(CC.Lex("{"), new List<Token>() {new Token() {Type = TokenType.OpenBrace, Text = "{"}});
    }

    [Fact]
    public void WhitespacesAreNeverReturned()
    {
      Assert.Equal(CC.Lex(" {"), new List<Token>() {new Token() {Text = "{", Type = TokenType.OpenBrace}});
    }

    [Fact]
    public void KeywordsAreReturned()
    {
      Assert.Equal(CC.Lex("return;"), new List<Token>()
      {
        new Token() {Text = "return", Type = TokenType.Keyword},
        new Token() {Text = ";", Type = TokenType.Semicolon}
      });
    }

    [Fact]
    public void LexReturn2()
    {
      Assert.Equal(CC.Lex("int main() { return 2; }"),
        new List<Token>()
        {
          new Token() {Text = "int", Type = TokenType.Keyword},
          new Token() {Text = "main", Type = TokenType.Identifier},
          new Token() {Text = "(", Type = TokenType.OpenParen},
          new Token() {Text = ")", Type = TokenType.CloseParen},
          new Token() {Text = "{", Type = TokenType.OpenBrace},
          new Token() {Text = "return", Type = TokenType.Keyword},
          new Token() {Text = "2", Type = TokenType.Integer},
          new Token() {Text = ";", Type = TokenType.Semicolon},
          new Token() {Text = "}", Type = TokenType.CloseBrace}
        });
    }
    
    [Fact]
    public void LexReturn2MinimumSpaces()
    {
      Assert.Equal(CC.Lex("int main(){return 2;}"),
        new List<Token>()
        {
          new Token() {Text = "int", Type = TokenType.Keyword},
          new Token() {Text = "main", Type = TokenType.Identifier},
          new Token() {Text = "(", Type = TokenType.OpenParen},
          new Token() {Text = ")", Type = TokenType.CloseParen},
          new Token() {Text = "{", Type = TokenType.OpenBrace},
          new Token() {Text = "return", Type = TokenType.Keyword},
          new Token() {Text = "2", Type = TokenType.Integer},
          new Token() {Text = ";", Type = TokenType.Semicolon},
          new Token() {Text = "}", Type = TokenType.CloseBrace}
        });
    }
  }
}
