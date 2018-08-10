namespace CCompiler
{
  using System;
  using CCompiler.AbstractSyntaxTree;
  internal static class Generater
  {
    internal static string Generate(Program program) =>
      GenerateFunction(program.functionList[0]);

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

    private static string GenerateStatement(Statement s) =>
      GenerateExpression(s.returnExp) +
        "\tmovl %ebx, %eax\n" +
        "\tret\n";

    private static string GenerateFunction(Function f) =>
      "\t.globl\t_main\n" +
      "_main:\n" + GenerateStatement(f.statementList[0]);
  }
}
