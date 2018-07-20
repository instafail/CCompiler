namespace CCompiler
{
  using System;
  using CCompiler.AbstractSyntaxTree;
  internal class Generater
  {
    private Program program;

    internal Generater(Program program) =>
      this.program = program;

    internal string Generate() =>
      this.GenerateFunction(this.program.functionList[0]);

    private string GenerateExpression(Expression e)
    {
      if (e is Constant c)
        return "$" + c.i;
      if (e is UnaryOp u)
      {
        switch (u.type)
        {
          case UnaryOp.Type.Negation:
            var ret = "\tmovl " + this.GenerateExpression(u.expression) + ", %ebx\n";
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

    private string GenerateStatement(Statement s) =>
      this.GenerateExpression(s.returnExp) +
        "\tmovl %ebx, %eax\n" +
        "\tret\n";

    private string GenerateFunction(Function f) =>
      "\t.globl\t_main\n" +
      "_main:\n" + GenerateStatement(f.statementList[0]);
  }
}
