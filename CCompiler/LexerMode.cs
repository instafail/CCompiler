namespace CCompiler
{
  using System;

  [Flags]
  internal enum LexerMode
  {
    Normal = 1 << 0,

    String = 1 << 1,
    StringEscape = 1 << 2,

    CommentStart = 1 << 3,
    Comment = 1 << 4,
    CommentEnd = 1 << 5,
  }
}
