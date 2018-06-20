using CCompiler;
using System.Linq;
using System.Collections.Generic;

namespace Tests
{
  class TokenList : Dictionary<string, CCompiler.TokenType>
  {
    public static implicit operator List<Token>(TokenList list) =>
      list.Select(x => new Token(x.Key, x.Value)).ToList();
  }
}
