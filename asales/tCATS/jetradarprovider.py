from .flightprovider import FlightProvider

########################################################################
class JetRadarFlightProvider(FlightProvider):
    """ jetradar.com search result provider """

    site_url = 'www.jetradar.com'
    request_uri = '/XXXXXXX?'
    
    #----------------------------------------------------------------------
    def __init__(self):
        """Constructor"""
        
