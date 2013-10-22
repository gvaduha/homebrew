using System;
using System.Collections.Generic;
using System.Data;
using ucmdb.UnitTests;

namespace ucmdb
{
  class Program
  {

    static void Main(string[] args)
    {
      var tst = new UcmdbDataRetrieverTest();
      tst.Initialize(args[0]);
      //tst.TestEmployeeLoad("cc_employee");
      tst.TestTopologyRequest();


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
