using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ucmdb
{
  //public class UcmdbDataRetriever
  //{
  //  private readonly UcmdbService.UcmdbService _svc = new UcmdbService.UcmdbService();
  //  private readonly CmdbContext _ctx;

  //  public UcmdbDataRetriever(Uri ucmdbUri, NetworkCredential credentials, string appContextName = "")
  //  {
  //    var credentialCache = new CredentialCache { { ucmdbUri, "Basic", credentials } };
  //    _svc.Credentials = credentialCache;
  //    _ctx = new CmdbContext { callerApplication = "RosStaff" };
  //  }

  //  /// <summary>
  //  /// 
  //  /// </summary>
  //  /// <param name="type"></param>
  //  /// <param name="properties"></param>
  //  /// <param name="constraints"></param>
  //  /// <returns>List of dictionary records</returns>
  //  public IEnumerable<IEnumerable<KeyValuePair<string, string>>> GetFilteredCiByType(string type, ISet<string> properties, Conditions conditions)
  //  {
  //    var request = new getFilteredCIsByType
  //    {
  //      cmdbContext = _ctx,
  //      type = "cc_employee",
  //      conditions = conditions,
  //      conditionsLogicalOperator = new getFilteredCIsByTypeConditionsLogicalOperator(),
  //      properties = new CustomProperties { propertiesList = properties.ToArray() }
  //    };

  //    // Send request and processing response
  //    try
  //    {
  //      var response = _svc.getFilteredCIsByType(request);

  //      return ProcessResponse(response, properties);
  //    }
  //    catch (Exception e)
  //    {
  //      Console.WriteLine(e.ToString());
  //      throw e;
  //    }
  //  }

  //  private IEnumerable<IEnumerable<KeyValuePair<string, string>>> ProcessResponse(dynamic response, ISet<string> propNames)
  //  {
  //    if (response.chunkInfo.numberOfChunks == 0) // Number of chunks is 0 when all results fitted in response
  //    {
  //      foreach (CI ci in response.CIs)
  //        yield return ProcessCi(ci, propNames);
  //    }
  //    else // Read all chunks and process them
  //    {
  //      var chunkRequest = new ChunkRequest { chunkInfo = response.chunkInfo };

  //      for (int chunkNumber = 1; chunkNumber <= response.chunkInfo.numberOfChunks; chunkNumber++)
  //      {
  //        chunkRequest.chunkNumber = chunkNumber;
  //        var ptmc = new pullTopologyMapChunks { cmdbContext = _ctx, ChunkRequest = chunkRequest };

  //        var ptmcResponse = _svc.pullTopologyMapChunks(ptmc);

  //        foreach (var ci in ptmcResponse.topologyMap.CINodes.SelectMany(ciNode => ciNode.CIs))
  //        {
  //          yield return ProcessCi(ci, propNames);
  //        }
  //      }

  //      _svc.releaseChunks(new releaseChunks { chunksKey = response.chunkInfo.chunksKey, cmdbContext = _ctx });
  //    }
  //  }

  //  /// <summary>
  //  /// Retrieves name value pairs from given ci
  //  /// </summary>
  //  /// <param name="ci"></param>
  //  /// <param name="propNames">Name of properties to return</param>
  //  /// <returns></returns>
  //  public virtual IEnumerable<KeyValuePair<string, string>> ProcessCi(CI ci, ISet<string> propNames)
  //  {
  //    //Console.WriteLine("CI type: " + ci.type + ", CI ID: " + ci.ID.Value);

  //    var result = new List<KeyValuePair<string, string>>();

  //    //Always add id
  //    result.Add(new KeyValuePair<string, string>("id", ci.ID.Value));

  //    foreach (var strProp in ci.props.strProps)
  //    {
  //      result.Add(new KeyValuePair<string, string>(strProp.name, strProp.value));
  //    }

  //    return result;
  //  }

  //  public void xx()
  //  {
  //    getCIsById req = new getCIsById();
  //    req.cmdbContext = _ctx;

  //    ID id = new ID();
  //    id.Value = "016e40e25f5fbe4c0c9b7a0306f9abfc";
  //    ID[] ids = new ID[] { id };

  //    req.IDs = ids;

  //    try
  //    {
  //      getCIsByIdResponse response = _svc.getCIsById(req);
  //      CI[] cis = response.CIs;
  //      ChunkInfo chunkInfo = response.chunkInfo;

  //      Console.WriteLine("\nReceived cis = " + cis.Length + "\n");

  //      // print cis

  //      foreach (CI ci in cis)
  //      {
  //        ProcessCi(ci, null);
  //      }
  //    }
  //    catch (Exception e)
  //    {
  //      Console.WriteLine(e.ToString());
  //      Console.WriteLine("\n\n Check the exception message for details.");
  //      Console.ReadLine();
  //      //throw e;
  //    }

  //  }

  //  public void TestA()
  //  {
  //    var props = new HashSet<string>
  //                  {
  //                    "ca_outstaff", "ca_login_name", "ca_location", "ca_last_name", "ca_job_description",
  //                    "ca_middle_name"
  //                  };

  //    var cond = new BooleanCondition
  //                 {
  //                   booleanOperator = BooleanConditionBooleanOperator.Equal,
  //                   condition = new BooleanProp {name = "ca_outstaff", value = true, valueSpecified = true}
  //                 };

  //    var request = new getFilteredCIsByType
  //                    {
  //                      cmdbContext = _ctx,
  //                      type = "cc_employee",
  //                      conditions =
  //                        new Conditions
  //                          {
  //                            booleanConditions =
  //                              new BooleanConditions {booleanCondition = new[] {cond}}
  //                          },
  //                      conditionsLogicalOperator = new getFilteredCIsByTypeConditionsLogicalOperator(),
  //                      properties = new CustomProperties { propertiesList =  props.ToArray() }
  //                    };

  //    // Send request and processing response
  //    try
  //    {
  //      var response = _svc.getFilteredCIsByType(request);

  //      foreach (var x in ProcessResponse(response, props))
  //      {
  //        foreach (var y in x) Console.WriteLine(y.Key+"="+y.Value);
  //        Console.WriteLine("-----------------------------------");
  //      }
  //    }
  //    catch (Exception e)
  //    {
  //      Console.WriteLine(e.ToString());
  //      //throw e;
  //    }

  //  }

  //  public void zz()
  //  {
  //    var request = new getCIsByType { cmdbContext = _ctx, type = "cc_employee" };
  //    // CIT
  //    // properties
  //    CustomProperties customProperties = new CustomProperties();
  //    PredefinedProperties predefinedProperties = new PredefinedProperties();
  //    SimplePredefinedProperty simplePredefinedProperty = new SimplePredefinedProperty();
  //    simplePredefinedProperty.name = SimplePredefinedPropertyName.CONCRETE;
  //    SimplePredefinedProperty[] simplePredefinedPropertyCollection = new SimplePredefinedProperty[1];
  //    simplePredefinedPropertyCollection[0] = simplePredefinedProperty;
  //    predefinedProperties.simplePredefinedProperties = simplePredefinedPropertyCollection;
  //    customProperties.predefinedProperties = predefinedProperties;
  //    request.properties = customProperties;
  //    try
  //    {
  //      var response = _svc.getCIsByType(request);

  //      ProcessResponse(response, null);
  //    }
  //    catch (Exception e)
  //    {
  //      Console.WriteLine(e.ToString());
  //      //throw e;
  //    }
  //  }
  //}

}
