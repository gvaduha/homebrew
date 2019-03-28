from threading import Lock

class Synchronized:
    def __init__(self):
        self.lock = Lock()

def synchronized(func, *args):
    def sync_imp(*args):
        self = args[0]
        with self.lock:
            func(*args)
    return sync_imp


class Observer:
    def __init__(self, name):
        self.name = name
    def update(self, observable):
        print(self.name+" updated with: "+str(observable.data))


class Observable(Synchronized):
    def __init__(self, data):
        super(Observable, self).__init__()
        self.isChanged = False
        self.data = data
        self.observers = []
    def addObserver(self, observer):
        self.observers.append(observer)
    def deleteObserver(self, observer):
        self.observers.remove(observer)
    def changeData(self, val):
        self.data = val
        self.isChanged = True
    @synchronized
    def updateAll(self):
        if self.isChanged:
            copy = self.data
            self.isChanged = False
            for o in self.observers:
                o.update(self)


class Data(Observable):
    def triggerUpdate(self):
        self.updateAll()

