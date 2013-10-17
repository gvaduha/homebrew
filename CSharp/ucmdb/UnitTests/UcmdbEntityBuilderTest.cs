using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ucmdb
{
  [UcmdbCiType("test_type")]
  class TestType
  {
    [UcmdbAttribute("str")]
    public string Str { get; set; }
    [UcmdbAttribute("date")]
    public DateTime? Date { get; set; }
    [UcmdbAttribute("int")]
    public int? Int;
    [UcmdbAttribute("bool")]
    public bool? Bool;
  }

  [TestFixture]
  public class UcmdbEntityBuilderTest
  {
    [Test]
    [ExpectedException(typeof(UcmdbFacadeException))]
    public void BuilderInitWrongBuilderType()
    {
      var eb = new UcmdbEntitiesBuilder();
      new[] { typeof(TestType), typeof(Int32) }.Select(eb.AddTemplateClass).ToList();
    }

    [Test]
    [ExpectedException(typeof(UcmdbFacadeException))]
    public void CreateUnknownType()
    {
      var eb = new UcmdbEntitiesBuilder();
      eb.AddTemplateClass(typeof (TestType));

      eb.Create("cc_XXX");
    }

    [Test]
    public void BuildObject()
    {
      var eb = new UcmdbEntitiesBuilder();
      eb.AddTemplateClass(typeof(TestType));

      //Correct
      var o = eb.Build("test_type", new Dictionary<string, string> { { "str", "test" }, { "date", "10/10/10" }, { "int", "5" }, { "bool", "True" } });
      Assert.AreEqual(o.GetType(), typeof(TestType));
      AssertObjects.PropertyValuesAreEquals(o, new TestType { Str = "test", Date = new DateTime(2010, 10, 10), Int = 5, Bool = true });
      //o = eb.Build("test_type", new Dictionary<string, string> { { "str", null }, { "date", "10/10/2010" }, { "int", "-5" }, { "bool", "1" } });
      //AssertObjects.PropertyValuesAreEquals(o, new TestType { Date = new DateTime(2010, 10, 10), Int = -5, Bool = true });

      //Redudant elements
      o = eb.Build("test_type", new Dictionary<string, string> { { "x", "x" }, { "str", "test" }, { "date", "10/10/10" }, { "int", "5" }, { "bool", "True" } });
      AssertObjects.PropertyValuesAreEquals(o, new TestType { Str = "test", Date = new DateTime(2010, 10, 10), Int = 5, Bool = true });

      //Sparce elements
      o = eb.Build("test_type", new Dictionary<string, string> { { "int", "5" }, { "bool", "True" } });
      AssertObjects.PropertyValuesAreEquals(o, new TestType { Int = 5, Bool = true });
      o = eb.Build("test_type", new Dictionary<string, string> { { "str", "test" } });
      AssertObjects.PropertyValuesAreEquals(o, new TestType {Str = "test"});

      //Null elements
      o = eb.Build("test_type", new Dictionary<string, string> { { "str", null }, { "bool", null } });
      AssertObjects.PropertyValuesAreEquals(o, new TestType { Str = null, Bool = null });
    }

  }

  public static class AssertObjects
  {

    public static void PropertyValuesAreEquals(object actual, object expected)
    {
      PropertyInfo[] properties = expected.GetType().GetProperties();
      foreach (PropertyInfo property in properties)
      {
        object expectedValue = property.GetValue(expected, null);
        object actualValue = property.GetValue(actual, null);

        if (actualValue is IList)
          AssertListsAreEquals(property, (IList)actualValue, (IList)expectedValue);
        else if (!Equals(expectedValue, actualValue))
          Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}", property.DeclaringType.Name, property.Name, expectedValue, actualValue);
      }
    }

    private static void AssertListsAreEquals(PropertyInfo property, IList actualList, IList expectedList)
    {
      if (actualList.Count != expectedList.Count)
        Assert.Fail("Property {0}.{1} does not match. Expected IList containing {2} elements but was IList containing {3} elements", property.PropertyType.Name, property.Name, expectedList.Count, actualList.Count);

      for (int i = 0; i < actualList.Count; i++)
        if (!Equals(actualList[i], expectedList[i]))
          Assert.Fail("Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
    }
  }
}
