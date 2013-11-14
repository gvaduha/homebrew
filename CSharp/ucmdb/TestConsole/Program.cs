using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using ucmdb.UcmdbServiceFacade;

namespace ucmdb
{
  class Program
  {
    static void PrintReply(IEnumerable<Employee> chunk)
    {
      chunk.ToList().ForEach(x => Console.WriteLine(x.Name + ":" + x.Email));
      Console.WriteLine("----------------");
    }

    static void Main(string[] args)
    {
      try
      {
        string serviceUri = "http://XXX:80/WebServices.t7_rb/EmployeeService.svc";
        string ucmdbUri = "http://XXX:8080/axis2/services/UcmdbService";
        string login = "guest";
        string password = "guest123";
        string lastDate = "2013/11/11";
        int chunkSize = 16;

        Console.WriteLine("test.exe serviceUri ucmdbUri login password date(yyyy/mm/dd)");
        serviceUri = args[0];
        ucmdbUri = args[1];
        login = args[2];
        password = args[3];
        lastDate = args[4];
        Console.WriteLine(String.Format("test.exe {0} {1} {2} {3} {4}", serviceUri, ucmdbUri, login, password, lastDate));

        var svc = new EmployeeServiceClient(new WSHttpBinding(),
          new EndpointAddress(new Uri(serviceUri)
        //////                      //,EndpointIdentity.CreateSpnIdentity("HOST/dev-app01:8008")
                              ));

        //var svc = new EmployeeServiceClient();

        svc.ConnectToUcmdbServer(new Uri(ucmdbUri), new NetworkCredential(login, password), Guid.NewGuid().ToString());

        svc.SetChunkSize(chunkSize);

        svc.FindUpdatedSince(DateTime.Parse(lastDate), true);

        var chunk = svc.GetNextChunk();

        while (chunk != Enumerable.Empty<Employee>())
        {
          PrintReply(chunk);
          chunk = svc.GetNextChunk();
          Console.ReadKey();
        }

        PrintReply(new EmployeeServiceClient().GetNextChunk());

        Console.WriteLine("End of test run");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message+Environment.NewLine+e.StackTrace+Environment.NewLine+e);
        throw;
      }
    }
  }
}
