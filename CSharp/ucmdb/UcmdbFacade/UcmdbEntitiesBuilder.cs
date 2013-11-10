using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UcmdbFacade
{
  /// <summary>
  /// Builds class tagged with UcmdbAttributes instances
  /// </summary>
  public class UcmdbEntitiesBuilder
  {
    private readonly Dictionary<string, Type> _templateClasses = new Dictionary<string, Type>();

    /// <summary>
    /// Adds class tagged as UcmdbCiType to builder collection
    /// </summary>
    /// <param name="classType"></param>
    /// <returns></returns>
    public UcmdbEntitiesBuilder AddTemplateClass(Type classType)
    {
      var attr = classType.GetCustomAttributes(typeof(UcmdbCiTypeAttribute), false);

      if (attr.Length == 0)
        throw new UcmdbFacadeException(String.Format("Class {0} doesn't tagged with UcmdbCiTypeAttribute", classType));

      _templateClasses.Add(((UcmdbCiTypeAttribute)attr.First()).Name, classType);

      return this;
    }

    /// <summary>
    /// Creates object of typeName
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public object Create(string typeName)
    {
      var classType = _templateClasses.FirstOrDefault(x => x.Key == typeName);

      if (classType.Value == null)
        throw new UcmdbFacadeException("Can't create object of type " + typeName);

      return Activator.CreateInstance(classType.Value);
    }

    /// <summary>
    /// Creates object of typeName and initialize object properties and fields marked as UcmdbAttributeAttribute
    /// with attrValues corresponsing values
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="attrValues"></param>
    /// <returns></returns>
    public object Build(string typeName, IDictionary<string, object> attrValues)
    {
      var obj = Create(typeName);

      try
      {
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
            p.Value.SetValue(obj, 
              Convert.ChangeType(attrValue.Value,Nullable.GetUnderlyingType(p.Value.PropertyType) ?? p.Value.PropertyType)
              , null);
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
      }
      catch (Exception e)
      {
        throw new UcmdbFacadeException(String.Format("Building object of type {0} failed", typeName), e);
      }

      return obj;
    }
  }
}
