using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ucmdb
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
  public class UcmdbCiTypeAttribute : Attribute
  {
    public string Name { get; set; }

    public UcmdbCiTypeAttribute(string name)
    {
      Name = name;
    }
  }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class UcmdbAttributeAttribute : Attribute
  {
    public string Name { get; set; }

    public UcmdbAttributeAttribute(string name)
    {
      Name = name;
    }
  }
}
