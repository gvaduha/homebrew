function FindProxyForURL(url, host)
{
if 	(shExpMatch(host, "10.11.1.1") ||
	 shExpMatch(host, "10.10.1.1") ||
         shExpMatch(host,"*my-work.site*"))
{ return ("PROXY host.my.com:8080"); }

else if (shExpMatch(host, "*alarm.shit.com*") ||
         shExpMatch(host,"*disaster.com*"))
{ return ("PROXY 127.0.0.1:777"); }

else if (shExpMatch(host, "*.mydomain*") ||
	shExpMatch(host, "10.1.1.1") ||			
	isPlainHostName(host))
{ return ("DIRECT"); }

else 
{ return ("PROXY proxy.mydomain.com:8080"); }
}
