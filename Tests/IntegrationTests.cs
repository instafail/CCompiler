using System;
using System.IO;
using CCompiler;
using Xunit;

namespace Tests
{
  public class IntegrationTests
  {
    public class FilePathAndFileContent
    {
      public string Path { get; set; }
      public string Content { get; set; }
      public FilePathAndFileContent(string path) =>
        this.Content = File.ReadAllText((this.Path = path));
      public override string ToString() =>
        $"{this.Path} {this.Content}";
    }

    public static TheoryData<FilePathAndFileContent> Valids =
      new TheoryData<FilePathAndFileContent>();
    public static TheoryData<FilePathAndFileContent> Invalids =
      new TheoryData<FilePathAndFileContent>();

    static IntegrationTests()
    {
      var i = Directory.GetFiles("TestData/Invalid");
      foreach (var path in i)
      {
        Invalids.Add(new FilePathAndFileContent(path));
      }

      var v = Directory.GetFiles("TestData/Valid");
      foreach (var path in v)
      {
        Valids.Add(new FilePathAndFileContent(path));
      }
    }

    [Theory]
    [MemberData(nameof(Valids))]
    public void ValidCases(FilePathAndFileContent file) => 
      CC.LexAndParse(file.Content);

    [Theory]
    [MemberData(nameof(Invalids))]
    public void InvalidCases(FilePathAndFileContent file) => 
      Assert.Throws<Exception>(() => CC.LexAndParse(file.Content));
  }
}
