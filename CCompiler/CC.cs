using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace CCompiler
{
  public class CC
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }

    public static List<string> Lex(string s)
    {
      var ret = new List<string>();
      String buf = "";
      var singles = new HashSet<char>() {'{', '}', '(', ')', ';'};
      foreach (var c in s)
      {
        if (c.Equals(' '))
        {
          if (!String.IsNullOrWhiteSpace(buf))
            ret.Add(buf);
          buf = "";
        }
        else if (singles.Contains(c))
        {
          if (!String.IsNullOrWhiteSpace(buf))
            ret.Add(buf);
          ret.Add(c.ToString());
          buf = "";
        }
        else
        {
          buf += c;
        }
      }

      return ret;
    }
  }
}
