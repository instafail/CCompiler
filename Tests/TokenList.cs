using CCompiler;
using System.Collections.Generic;

namespace Tests
{
  class TokenList : List<Token>
  {
    public void Add(string text, TokenType type) =>
      this.Add(new Token(text, type));
  }
}
