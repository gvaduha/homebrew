using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using ucmdb.UcmdbService;

namespace ucmdb
{
  class Program
  {
    static public void TestA(UcmdbDataRetriever udr, UcmdbEntitiesBuilder ueb)
    {
      var props = typeof (Employee).AllUcmdbAttributedFields().Union(typeof (Employee).AllUcmdbAttributedProperties());

      var cond =
        new Conditions
          {
            booleanConditions = new BooleanConditions
                                  {
                                    booleanCondition = new[]
                                                         {
                                                           new BooleanCondition
                                                             {
                                                               booleanOperator =BooleanConditionBooleanOperator.Equal,
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
        var collection = udr.GetFilteredCiByType("cc_employee", new HashSet<string>(props), cond);

        //foreach (var x in collection)
        //{
        //  foreach (var y in x) Console.WriteLine(y.Key + "=" + y.Value);
        //  Console.WriteLine("-----------------------------------");
        //  Console.ReadLine();
        //}

        foreach (var x in collection)
        {
          //var o = ueb.Build("cc_employee", new Dictionary<string, string>(x));
        }
      }
      catch (UcmdbFacadeException e)
      {
        Console.WriteLine(e.ToString());
        throw;
      }
    }


    static void Main(string[] args)
    {
      string ucmdbUri = String.Format("http://{0}:8080/axis2/services/UcmdbService",args[0]);

      //var u = new UcmdbDataRetriever(new Uri(ucmdbUri), new NetworkCredential("sysadmin", "sysadmin"), null);
      var u = new UcmdbDataRetriever(new Uri(ucmdbUri), new NetworkCredential("guest", "guest123"));

      //u.x();

      var ueb = new UcmdbEntitiesBuilder();
      new[] { typeof(OrgUnit), typeof(Employee) }.Select(ueb.AddTemplateClass).ToList();

      //var o = ueb.Build("cc_organization_unit", new Dictionary<string, string> { { "id", "10" }, { "name", "me" }, { "x", "5" } });

      TestA(u, ueb);

      Console.WriteLine();
    }
  }

  /*********************************************************************************************************/
  /*********************************************************************************************************/
  /**************************************SOME PROBABLE USABLE CODE******************************************/
  /*********************************************************************************************************/
  /*********************************************************************************************************/

  class DatasetLoader<T> where T: DataTable
  {
    private readonly IDictionary<string, string> _mapping;

    public DatasetLoader(IDictionary<string, string> mapping)
    {
      _mapping = mapping;
    }

    public void Load(IEnumerable<IEnumerable<KeyValuePair<string,string>>> src, T table)
    {
      foreach (var line in src)
      {
        var row = table.NewRow();

        foreach (var pair in line)
        {
          row[_mapping[pair.Key]] = pair.Value;
        }

        table.Rows.Add(row);
      }
    }
  }

  static class UcmdbConstraintBuilder
  {
    private static readonly Dictionary<string, Func<string, string, object>> Constraints = new Dictionary<string, Func<string, string, object>> { { "==", BuildEqual } };

    private static object BuildEqual(string name, string value)
    {
      return 1;
    }

    public static object Build(string expression)
    {
      var parts = expression.Split();

      return Constraints[parts[1]](parts[0], parts[2]);
    }
  }

}
