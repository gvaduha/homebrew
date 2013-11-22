using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace UcmdbFacade.UnitTests
{
  [TestFixture]
  class UcmdbEntitiesExtensionsTest
  {
    [Test]
    public void ClassTagNameTest()
    {
      Assert.True("test_type" == typeof(TestType).GetUcmdbTypeAttribute());
    }

    [Test]
    public void FieldsTagNamesTest()
    {
      var o = new TestType();
      CollectionAssert.AreEqual(o.AllUcmdbAttributedFields(), new[] { "int", "bool" });
    }

    [Test]
    public void PropertiesTagNamesTest()
    {
      var o = new TestType();
      CollectionAssert.AreEqual(o.AllUcmdbAttributedProperties().ToList(), new[] { "str", "date" });
    }

    [Test]
    public void EntityToEnumerableTest()
    {
      var o = new TestType {Str = "test", Date = new DateTime(2010,10,10), Int = 5, Bool = true};

      CollectionAssert.AreEqual(o.UcmdbAttributedToEnumerable().ToList(),
                                new []
                                  {
                                    new KeyValuePair<string, string>("str", "test"),
                                    new KeyValuePair<string, string>("date", new DateTime(2010, 10, 10).ToString()),
                                    new KeyValuePair<string, string>("int", "5"),
                                    new KeyValuePair<string, string>("bool", "True")
                                  });
    }
  }
}
