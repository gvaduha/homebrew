# deferred response REST service template
#curl localhost/exec -X POST -d '{"req":"4a9ffn4fiu388"}'
#curl localhost/getjob/10/1

jobReqURL = '/exec'
jobResultURL = '/getjob'

import random
from flask import Flask, request, jsonify
app = Flask(__name__)

#g++ -c -fPIC helperlib.cpp -o helperlib.o
#g++ -shared -Wl,-soname,libhelperlib.so -o helperlib.so  helperlib.o
from ctypes import cdll
helperlib = cdll.LoadLibrary('./helperlib.so')

def get_wait_resp():
    return {'id': random.choice([1,100]),
                'timeout': random.choice([1000,5000]),
                'url': request.url_root+jobResultURL
    }

def get_resp(req):
    return 'result of c++ call on: '  +req['req'] + ' returned: ' + str(helperlib.get_job_result())

def get_deferred_resp(id):
    return 'JOB: ' + id + ' result: ' + str(helperlib.get_job_result())

@app.route(jobReqURL, methods=['POST'])
def getlic():
    if (random.choice([True,False])):
        resp = get_wait_resp()
        return jsonify(resp), 202 #HTTP 202 ACCEPTED
    else:
        req = request.get_json(force=True) #skip Content-Type
        resp = {'resp': get_resp(req)}
        return jsonify(resp)

@app.route(jobResultURL+"/<path:id>", methods=['GET']) #to support '/' in id
def getjob(id):
    c = random.choice([1,2,3])
    if (c == 1):
        resp = get_wait_resp()
        return jsonify(resp), 202 #HTTP 202 ACCEPTED
    elif (c == 2):
        resp = {'resp': get_deferred_resp(id)}
        return jsonify(resp)
    else:
        return id + ' - NOT FOUND', 404 #job missing


if __name__ == "__main__":
    app.run(port=80)
