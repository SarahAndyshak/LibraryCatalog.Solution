using System.Collections.Generic;
using System;

namespace ProjectName.Models
{
  public class ClassName
  {
    private int _side1;
    public int Side1
    {
      get { return _side1; }
      set { _side1 = value; }
    }
    public int Side2 { get; set; }
    private int _side3;
    private static List<ClassName> _instances = new List<ClassName> {};

    public ClassName(int length1, int length2, int length3)
    {
      _side1 = length1;
      Side2 = length2;
      _side3 = length3;
      _instances.Add(this);
    }

  }
}