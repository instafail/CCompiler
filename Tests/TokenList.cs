using CCompiler;
using System;
using System.Collections.Generic;

namespace Tests
{
  class TokenList : List<Token>
  {
    public void Add(string text, TokenType type) =>
      this.Add(new Token(text, type));

    public void Add(string text, TokenType type, int lineNumber) =>
      this.Add(new Token(text, type, String.Empty, lineNumber));
  }
  
  class TokenTestComparer : IEqualityComparer<Token>
  {
    public bool Equals(Token x, Token y) =>
      x.Equals(y) && x.lineNumber == y.lineNumber && x.filePath.Equals(y.filePath);

    public int GetHashCode(Token obj) =>
      HashCode.Combine(obj, obj.lineNumber, obj.filePath);
  }
}
