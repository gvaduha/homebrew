import urllib.request as urlreq

#def yoba():
#    import httplib2
#    http = httplib2.Http(proxy_info = httplib2.ProxyInfo(r'http://bsgv\ba003595:Go88Adventure@bsgv-ssmospiapi.bsgv.eu.isam.socgen:8080')


def urlopen(url, headers = {}):
    from http.client import HTTPConnection as con
    con.debuglevel = 1
    proxy = urlreq.ProxyHandler({
        'http' : r'http://bsgv\ba003595:XXX@bsgv-ssmospiapi.bsgv.eu.isam.socgen:8080',
        'https' : r'https://bsgv\ba003595:XXX@bsgv-ssmospiapi.bsgv.eu.isam.socgen:8080',
        'ftp' : r'ftp://bsgv\ba003595:XXX@bsgv-ssmospiapi.bsgv.eu.isam.socgen:8080'
        })
    auth = urlreq.HTTPBasicAuthHandler() # no need in this
    error = DefaultErrorHandler()
    opener = urlreq.build_opener(proxy, error, auth, urlreq.HTTPHandler(debuglevel=1))
    urlreq.install_opener(opener)
    request = urlreq.Request(url)
    for k,v in headers.items(): request.add_header(k,v)
    sock = opener.open(request)
    #htmlSource = sock.read()
    #sock.close()
    #return htmlSource
    return sock


class DefaultErrorHandler(urlreq.HTTPDefaultErrorHandler):    
    def http_error_default(self, req, fp, code, msg, headers): 
        result = urlreq.HTTPError(                           
            req.get_full_url(), code, msg, headers, fp)       
        result.status = code                                   
        return result


if __name__ == "__main__":
    print(urlopen('http://ping.eu'))
    

