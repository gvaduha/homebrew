using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UcmdbFacade;
using UcmdbFacade.UcmdbService;

namespace UcmdbServiceFacade
{
  class EmployeeRetriever : IEmployeeService
  {
    private UcmdbDataRetriever _udr;
    private readonly UcmdbEntitiesBuilder _ueb = new UcmdbEntitiesBuilder().AddTemplateClass(typeof(Employee));
    private IEnumerator<IDictionary<string, object>> _retEnumerator;
    private int _retChunkSize = int.MaxValue;
    private const string EmployeeClassName = "cc_employee";

    public void ConnectToUcmdbServer(Uri ucmdbUri, NetworkCredential credentials, string appContextName = null)
    {
      _udr = new UcmdbDataRetriever(ucmdbUri, credentials, appContextName: appContextName);
    }

    public void FindUpdatedSince(DateTime date, bool nonBlockedOnly = true)
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
                                                                     value = !nonBlockedOnly,
                                                                     valueSpecified = true
                                                                   }
                                                             }
                                                         }
          },
          dateConditions = new DateConditions
          {
            dateCondition = new[]
                                                   {
                                                     new DateCondition
                                                       {
                                                         dateOperator = DateConditionDateOperator.Greater,
                                                         condition = new DateProp
                                                                       {
                                                                         name = "last_modified_time",
                                                                         value = date,
                                                                         valueSpecified = true
                                                                       }
                                                       }
                                                   }
          }
        };

      _retEnumerator = _udr.GetFilteredCiByType(EmployeeClassName, new HashSet<string>(props), cond).GetEnumerator();
    }

    public IEnumerable<Employee> GetNextChunk()
    {
      int retCount = 0;
      var ret = new List<Employee>();

      while (_retEnumerator.MoveNext())
      {
        ret.Add((Employee)_ueb.Build(EmployeeClassName, _retEnumerator.Current));

        if (++retCount >= _retChunkSize) break;
      }

      return ret;
    }

    public void SetChunkSize(int size)
    {
      _retChunkSize = size;
    }
  }
}