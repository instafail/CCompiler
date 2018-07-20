namespace CCompiler
{
  using CCompiler.AbstractSyntaxTree;
  using System;
  using System.Collections.Generic;
  public class Parser
  {
    private IEnumerator<Token> enumerator;

    public Parser(IEnumerable<Token> enumerable) => 
      this.enumerator = enumerable.GetEnumerator();

    private Token Current => this.enumerator.Current;

    private string CurrentText => this.enumerator.Current.text;

    private TokenType CurrentType => this.enumerator.Current.type;

    public Program Pars()
    {
      return enumerator.MoveNext() ?
        new Program(ParseFunctions()) : new Program();
    }

    private List<Function> ParseFunctions()
    {
      string name;

      if (!Equals(this.Current, new Token("int", TokenType.Keyword)))
      {
        throw new Exception("Return must be keyword!");
      }

      this.Next();
      if (this.CurrentType == TokenType.Identifier)
      {
        name = this.Current.text;
      }
      else
      {
        throw new Exception("Function name must be an identifier");
      }

      this.Next();
      if (this.CurrentType != TokenType.OpenParen)
      {
        throw new Exception("");
      }

      this.Next();
      if (this.CurrentType != TokenType.CloseParen)
      {
        throw new Exception("");
      }

      return new List<Function>() {
        new Function(name, this.ParseStatements())
      };
    }

    private List<Statement> ParseStatements()
    {
      var ret = new List<Statement>();
      this.Next();
      if (this.CurrentType != TokenType.OpenBrace)
      {
        throw new Exception("Open brace not found");
      }

      this.Next();
      if (this.CurrentType == TokenType.Keyword && this.CurrentText == "return")
      {
        ret.Add(new Statement(this.ParseExpression()));
      }
      else
      {
        throw new Exception("Not a recognized keyword");
      }

      this.Next();
      if (this.CurrentType != TokenType.Semicolon)
      {
        throw new Exception("Missing semicolon at statement end");
      }

      this.Next();
      if (this.CurrentType != TokenType.CloseBrace)
      {
        throw new Exception("Closing brace not found");
      }

      return ret;
    }

    private Expression ParseExpression()
    {
      this.Next();
      try
      {
        switch (this.CurrentType)
        {
          case TokenType.Integer:
            return new Constant(int.Parse(this.CurrentText));
          case TokenType.Negation:
            return UnaryOp.Negation(this.ParseExpression());
          default:
            throw new Exception("Bad Expression");
        }
      }
      catch (FormatException)
      {
        throw new Exception("Bad numeric");
      }
    }

    private void Next()
    {
      if (!this.enumerator.MoveNext())
      {
        throw new Exception("Unexpected end of tokens");
      }
    }

  }
}
