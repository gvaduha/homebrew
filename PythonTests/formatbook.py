import sys

def format(filename):
        def printbuff(buff):
                if len(buff) > 0: print(buff)
        with open(filename) as f:
                buff = ''
                for line in f:
                        if line.isspace() and len(buff) == 0: #skip lots of empty lines
                                continue
                        if line.isspace():
                                printbuff(buff)
                                print()
                                buff = ''
                                continue
                        elif line[0].isspace() \
                             or line[0] == r'"':  #direct speech?
                                printbuff(buff)
                                buff = ''
                        buff += line.rstrip('\r\n') + ' '
                print(buff)

if __name__ == "__main__":
        if len(sys.argv) < 2:
                print("Error: input file name must be given")
        else:
                format(sys.argv[1])
