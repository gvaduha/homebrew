using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using UcmdbFacade;
using UcmdbFacade.UcmdbService;
using UcmdbFacade.UnitTests;
using System.Net;

namespace ucmdb
{
  class XXX
  {
    private UcmdbDataRetriever _udr;
    private UcmdbEntitiesBuilder _ueb  = new UcmdbEntitiesBuilder();
    private IEnumerator<IDictionary<string, object>> _retEnumerator;
    private int _retChunkSize = 0;
    private const string EmployeeClass = "cc_employee";

    public void ConnectToUcmdbServer(Uri ucmdbUri, NetworkCredential credentials, string appContextName = null)
    {
      _udr = new UcmdbDataRetriever(ucmdbUri, credentials, appContextName);
    }

    public void XXXEmployeeLoad()
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

      _retEnumerator = _udr.GetFilteredCiByType(EmployeeClass, new HashSet<string>(props), cond).GetEnumerator();
    }

    public IEnumerable<Employee> GetNextChunk()
    {
      int retCount = 0;
      var ret = new List<Employee>(_retChunkSize);

      while(retCount++ < _retChunkSize)
      {
        ret[retCount++] = (Employee)_ueb.Build(EmployeeClass, _retEnumerator.Current);

        _retEnumerator.MoveNext();
      }

      return ret;
    }

    public void SetChunkSize(int size)
    {
      _retChunkSize = size;
    }
  }

  class FakeXXX
  {
    private int _retChunkSize = 10;
    private int _curId = 0;

    public IEnumerable<Employee> GetNextChunk()
    {
      var ret = Enumerable.Range(_curId, _retChunkSize).ToList().Select(id => new Employee{LastName = id.ToString()});

      _curId += _retChunkSize;

      return ret;
    }

    public void SetChunkSize(int size)
    {
      _retChunkSize = size;
    }
  }


  class Program
  {

    static void Main(string[] args)
    {
      var tst = new UcmdbDataRetrieverTest();
      tst.Initialize(args[0]);
      //tst.TestEmployeeLoad("cc_employee");
      tst.TestTopologyRequest();


      Console.WriteLine("End of test run");
    }
  }
}
