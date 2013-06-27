with open(r'c:\aff.txt') as f:
	with open(r'c:\yyy', mode='w') as of:
		buff = ''
		for line in f:
			if line.isspace():
				of.write(buff + '\n' + line)
				buff = ''
			if line[0] == r'"':
				if len(buff)>0: of.write(buff + '\n')
				buff = line.rstrip('\n')
			else:
				buff += line.rstrip('\n')
		of.write(buff)
