
########################################################################
class FlightProvider:
    """Base class for finding flights"""

    #----------------------------------------------------------------------
    def __init__(self):
        self.requests = []
        self.req_index = -1
    
    def __iter__(self):
        self.req_index = -1
        return self
        
    def __next__(self):
        self.req_index += 1
        if len(self.requests) <= self.req_index:
            raise StopIteration
        else:
            return self._get_found_tickets()
    
    def find_tickets (self, dates, route, tickets_number):
        """
        dates - list of tuples [(begin,end), ]
        route - tuple (src, dst)
        tickets_number - number of passengers
        """
        self.requests = self._prepare_requests(dates, route, tickets_number)
    
    def _prepare_requests(self, dates, route, tickets_number):
        """ Return request to target provider. Should be implemented in subclasses """
        pass
    
    def _get_found_tickets(self):
        """ Return found tickets list. Should be implemented in subclasses """
        pass
        
