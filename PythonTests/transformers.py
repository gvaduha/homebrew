class CpTransformBase:
    ''' Implement cp in1 in2 ... out with additional input file transform
    '''
    def __init__(self,*args):
        self.inp = list(args)[:-1]
        self.out = list(args)[-1]
    def __str__(self):
        return "cp %s -> %s with %s" % (self.inp, self.out, self.transform.__doc__)
    def cp(self):
        with open(self.out, mode='w') as fout:
            list(map(lambda x: self.__cp(x, fout=fout), self.inp))
    def __cp(self, inname, fout):
        with open(inname) as fin:
            res = self.transform(fin.read())
            fout.write(res)
    def transform(self, inp):
        '''no transform'''
        return inp

class CpUppercaseTransform(CpTransformBase):
    def transform(self, inp):
        '''uppercase'''
        return inp.upper()

