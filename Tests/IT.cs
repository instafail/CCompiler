using System;
using System.IO;
using CCompiler;
using Xunit;

namespace Tests
{
  public class IT
  {
    public static TheoryData<string> Valids = new TheoryData<string>();
    public static TheoryData<string> Invalids = new TheoryData<string>();

    static IT()
    {
      var i = Directory.GetFiles("TestData/Invalid");
      foreach (var item in i)
      {
        Invalids.Add(File.ReadAllText(item));
      }

      var v = Directory.GetFiles("TestData/Valid");
      foreach (var item in v)
      {
        Valids.Add(File.ReadAllText(item));
      }
    }

    [Theory]
    [MemberData(nameof(Valids))]
    public void ValidCases(string source)
    {
      CC.LexAndParse(source);
    }

    [Theory]
    [MemberData(nameof(Invalids))]
    public void InvalidCases(string source)
    {
      Assert.Throws<Exception>(() => CC.LexAndParse(source));
    }
  }
}
