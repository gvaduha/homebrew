using System;
using System.Collections.Generic;
using System.Linq;

namespace UcmdbFacade
{
  /// <summary>
  /// Set of various extension functions to work with classes tagged with UcmdbAttributes
  /// </summary>
  public static class UcmdbEntitiesExtensions
  {
    /// <summary>
    /// Return type's UcmdbCiTypeAttribute.Name attribute
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetUcmdbTypeAttribute(this Type type)
    {
      return ((UcmdbCiTypeAttribute)type.GetCustomAttributes(typeof (UcmdbCiTypeAttribute), true).First()).Name;
    }

    /// <summary>
    /// Return collection of all UcmdbAttributeAttribute.Name for object.properties
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IEnumerable<string> AllUcmdbAttributedProperties(this object obj)
    {
      return AllUcmdbAttributedProperties(obj.GetType());
    }

    /// <summary>
    /// Return collection of all UcmdbAttributeAttribute.Name for type.properties
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<string> AllUcmdbAttributedProperties(this Type type)
    {
      var props = from p in type.GetProperties()
                  let attrs = p.GetCustomAttributes(typeof(UcmdbAttributeAttribute), false)
                  where attrs.Length != 0
                  select ((UcmdbAttributeAttribute)attrs.First()).Name;

      return props;
    }

    /// <summary>
    /// Return collection of all UcmdbAttributeAttribute.Name for object.fields
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IEnumerable<string> AllUcmdbAttributedFields(this object obj)
    {
      return AllUcmdbAttributedFields(obj.GetType());
    }

    /// <summary>
    /// Return collection of all UcmdbAttributeAttribute.Name for type.fields
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<string> AllUcmdbAttributedFields(this Type type)
    {
      var fields = from p in type.GetFields()
                   let attrs = p.GetCustomAttributes(typeof (UcmdbAttributeAttribute), false)
                   where attrs.Length != 0
                   select ((UcmdbAttributeAttribute) attrs.First()).Name;

      return fields;
    }

    /// <summary>
    /// Return collection of object properties and fields with their values
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<string,string>> UcmdbAttributedToEnumerable(this object obj)
    {
      var props = from p in obj.GetType().GetProperties()
                  let attrs = p.GetCustomAttributes(typeof(UcmdbAttributeAttribute), false)
                  let pval = p.GetValue(obj, null)
                  where attrs.Length != 0 && pval != null
                  select new KeyValuePair<string, string>(((UcmdbAttributeAttribute)attrs.First()).Name, pval.ToString());

      var fields = from f in obj.GetType().GetFields()
                   let attrs = f.GetCustomAttributes(typeof (UcmdbAttributeAttribute), false)
                   let fval = f.GetValue(obj)
                   where attrs.Length != 0 && fval != null
                   select new KeyValuePair<string, string>(((UcmdbAttributeAttribute) attrs.First()).Name, fval.ToString());

      return props.Union(fields);
    }
  }
}
