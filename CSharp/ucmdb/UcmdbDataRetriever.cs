﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ucmdb.UcmdbService;

namespace ucmdb
{
  public class UcmdbDataRetriever
  {
    private readonly UcmdbService.UcmdbService _svc = new UcmdbService.UcmdbService();
    private readonly CmdbContext _ctx;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ucmdbUri">URI of uCMDB service to connect</param>
    /// <param name="credentials">Basic network credentials</param>
    /// <param name="appContextName">Name to be used in CMDB context, by default will be used new GUID</param>
    public UcmdbDataRetriever(Uri ucmdbUri, NetworkCredential credentials, string appContextName = null)
    {
      var credentialCache = new CredentialCache { { ucmdbUri, "Basic", credentials } };
      _svc.Credentials = credentialCache;
      _ctx = new CmdbContext { callerApplication = appContextName ?? Guid.NewGuid().ToString() };
    }

    /// <summary>
    /// Request uCMDB to return all records of given CI type with filter by conditions.
    /// NOTE: that now is only one operator for condition used is OR.
    /// NOTE: by defauld uCMDB stores chunks for 10 minutes while results of this function is lazy collection
    /// Only properties from <paramref name="properties"/> list returned.
    /// </summary>
    /// <param name="ciType">Type of CI to request</param>
    /// <param name="properties">CI's properties to request</param>
    /// <param name="conditions">Conditions to filter CIs</param>
    /// <returns>List of CI's property-value dictionaries. Records are sparsed.</returns>
    public IEnumerable<IEnumerable<KeyValuePair<string, string>>> GetFilteredCiByType(string ciType, ISet<string> properties, Conditions conditions)
    {
      var request = new getFilteredCIsByType
      {
        cmdbContext = _ctx,
        type = ciType,
        conditions = conditions,
        conditionsLogicalOperator = new getFilteredCIsByTypeConditionsLogicalOperator(),
        properties = new CustomProperties { propertiesList = properties.ToArray() }
      };

      try
      {
        var response = _svc.getFilteredCIsByType(request);

        return ProcessResponse(response, properties);
      }
      catch (Exception e)
      {
        throw new UcmdbDataRetrieverException("GetFilteredCiByType", e);
      }
    }

    /// <summary>
    /// Process uCMDB ucmdbResponse into list of CI's property-value dictionaries.
    /// </summary>
    /// <param name="ucmdbResponse">Response from one of uCMDB request function</param>
    /// <param name="propNames">CI's properties to return</param>
    /// <returns>List of CI's property-value dictionaries. Records are sparsed.</returns>
    private IEnumerable<IEnumerable<KeyValuePair<string, string>>> ProcessResponse(dynamic ucmdbResponse, ISet<string> propNames)
    {
      if (ucmdbResponse.chunkInfo.numberOfChunks == 0) // Number of chunks is 0 when all results fitted in ucmdbResponse
      {
        foreach (CI ci in ucmdbResponse.CIs)
          yield return ProcessCi(ci, propNames);
      }
      else // Read all chunks and process them
      {
        var chunkRequest = new ChunkRequest { chunkInfo = ucmdbResponse.chunkInfo };

        for (int chunkNumber = 1; chunkNumber <= ucmdbResponse.chunkInfo.numberOfChunks; chunkNumber++)
        {
          chunkRequest.chunkNumber = chunkNumber;
          var ptmc = new pullTopologyMapChunks { cmdbContext = _ctx, ChunkRequest = chunkRequest };

          var ptmcResponse = _svc.pullTopologyMapChunks(ptmc);

          foreach (var ci in ptmcResponse.topologyMap.CINodes.SelectMany(ciNode => ciNode.CIs))
          {
            yield return ProcessCi(ci, propNames);
          }
        }

        _svc.releaseChunks(new releaseChunks { chunksKey = ucmdbResponse.chunkInfo.chunksKey, cmdbContext = _ctx });
      }
    }

    /// <summary>
    /// Retrieves name value pairs from given ci
    /// </summary>
    /// <param name="ci">uCMDB CI record</param>
    /// <param name="propNames">Name of properties to return</param>
    /// <returns>Dictionary of property name - values</returns>
    public virtual IEnumerable<KeyValuePair<string, string>> ProcessCi(CI ci, ISet<string> propNames)
    {
      var result = new List<KeyValuePair<string, string>>
                     {
                       //Always add id
                       new KeyValuePair<string, string>("id", ci.ID.Value)
                     };

      //result.AddRange(from strProp in ci.props.strProps
      //                where propNames.Contains(strProp.name)
      //                select new KeyValuePair<string, string>(strProp.name, strProp.value));

      Func<IEnumerable<dynamic>, IEnumerable<KeyValuePair<string, string>>> fun =
        list => from prop in list
                where propNames.Contains((string)prop.name)
                select new KeyValuePair<string, string>(prop.name, prop.value.ToString());

      if (ci.props.booleanProps != null) result.AddRange(fun(ci.props.booleanProps));
      if (ci.props.bytesProps != null) result.AddRange(fun(ci.props.bytesProps));
      if (ci.props.dateProps != null) result.AddRange(fun(ci.props.dateProps));
      if (ci.props.doubleProps != null) result.AddRange(fun(ci.props.doubleProps));
      if (ci.props.floatProps != null) result.AddRange(fun(ci.props.floatProps));
      if (ci.props.intListProps != null) result.AddRange(fun(ci.props.intListProps));
      if (ci.props.intProps != null) result.AddRange(fun(ci.props.intProps));
      if (ci.props.longProps != null) result.AddRange(fun(ci.props.longProps));
      if (ci.props.strListProps != null) result.AddRange(fun(ci.props.strListProps));
      if (ci.props.strProps != null) result.AddRange(fun(ci.props.strProps));
      if (ci.props.xmlProps != null) result.AddRange(fun(ci.props.xmlProps));

      return result;
    }
  }
}