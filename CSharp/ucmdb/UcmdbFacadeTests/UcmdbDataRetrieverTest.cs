using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using UcmdbFacade.UcmdbService;

namespace UcmdbFacade.UnitTests
{
  [TestFixture]
  public class UcmdbDataRetrieverTest
  {
    private UcmdbDataRetriever _udr;
    private UcmdbEntitiesBuilder _ueb;

    //This method has invalid signature and WILL BE IGNORED! SetUp shouldn't have parameters
    [TestFixtureSetUp]
    public void Initialize(string serverName)
    {
      string ucmdbUri = String.Format("http://{0}:8080/axis2/services/UcmdbService", serverName);

      //var _udr = new UcmdbDataRetriever(new Uri(ucmdbUri), new NetworkCredential("sysadmin", "sysadmin"), null);
      _udr = new UcmdbDataRetriever(new Uri(ucmdbUri), new NetworkCredential("guest", "guest123"));

      _ueb = new UcmdbEntitiesBuilder();
      new[] { typeof(OrgUnit), typeof(Employee) }.Select(_ueb.AddTemplateClass).ToList();
    }

    [Test]
    public void TestEmployeeLoad(string className)
    {
      var props = typeof(Employee).AllUcmdbAttributedFields().Union(typeof(Employee).AllUcmdbAttributedProperties());

      var cond =
        new Conditions
          {
            booleanConditions = new BooleanConditions
                                  {
                                    booleanCondition = new[]
                                                         {
                                                           new BooleanCondition
                                                             {
                                                               booleanOperator = BooleanConditionBooleanOperator.Equal,
                                                               condition =
                                                                 new BooleanProp
                                                                   {
                                                                     name = "ca_blocked",
                                                                     value = false,
                                                                     valueSpecified = true
                                                                   }
                                                             }
                                                         }
                                  }
          };

      try
      {
        var collection = _udr.GetFilteredCiByType(className, new HashSet<string>(props), cond);

        foreach (var ucmdbEntity in collection)
        {
          var o = _ueb.Build(className, ucmdbEntity);

          foreach (var x in o.UcmdbAttributedToEnumerable())
            Console.WriteLine(x.Key + "=" + x.Value);
          Console.WriteLine("-----------------------------------");
          Console.ReadLine();
        }
      }
      catch (UcmdbFacadeException e)
      {
        Console.WriteLine(e.ToString());
        throw;
      }
    }

    [Test]
    public void TestTopologyRequest()
    {
      //set parameters
      //var dateParam = new ParameterizedNode
      //                  {
      //                    nodeLabel = "Employee",
      //                    parameters = new CIProperties
      //                                   {
      //                                     //dateProps = new[] { new DateProp { name = "param_last_modified_time", value = DateTime.Now } }
      //                                     //dateProps = new[] { new DateProp { name = "LastModifiedTime", value = DateTime.Now } }
      //                                     dateProps = new[] { new DateProp { name = "param_last_modified_time", value = DateTime.Now, valueSpecified = true } }
      //                                   }
      //                  };

      var dateParam = new ParameterizedNode
      {
        nodeLabel = "Employee",
        parameters = new CIProperties
        {
          //dateProps = new[] { new DateProp { name = "param_last_modified_time", value = DateTime.Now } }
          //dateProps = new[] { new DateProp { name = "LastModifiedTime", value = DateTime.Now } }
          strProps = new[] { new StrProp { name = "abc", value = "aaa" } },
          //dateProps = new[] { new DateProp { name = "param_last_modified_time", value = DateTime.Now, valueSpecified = true } }
        }
      };

      var topologyMap = _udr.ExecuteTopologyQueryByNameWithParameters("MyView1", new[] {dateParam});

      //foreach (var ci in topologyMap.CINodes[1].CIs)
      //{
      //  foreach (var x in _udr.ProcessCi(ci, null))
      //  {
      //    Console.WriteLine(String.Format("{0}={1}", x.Key, x.Value));
      //  }
      //  Console.WriteLine("---------------------------------");
      //  Console.ReadLine();
      //}

      foreach (var rel in topologyMap.relationNodes[0].relations)
      {
        foreach (var x in _udr.ProcessRelation(rel, new HashSet<string>()))
        {
          Console.WriteLine(String.Format("{0}={1}", x.Key, x.Value));
        }
        Console.WriteLine("---------------------------------");
        Console.ReadLine();
      }


    }
  }
}
