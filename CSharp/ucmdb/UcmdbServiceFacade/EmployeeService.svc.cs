using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;

namespace UcmdbServiceFacade
{

  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, Namespace = "http://localhost")]
  public class EmployeeService : IEmployeeService
  {
    private readonly IEmployeeService _bridge = new EmployeeRetriever();

    public void ConnectToUcmdbServer(Uri ucmdbUri, NetworkCredential credentials, string appContextName)
    {
      _bridge.ConnectToUcmdbServer(ucmdbUri, credentials, appContextName);
    }

    public void FindUpdatedSince(DateTime date, bool nonBlockedOnly)
    {
      _bridge.FindUpdatedSince(date, nonBlockedOnly);
    }

    public void SetChunkSize(int size)
    {
      _bridge.SetChunkSize(size);
    }

    public IEnumerable<Employee> GetNextChunk()
    {
      return _bridge.GetNextChunk();
    }
  }
}
