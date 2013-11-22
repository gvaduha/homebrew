using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UcmdbFacade.UcmdbService;

namespace UcmdbFacade
{
  public class UcmdbDataRetriever
  {
    private readonly UcmdbService.UcmdbService _svc = new UcmdbService.UcmdbService();
    private readonly CmdbContext _ctx;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ucmdbUri">URI of uCMDB service to connect</param>
    /// <param name="credentials">Username and password to login uCMDB server packed in NetworkCredentials</param>
    /// <param name="authType">Type of authentication in credentials cache to be created for uCMDB access</param>
    /// <param name="appContextName">Name to be used in CMDB context, by default will be used new GUID</param>
    public UcmdbDataRetriever(Uri ucmdbUri, NetworkCredential credentials, string authType = "Basic", string appContextName = null)
    {
      _svc.Url = ucmdbUri.ToString();
      var credentialCache = new CredentialCache { { ucmdbUri, authType, credentials } };
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
    /// <param name="andConditioning">Use AND operator between conditions if true, OR if false</param>
    /// <returns>List of CI's property-value dictionaries. Records are sparsed.</returns>
    public IEnumerable<IDictionary<string, object>> GetFilteredCiByType(string ciType, ISet<string> properties, Conditions conditions, bool andConditioning = true)
    {
      var request = new getFilteredCIsByType
      {
        cmdbContext = _ctx,
        type = ciType,
        conditions = conditions,
        conditionsLogicalOperator = andConditioning ? getFilteredCIsByTypeConditionsLogicalOperator.AND : getFilteredCIsByTypeConditionsLogicalOperator.OR,
        properties = new CustomProperties { propertiesList = properties.ToArray() }
      };

      try
      {
        var response = _svc.getFilteredCIsByType(request);

        return ProcessResponse(response, properties);
      }
      catch (Exception e)
      {
        throw new UcmdbFacadeException(String.Format("GetFilteredCiByType ({0})", e.Message), e);
      }
    }

    /// <summary>
    /// Process uCMDB ucmdbResponse into list of CI's property-value dictionaries.
    /// </summary>
    /// <param name="ucmdbResponse">Response from one of uCMDB request function</param>
    /// <param name="propNames">CI's properties to return</param>
    /// <returns>List of CI's property-value dictionaries. Records are sparsed.</returns>
    private IEnumerable<IDictionary<string, object>> ProcessResponse(dynamic ucmdbResponse, ISet<string> propNames)
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
    /// Retrieves name value pairs from given CI
    /// </summary>
    /// <param name="ci">uCMDB CI record</param>
    /// <param name="propNames">Name of properties to return. If null ALL properties selected</param>
    /// <returns>Dictionary of property name - values</returns>
    public virtual IDictionary<string, object> ProcessCi(CI ci, ISet<string> propNames)
    {
      //Add "special" property that don't listed in collections
      var result = new Dictionary<string, object>
                     {
                       {"id", ci.ID.Value}
                     };

      AddPropsToDictionary(result, ci.props, propNames);

      return result;
    }

    /// <summary>
    /// Retrieves name value pairs from given Relation
    /// </summary>
    /// <param name="relation">uCMDB Relation record</param>
    /// <param name="propNames">Name of properties to return. If null ALL properties selected</param>
    /// <returns>Dictionary of property name - values</returns>
    public virtual IDictionary<string, object> ProcessRelation(Relation relation, ISet<string> propNames)
    {
      //Add "special" property that don't listed in collections
      var result = new Dictionary<string, object>
                     {
                       {"id", relation.ID.Value},
                       {"end1id", relation.end1ID},
                       {"end2id", relation.end2ID}
                     };

      AddPropsToDictionary(result, relation.props, propNames);

      return result;
    }

    /// <summary>
    /// Retrieves name value pairs from CIProperties collection
    /// </summary>
    /// <param name="dict">Dictionary to add data to</param>
    /// <param name="props">Collection of CIProperties to process</param>
    /// <param name="propNames">Name of properties to return. If null ALL properties selected</param>
    /// <returns>Dictionary of property name - values</returns>
    protected void AddPropsToDictionary(IDictionary<string, object> dict, CIProperties props, ISet<string> propNames)
    {
      //Function which select item properties that listed in propNames set
      Func<IEnumerable<dynamic>, IEnumerable<KeyValuePair<string, object>>> listedPropsSelector =
        list => from prop in list
                where propNames.Contains((string)prop.name)
                select new KeyValuePair<string, object>(prop.name, prop.value);

      //Function which select ALL item properties
      Func<IEnumerable<dynamic>, IEnumerable<KeyValuePair<string, object>>> fullPropsSelector =
        list => from prop in list
                select new KeyValuePair<string, object>(prop.name, prop.value);

      //Select appropriate selector function
      var getProps = propNames == null ? fullPropsSelector : listedPropsSelector;

      //Function adds property and value in result Dictionary
      Action<IEnumerable<KeyValuePair<string, object>>> addToResult =
        list =>
        {
          foreach (var x in list) // list.Where(x => !result.ContainsKey(x.Key)) ?
            dict.Add(x.Key, x.Value);
        };

      //Mess with java collections
      if (props.booleanProps != null) addToResult(getProps(props.booleanProps));
      if (props.bytesProps != null) addToResult(getProps(props.bytesProps));
      if (props.dateProps != null) addToResult(getProps(props.dateProps));
      if (props.doubleProps != null) addToResult(getProps(props.doubleProps));
      if (props.floatProps != null) addToResult(getProps(props.floatProps));
      if (props.intListProps != null) addToResult(getProps(props.intListProps));
      if (props.intProps != null) addToResult(getProps(props.intProps));
      if (props.longProps != null) addToResult(getProps(props.longProps));
      //if (props.strListProps != null) addToResult(getProps(props.strListProps)); There's no .value in strListProps
      if (props.strProps != null) addToResult(getProps(props.strProps));
      if (props.xmlProps != null) addToResult(getProps(props.xmlProps));
    }


    /// <summary>
    /// Execute executeTopologyQueryByNameWithParameters and return result. 
    /// Sketch version! Should be redone to implement good lazy collections return interface.
    /// </summary>
    /// <param name="viewName">Name of the uCMDB view</param>
    /// <param name="parameters">Named "parametrized" parameters of the view</param>
    /// <returns>TopologyMap CIs(Type1Cis[], ...) + Relations(RelType1[], ...)</returns>
    public TopologyMap ExecuteTopologyQueryByNameWithParameters(string viewName, ParameterizedNode[] parameters)
    {
      var request = new executeTopologyQueryByNameWithParameters
                      {
                        cmdbContext = _ctx,
                        queryName = viewName,
                        parameterizedNodes = parameters
                      };

      try
      {
        var response = _svc.executeTopologyQueryByNameWithParameters(request);

        return response.topologyMap;
      }
      catch (Exception e)
      {
        throw new UcmdbFacadeException(String.Format("Topology view {0} request failed", viewName), e);
      }
    }
  }
}
