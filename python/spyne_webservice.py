"""
USE: Wizdler Chrome Extension
from suds.client import Client
c = Client('http://localhost:8008/...?wsdl')
c.service.SmokeTest('XXX')
"""

response_file = '$RESP_FILE'

from spyne import Application, rpc, ServiceBase, Iterable, Integer, Unicode
from spyne.protocol.soap import Soap11
from spyne.server.wsgi import WsgiApplication


class TestService(ServiceBase):
    __service_url_path__ = '/TestCenter/UC1/smoke.test'
    #__in_protocol__ = Soap11(validator='lxml')
    #__out_protocol__ = Soap11()

    @rpc(Unicode, _returns=Unicode)
    def SmokeTest(ctx, req):
        """Test stub
        <b>Base test:</b>
        @req input request 
        @return predefined file content
        """
	f = open(response_file, 'r')
	return req
        return f.read()


application = Application([TestService], 'TestServiceNamespace',
        in_protocol=Soap11(validator='lxml'),
        out_protocol=Soap11()
    )

wsgi_application = WsgiApplication(application)


if __name__ == '__main__':
    import logging

    from wsgiref.simple_server import make_server

    logging.basicConfig(level=logging.DEBUG)
    logging.getLogger('spyne.protocol.xml').setLevel(logging.DEBUG)

    logging.info("Service started")

    server = make_server('127.0.0.1', 8008, wsgi_application)
    server.serve_forever()
