using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UcmdbFacade;
using UcmdbFacade.UcmdbService;

namespace UcmdbServiceFacade
{
  public class EntityRetriever<T> : IEntityRetriever<T>
  {
    private UcmdbDataRetriever _udr;
    private readonly UcmdbEntitiesBuilder _ueb = new UcmdbEntitiesBuilder().AddTemplateClass(typeof(T));
    private IEnumerator<IDictionary<string, object>> _retEnumerator;
    private int _retChunkSize = int.MaxValue;
    private readonly string _entityClassName;

    public EntityRetriever()
    {
      _entityClassName = typeof(T).GetUcmdbTypeAttribute();
    }

    public void ConnectToUcmdbServer(Uri ucmdbUri, NetworkCredential credentials, string appContextName = null)
    {
      _udr = new UcmdbDataRetriever(ucmdbUri, credentials, appContextName: appContextName);
    }

    public void FindUpdatedSince(DateTime date, bool nonBlockedOnly = true)
    {
      var props = typeof(T).AllUcmdbAttributedFields().Union(typeof(T).AllUcmdbAttributedProperties());

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

      _retEnumerator = _udr.GetFilteredCiByType(_entityClassName, new HashSet<string>(props), cond).GetEnumerator();
    }

    public IEnumerable<T> GetNextChunk()
    {
      int retCount = 0;
      var ret = new List<T>();

      while (_retEnumerator.MoveNext())
      {
        ret.Add( (T)_ueb.Build(_entityClassName, _retEnumerator.Current) );

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