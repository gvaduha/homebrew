'''
Command -partof-> Strategy -partof-> ChainOfResponsibility
Command 1 of Strategies
Strategy solver - logic to change Commands/Strategies
COR - logic to find solution or solutions
decouple COR from Command/Strategy hierarchy
'''
class Problem:
    def __init__(self, data):
        self.data = data

class Solution:
    def __init__(self, klass):
        self.klass = klass
    def __str__(self):
        return self.klass.__class__.__name__

class Algorithm:
    '''Strategy/Command base'''
    def __init__(self):
        self.solved = False
    def solve(self, problem):
        pass

class Xalg(Algorithm):
    def solve(self, problem):
        try:
            problem.data.index('xalg') #just try
            return Solution(self)
        except:
            pass

class Yalg(Algorithm):
    def solve(self, problem):
        try:
            problem.data.index('yalg') #just try
            return Solution(self)
        except:
            pass

        
class ProblemSolver:
    '''Chain of responsibility'''
    def __init__(self, algorithms):
        self.alg_chain = algorithms
    def add(self, algorithm):
        self.alg_chain.append(algorithm)
    def __call__(self, problem):
        for alg in self.alg_chain:
            result = alg.solve(problem)
            if result:
                return result
        #return None

