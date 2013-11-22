using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace UcmdbServiceFacade
{
  class FakeEntityRetriever<T> : IEntityRetriever<T> where T : new()
  {
    private int _retChunkSize = 10;
    private int _curId;

    /// <summary>
    /// Constructs fake return list of abstract entities, with only 'Name' field declared as sequence number
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> GetNextChunk()
    {
      var ret = Enumerable.Range(_curId, _retChunkSize).ToList().Select(CreateObject);

      _curId += _retChunkSize;

      return ret;
    }

    static T CreateObject(int num)
    {
      var t = new T();

      //HERE YOU SHOULD INITIALIZE ONE OR MORE OF PROPERTY OR FIELD
      //var f = t.GetType().GetFields().FirstOrDefault();
      //if (f != null)
      //  f.SetValue(t, num);

      return t;
    }

    public void ConnectToUcmdbServer(Uri ucmdbUri, NetworkCredential credentials, string appContextName)
    {
    }

    public void FindUpdatedSince(DateTime date, bool nonBlockedOnly)
    {
    }

    public void SetChunkSize(int size)
    {
      _retChunkSize = size;
    }
  }
}