using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace UcmdbServiceFacade
{
  class FakeEmployeeRetriever : IEmployeeService
  {
    private int _retChunkSize = 10;
    private int _curId;

    /// <summary>
    /// Constructs fake return list of employees, with only 'Name' field declared as sequence number
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Employee> GetNextChunk()
    {
      var ret = Enumerable.Range(_curId, _retChunkSize).ToList().Select(id => new Employee { Name = id.ToString() });

      _curId += _retChunkSize;

      return ret;
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