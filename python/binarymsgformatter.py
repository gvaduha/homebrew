import sys

def prnfmt(fmtfile, valfile):
    columns = []
    with open(fmtfile) as f:
        for line in f:
            col = (a,b,c,d,e,f,g) = line.rstrip('\n').split(',')
            columns.append(col)
    with open(valfile,encoding='cp866') as f:
        for line in f:
            line = line.rstrip('\n')
            for col in columns:
                print("[%s] %s\t [%s:%s]:\t |%s|" % (col[6], int(col[0]-1), col[5], col[2], line[int(col[5])-1:int(col[5])+int(col[2])-1])) #mind fu10 offset [1]vs[5]
            print('\n===================================================\n')            
        
if __name__ == "__main__":
    if len(sys.argv) < 3:
        print('Please use 2 arguments: format_file_name.csv input_message_file_name')
    else:
        prnfmt(sys.argv[1], sys.argv[2])

