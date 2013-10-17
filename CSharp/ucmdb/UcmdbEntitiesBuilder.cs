using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ucmdb
{
  public class UcmdbEntitiesBuilder
  {
    private readonly Dictionary<string, Type> _templateClasses = new Dictionary<string, Type>();

    public UcmdbEntitiesBuilder AddTemplateClass(Type classType)
    {
      var attr = classType.GetCustomAttributes(typeof(UcmdbCiTypeAttribute), false);

      if (attr.Length == 0)
        throw new UcmdbFacadeException(String.Format("Class {0} doesn't tagged with UcmdbCiTypeAttribute", classType));

      _templateClasses.Add(((UcmdbCiTypeAttribute)attr.First()).Name, classType);

      return this;
    }

    public object Create(string typeName)
    {
      var classType = _templateClasses.FirstOrDefault(x => x.Key == typeName);

      if (classType.Value == null)
        throw new UcmdbFacadeException("Can't create object of type " + typeName);

      return Activator.CreateInstance(classType.Value);
    }

    public object Build(string typeName, IDictionary<string, string> attrValues)
    {
      var obj = Create(typeName);

      //Fill properties for which attribute is defined
      var props = from p in obj.GetType().GetProperties()
                  let attrs = p.GetCustomAttributes(typeof(UcmdbAttributeAttribute), false)
                  where attrs.Length != 0
                  select
                    new KeyValuePair<UcmdbAttributeAttribute, PropertyInfo>((UcmdbAttributeAttribute)attrs.First(), p);

      foreach (var p in props)
      {
        var p1 = p;
        var attrValue = attrValues.FirstOrDefault(x => x.Key == p1.Key.Name);

        if (attrValue.Value != null)
          p.Value.SetValue(obj, Convert.ChangeType(attrValue.Value,
                        Nullable.GetUnderlyingType(p.Value.PropertyType) ?? p.Value.PropertyType), null);
      }

      //Fill fields for which attribute is defined
      var fields = from f in obj.GetType().GetFields()
                   let attrs = f.GetCustomAttributes(typeof(UcmdbAttributeAttribute), false)
                   where attrs.Length != 0
                   select
                     new KeyValuePair<UcmdbAttributeAttribute, FieldInfo>((UcmdbAttributeAttribute)attrs.First(), f);

      foreach (var f in fields)
      {
        var f1 = f;
        var attrValue = attrValues.FirstOrDefault(x => x.Key == f1.Key.Name);

        if (attrValue.Value != null)
          f.Value.SetValue(obj, Convert.ChangeType(attrValue.Value,
            Nullable.GetUnderlyingType(f.Value.FieldType) ?? f.Value.FieldType));
      }

      return obj;
    }
  }
}
