using System;
using System.Collections.Generic;
using System.Linq;

namespace ucmdb
{
  public static class UcmdbEntitiesExtensions
  {
    public static IEnumerable<string> AllUcmdbAttributedProperties(this object obj)
    {
      return AllUcmdbAttributedProperties(obj.GetType());
    }

    public static IEnumerable<string> AllUcmdbAttributedProperties(this Type type)
    {
      var props = from p in type.GetProperties()
                  let attrs = p.GetCustomAttributes(typeof(UcmdbAttributeAttribute), false)
                  where attrs.Length != 0
                  select ((UcmdbAttributeAttribute)attrs.First()).Name;

      return props;
    }

    public static IEnumerable<string> AllUcmdbAttributedFields(this object obj)
    {
      return AllUcmdbAttributedFields(obj.GetType());
    }

    public static IEnumerable<string> AllUcmdbAttributedFields(this Type type)
    {
      var fields = from p in type.GetFields()
                   let attrs = p.GetCustomAttributes(typeof (UcmdbAttributeAttribute), false)
                   where attrs.Length != 0
                   select ((UcmdbAttributeAttribute) attrs.First()).Name;

      return fields;
    }

    public static IEnumerable<KeyValuePair<string,string>> UcmdbAttributedToEnumerable(this object obj)
    {
      var props = from p in obj.GetType().GetProperties()
                  let attrs = p.GetCustomAttributes(typeof(UcmdbAttributeAttribute), false)
                  where attrs.Length != 0
                  select new KeyValuePair<string,string>(((UcmdbAttributeAttribute)attrs.First()).Name, p.GetValue(obj,null).ToString());

      return props;
    }
  }
}
