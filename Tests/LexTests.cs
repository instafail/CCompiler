using System;
using System.Collections.Generic;
using CCompiler;
using Xunit;
using static CCompiler.TokenType;

namespace Tests
{
  public class LexTests
  {
    private readonly TokenList return2expect = new TokenList
    {
      {"int", Keyword},
      {"main", Identifier},
      {"(", OpenParen},
      {")", CloseParen},
      {"{", OpenBrace},
      {"return", Keyword},
      {"2", Integer},
      {";", Semicolon},
      {"}", CloseBrace},
    };

    [Fact]
    public void EmptyStringReturnsEmptyList() =>
      Assert.Equal(Lexer.LexString(""), new List<Token>());

    [Fact]
    public void SingleTokenReturnsToken() =>
      Assert.Equal(Lexer.LexString("{"), new List<Token>() { new Token("{", OpenBrace) });

    [Fact]
    public void WhitespacesAreNeverReturned() =>
      Assert.Equal(Lexer.LexString(" {"), new List<Token>() { new Token("{", OpenBrace) });

    [Fact]
    public void KeywordsAreReturned() =>
      Assert.Equal(Lexer.LexString("return;"), new List<Token>()
      {
        new Token("return", Keyword),
        new Token(";", Semicolon),
      });

    [Fact]
    public void LexReturn2() =>
      Assert.Equal(Lexer.LexString("int main() { return 2; }"), return2expect);

    [Fact]
    public void LexReturn2AndComments() =>
      Assert.Equal(Lexer.LexString("/*1*/int/*2*/main(/*3*/)/*4*/{/*5*/ return 2; }"),
        return2expect);

    [Fact]
    public void LexCommentNotening()
    {
      var ex = Assert.Throws<Exception>(() => Lexer.LexString("/* \n \n return 2; }"));
      Assert.Equal("unterminated comment", ex.Message);
    }

    [Fact]
    public void LexCommentInCommentDoesNotThrow() =>
      Lexer.LexString("/*/* return 2; }*/");

    [Fact]
    public void LexReturn2MinimumSpaces() =>
      Assert.Equal(Lexer.LexString("int main(){return 2;}"), return2expect);

    [Fact]
    public void LexReturn2WithLinebreaks() =>
      Assert.Equal<Token>(
        Lexer.LexString("\nint\nmain\n(\n)\n{\nreturn\n2\n;\n}\n"),
        new TokenList
        {
          {"int", Keyword, 2},
          {"main", Identifier, 3},
          {"(", OpenParen, 4},
          {")", CloseParen, 5},
          {"{", OpenBrace, 6},
          {"return", Keyword, 7},
          {"2", Integer, 8},
          {";", Semicolon, 9},
          {"}", CloseBrace, 10},
        }, new TokenTestComparer());

    [Fact]
    public void LexNegation() =>
      Assert.Equal(Lexer.LexString("-1"),
        new TokenList
        {
          {"-", Negation},
          {"1", Integer},
        });


  }
}
