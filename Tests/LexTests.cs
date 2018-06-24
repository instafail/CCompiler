using System;
using System.Collections.Generic;
using CCompiler;
using Xunit;
using static CCompiler.TokenType;

namespace Tests
{
  public class LexTests
  {
    private readonly Lexer lexer = new Lexer();

    [Fact]
    public void EmptyStringReturnsEmptyList() =>
      Assert.Equal(lexer.Lex(""), new List<Token>());

    [Fact]
    public void SingleTokenReturnsToken() =>
      Assert.Equal(lexer.Lex("{"), new List<Token>() { new Token("{", OpenBrace) });

    [Fact]
    public void WhitespacesAreNeverReturned() =>
      Assert.Equal(lexer.Lex(" {"), new List<Token>() { new Token("{", OpenBrace) });

    [Fact]
    public void KeywordsAreReturned() =>
      Assert.Equal(lexer.Lex("return;"), new List<Token>()
      {
        new Token("return", Keyword),
        new Token(";", Semicolon),
      });

    [Fact]
    public void LexReturn2()
    {
      Assert.Equal(lexer.Lex("int main() { return 2; }"),
        new TokenList
        {
          { "int", Keyword},
          { "main", Identifier},
          { "(", OpenParen},
          { ")", CloseParen},
          { "{", OpenBrace},
          { "return", Keyword},
          { "2", Integer},
          { ";", Semicolon},
          { "}", CloseBrace},
        });
    }

    [Fact]
    public void LexReturn2AndComments() => 
      Assert.Equal(lexer.Lex("/*1*/int/*2*/main(/*3*/)/*4*/{/*5*/ return 2; }"),
        new TokenList
        {
          { "int", Keyword},
          { "main", Identifier},
          { "(", OpenParen},
          { ")", CloseParen},
          { "{", OpenBrace},
          { "return", Keyword},
          { "2", Integer},
          { ";", Semicolon},
          { "}", CloseBrace},
        });

    [Fact]
    public void LexCommentNotening()
    {
      var ex = Assert.Throws<Exception>(() => lexer.Lex("/* \n \n return 2; }"));
      Assert.Equal("unterminated comment", ex.Message);
    }

    public void LexCommentInComment1() =>
      lexer.Lex("/*/* return 2; }*/");

    public void LexCommentInComment2() =>
      lexer.Lex("/* return 2; /* }*/");

    [Fact]
    public void LexReturn2MinimumSpaces() =>
      Assert.Equal(lexer.Lex("int main(){return 2;}"),
        new TokenList
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
        });
  }
}
