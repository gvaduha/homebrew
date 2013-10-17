using System;
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
      _svc.Url = ucmdbUri.ToString();
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
    public IEnumerable<IDictionary<string, string>> GetFilteredCiByType(string ciType, ISet<string> properties, Conditions conditions)
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
        throw new UcmdbFacadeException("GetFilteredCiByType", e);
      }
    }

    /// <summary>
    /// Process uCMDB ucmdbResponse into list of CI's property-value dictionaries.
    /// </summary>
    /// <param name="ucmdbResponse">Response from one of uCMDB request function</param>
    /// <param name="propNames">CI's properties to return</param>
    /// <returns>List of CI's property-value dictionaries. Records are sparsed.</returns>
    private IEnumerable<IDictionary<string, string>> ProcessResponse(dynamic ucmdbResponse, ISet<string> propNames)
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
    public virtual IDictionary<string, string> ProcessCi(CI ci, ISet<string> propNames)
    {
      var result = new Dictionary<string,string>
                     {
                       //Always add id
                       {"id", ci.ID.Value}
                     };

      //addToResult(from strProp in ci.props.strProps
      //                where propNames.Contains(strProp.name)
      //                select new KeyValuePair<string, string>(strProp.name, strProp.value));

      Func<IEnumerable<dynamic>, IEnumerable<KeyValuePair<string, string>>> getProps =
        list => from prop in list
                where propNames.Contains((string)prop.name)
                select new KeyValuePair<string, string>(prop.name, prop.value.ToString());

      Action<IEnumerable<KeyValuePair<string, string>>> addToResult =
        list =>
          {
            foreach (var x in list.Where(x => !result.ContainsKey(x.Key)))
              result.Add(x.Key, x.Value);
          };

      if (ci.props.booleanProps != null) addToResult(getProps(ci.props.booleanProps));
      if (ci.props.bytesProps != null) addToResult(getProps(ci.props.bytesProps));
      if (ci.props.dateProps != null) addToResult(getProps(ci.props.dateProps));
      if (ci.props.doubleProps != null) addToResult(getProps(ci.props.doubleProps));
      if (ci.props.floatProps != null) addToResult(getProps(ci.props.floatProps));
      if (ci.props.intListProps != null) addToResult(getProps(ci.props.intListProps));
      if (ci.props.intProps != null) addToResult(getProps(ci.props.intProps));
      if (ci.props.longProps != null) addToResult(getProps(ci.props.longProps));
      if (ci.props.strListProps != null) addToResult(getProps(ci.props.strListProps));
      if (ci.props.strProps != null) addToResult(getProps(ci.props.strProps));
      if (ci.props.xmlProps != null) addToResult(getProps(ci.props.xmlProps));

      return result;
    }


    public void x()
    {
      var request = new executeTopologyQueryByNameWithParameters { cmdbContext = _ctx, queryName = "New_View_2" };

      //set parameters
      var hostParametrizedNode = new ParameterizedNode { nodeLabel = "Host" };
      var parameters = new CIProperties();
      var strProps = new StrProp[1];
      var strProp = new StrProp { name = "host_os", value = "%2000%" };
      strProps[0] = strProp;
      parameters.strProps = strProps;
      hostParametrizedNode.parameters = parameters;

      var diskParametrizedNode = new ParameterizedNode { nodeLabel = "Disk" };
      var parameters1 = new CIProperties();
      var intProps = new IntProp[1];
      var intProp = new IntProp { name = "disk_failures", value = "30" };
      intProps[0] = intProp;
      parameters1.intProps = intProps;
      diskParametrizedNode.parameters = parameters1;

      request.parameterizedNodes = new[] { hostParametrizedNode, diskParametrizedNode };

      //properties to retrieve (TypedProperties[])
      //request.queryTypedProperties
      #region "test"
      var tp = new TypedProperties();
      tp.properties.predefinedTypedProperties.simpleTypedPredefinedProperties = new[]
                                                                                  {
                                                                                    new SimpleTypedPredefinedProperty { name = SimpleTypedPredefinedPropertyName.CONCRETE}
                                                                                  };

      request.queryTypedProperties = new[]
                                       {
                                         new TypedProperties
                                           {
                                             properties = new CustomTypedProperties
                                                            {
                                                              predefinedTypedProperties =
                                                                new PredefinedTypedProperties
                                                                  {
                                                                    simpleTypedPredefinedProperties = new[]
                                                                                                        {
                                                                                                          new SimpleTypedPredefinedProperty
                                                                                                            {
                                                                                                              name = SimpleTypedPredefinedPropertyName.CONCRETE
                                                                                                            }
                                                                                                        }
                                                                  }
                                                            }
                                           }
                                       };
      #endregion

      try
      {
        var response = _svc.executeTopologyQueryByNameWithParameters(request);
        var topologyMap = response.topologyMap;


      }
      catch (Exception e)
      {
      }
    }
 }

}
