#! /usr/bin/env python
import sys

print "".join(["%%%02x" % ord(x) for x in sys.argv[1]])
print "".join(["\\u%04x" % ord(x) for x in sys.argv[1]])
