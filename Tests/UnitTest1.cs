using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using CCompiler;

namespace Tests
{
  public class UnitTest1
  {
    [Fact]
    public void EmptyStringReturnsEmptyList()
    {
      Assert.Equal(CC.Lex(""), new List<string>());
    }

    [Fact]
    public void SingleTokenReturnsToken()
    {
      Assert.Equal(CC.Lex("{"), new List<string>() {"{"});
    }

    [Fact]
    public void WhitespacesAreNeverReturned()
    {
      Assert.Equal(CC.Lex(" {"), new List<string>() {"{"});
    }

    [Fact]
    public void KeywordsAreReturned()
    {
      Assert.Equal(CC.Lex("return;"), new List<string>() {"return", ";"});
    }

    [Fact]
    public void LexReturn2()
    {
      Assert.Equal(CC.Lex("int main() { return 2; }"),
        new List<string>() { "int", "main", "(", ")", "{", "return", "2", ";", "}"});
    }
  }
}
