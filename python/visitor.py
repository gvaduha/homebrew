import random

class Weapon:
    axe = 'Axe'
    sword = 'Sword'
    lance = 'Lance'
    bow = 'Bow'
    spell = 'Spell'
    cmpTable = {Weapon.axe: 30, Weapon.sword: 10, Weapon.lance: 40, Weapon.bow: 20, Weapon.spell: 50}
    @classmethod
    def firstIsWin(cls, weapon1, weapon2):
        return Weapon.cmpTable[weapon1] > Weapon.cmpTable[weapon2]


class Warrior:
    def challenge(self, warrior):
        '''Visited.accept function'''
        myWeapon = self.getWeapon()
        hisWeapon = warrior.getWeapon()
        res = Weapon.firstIsWin(myWeapon, hisWeapon)
        print('%s whith %s %s %s with %s' %
              (self.__class__.__name__, myWeapon, res and 'WIN' or 'LOOSE', warrior.__class__.__name__, hisWeapon))
    def getWeapon(self):
        pass

class Dwarf(Warrior):
    def getWeapon(self):
        return random.choice((Weapon.axe,Weapon.lance))

class Elf(Warrior):
    def getWeapon(self):
        return random.choice((Weapon.sword,Weapon.bow))

class Troll(Warrior):
    def getWeapon(self):
        return random.choice((Weapon.spell,Weapon.sword,Weapon.axe))
