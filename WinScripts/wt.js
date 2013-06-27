var ServiceUrl = "http://server:81/WebServices/WebTutorService.asmx";
var SubsystemCode = "WT";
var UserLogin = "USER";
var Password = "";
var InterfaceName = "GetPersons";
// InterfaceName = "GetDepartments";
var SoapAction="http://www.company.com/webservices/SendRequest";


var SoapEnvelope = "\
<?xml version='1.0' encoding='utf-8'?>\
<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' \
               xmlns:xsd='http://www.w3.org/2001/XMLSchema' \
               xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'> \
  <soap:Header> \
    <Auth xmlns='http://www.company.com/webservices/'> \
      <S>"+SubsystemCode+"</S> \
      <U>"+UserLogin+"</U> \
      <P>"+Password+"</P> \
    </Auth> \
  </soap:Header> \
  <soap:Body> \
    <SendRequest xmlns='http://www.company.com/webservices/'> \
      <interfaceName>"+InterfaceName+"</interfaceName> \
      <request> \
      </request> \
    </SendRequest> \
  </soap:Body> \
</soap:Envelope> \
";

WScript.Echo(SoapEnvelope);

var oXml = new ActiveXObject("msxml2.xmlhttp");
oXml.open("POST", ServiceUrl, false);
oXml.setRequestHeader("Content-Type", "text/xml; charset=utf-8;");
oXml.setRequestHeader("SoapAction", SoapAction);
oXml.send(SoapEnvelope);

var ResponseText = oXml.responseXml;

WScript.Echo(ResponseText.text);
