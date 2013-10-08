using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using ucmdb.UcmdbService;

namespace ucmdb
{
  class Program
  {
    static public void TestA(UcmdbDataRetriever udr)
    {
      var props = new HashSet<string>
                    {
                      "ca_outstaff", "ca_login_name", "ca_location", "ca_last_name", "ca_job_description",
                      "ca_middle_name", "ca_date_of_hire", "ca_email", "ca_employee_id", "ca_job_status"
                    };

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
        var collection = udr.GetFilteredCiByType("cc_employee", props, cond);

        foreach (var x in collection)
        {
          foreach (var y in x) Console.WriteLine(y.Key + "=" + y.Value);
          Console.WriteLine("-----------------------------------");
          Console.ReadLine();
        }
      }
      catch (UcmdbDataRetrieverException e)
      {
        Console.WriteLine(e.ToString());
        throw;
      }
    }


    static void Main(string[] args)
    {
      string ucmdbUri = String.Format("http://{0}:8080/axis2/services/UcmdbService",args[0]);

      //var x = new UcmdbDataRetriever(new Uri(ucmdbUri), new NetworkCredential("sysadmin", "sysadmin"), null);
      var x = new UcmdbDataRetriever(new Uri(ucmdbUri), new NetworkCredential("guest", "guest123"), null);

      TestA(x);
    }
  }

  /*********************************************************************************************************/
  /*********************************************************************************************************/
  /**************************************SOME PROBABLE USABLE CODEs*****************************************/
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
