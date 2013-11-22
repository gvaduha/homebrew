using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;

namespace UcmdbServiceFacade
{
  /// <summary>
  /// Interface to interact with uCMDB server. Protocol of interaction should be as follow:
  /// 
  /// - ConnectToUcmdbServer
  /// - SetChunkSize (optional)
  /// - FindUpdatedSince (or similar)
  /// - while( GetNextChunk != Enumerable.Empty )
  /// 
  /// </summary>
  [ServiceContract(SessionMode = SessionMode.Required, Namespace = "http://localhost")]
  public interface IEntityRetriever<out T>
  {
    /// <summary>
    /// Establish connection to uCMDB server
    /// </summary>
    /// <param name="ucmdbUri">URI of uCMDB service instance in form http://server:port/path_to_ucmdservice/UcmdbService</param>
    /// <param name="credentials">NetworkCredential ISN'T SERIALIZABLE FOR SECURITY REASONS. NEED TO BE CHANGED</param>
    /// <param name="appContextName">Contex name for uCMDB connection. Now uCMDB Facade automatically create GUID name if don't specified</param>
    [OperationContract]
    void ConnectToUcmdbServer(Uri ucmdbUri, NetworkCredential credentials, string appContextName = null);

    /// <summary>
    /// Get all Employees updated in uCMDB since given date
    /// </summary>
    /// <param name="date">Date and time of the last update. 'Greater' operator would be used for this value</param>
    /// <param name="nonBlockedOnly">Find only non blocked entities (ca_blocked != true).
    /// NOTE: this isn't standard property hence 'ca_', so this part of the class is hardcoded and should be rewriten to
    /// eliminate this in case of standartizing to entities without this custom attribute</param>
    [OperationContract]
    void FindUpdatedSince(DateTime date, bool nonBlockedOnly = true);

    /// <summary>
    /// Set size of chunks returned by GetNextChunk
    /// </summary>
    /// <param name="size">MaxValue is set by default and means return all data in one chunk</param>
    [OperationContract]
    void SetChunkSize(int size);

    /// <summary>
    /// Return result set chunk
    /// </summary>
    /// <returns>Empty if there is no more data, otherwise Employee list according to SetChunkSize value</returns>
    [OperationContract]
    IEnumerable<T> GetNextChunk();
  }
}
