# www.python.org/idle
import sys
import json

configFile = r'x:\tcats.js'

'''
Initialize request map from given JSON config file
Return: [{'/search/v0': ('mod.dm1', 'mod.rt1', 'mod.agg1')}, ...]
'''
def initRequestMap():
        f = open(configFile)
        c = json.load(f)

        reqhandlers = []
        reqMap = {}

        # Create handlers map for modules import

        #Import modules from handlers definitions: *mod*.dm1...
        modules = set([f.split('.')[0] for r in c['requests'].values() for h in r['handlers'] for f in h.values()])

        print('Import modules: %s' % modules)
        list(map(__import__, modules))
        
        '''
        for r in c['requests']:
                aggr = c['requests'][r]['aggregator']
                modules.append(aggr.split('.')[0])
                for h in c['requests'][r]['handlers']:
                        req = h['req']
                        modules.append(req.split('.')[0])
                        res = h['res']
                        modules.append(res.split('.')[0])
                        reqhandlers.append([r,req,res,aggr])


        x = [{url:(req,res,agg)} for url,req,res,agg in reqhandlers]

        #for r in reqhandlers:
'''                
