using System;

namespace UcmdbFacade
{
  /// <summary>
  /// Ucmdb entity class marker
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
  public class UcmdbCiTypeAttribute : Attribute
  {
    public string Name { get; set; }

    public UcmdbCiTypeAttribute(string name)
    {
      Name = name;
    }
  }

  /// <summary>
  /// Ucmdb entity class attributes marker
  /// </summary>
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
