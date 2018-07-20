using System;
using System.Collections.Generic;
using CCompiler.AbstractSyntaxTree;

namespace CCompiler
{
  public static class CC
  {

    public static Program Parse(List<Token> list)
    {
      if (list == null)
        throw new ArgumentNullException($"{nameof(list)} is Null");

      var parser = new Parser(list);

      return parser.Pars();
    }

    private static string GenerateExpression(Expression e)
    {
      if (e is Constant c)
        return "$" + c.i;
      if (e is UnaryOp u)
      {
        switch (u.type)
        {
          case UnaryOp.Type.Negation:
            var ret = "\tmovl " + GenerateExpression(u.expression) + ", %ebx\n";
            return ret + "\tneg %ebx\n";
          default:
            throw new NotImplementedException();
        }
      }
      throw new NotImplementedException();
    }
    /*
        RAX - 64 ------------ EAX - 32 ------ AX 16 AH 8, AL, 8

        movl $2, %eax
        neg %eax
        ret
        */

    private static string GenerateStatement(Statement s)
    {
      return GenerateExpression(s.returnExp) +
        "\tmovl %ebx, %eax\n" +
        "\tret\n";
    }

    public static string GenerateFunction(Function f)
    {
      return "\t.globl\t_main\n" +
             "_main:\n" + GenerateStatement(f.statementList[0]);
    }

    public static string Generate(Program p)
    {
      return GenerateFunction(p.functionList[0]);
    }

    public static Program LexAndParse(string source)
    {
      var tokens = Lexer.LexString(source);
      return Parse(tokens);
    }
  }
}
