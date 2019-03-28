#! /usr/bin/env python
import sys

arg = sys.argv[1]
print arg
print "".join(["%%%02x" % ord(x) for x in arg])
print "".join(["\\u%04x" % ord(x) for x in arg])
