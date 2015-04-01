#include <stdint.h>
#include <assert.h>

/////////////////////////////////////////////////////////////////////////////////////////
class BitArray
{
public:
    uint32_t *v;
    const uint32_t size;

    BitArray(uint32_t size =1)
        : size(size)
    {
        v = new uint32_t [size];
    }

    ~BitArray()
    {
        delete v;
    }

    uint32_t * operator[](uint32_t n)
    {
        return getDWord(n);
    }

public:

    uint32_t * getDWord(uint32_t n)
    {
        assert(n < size);
        return v+n;
    }

public:
    /////////////////////////////////////
    // Bit Operations
    //number |= 1 << n; //set bit
    //number &= ~(1 << n); //clear a bit
    //number ^= 1 << n; //toggle a bit
    //bit = (number >> n) & 1; //check a bit
    //number ^= (-value ^ number) & (1 << n); //setting the nth bit to either 1 or 0

    void setBit(uint32_t n)
    {
        uint32_t *pByte = getDWord(n/32);
        *pByte |= 1 << n%32;
    }

    void clearBit(uint32_t n)
    {
        uint32_t *pByte = getDWord(n/32);
        *pByte &= ~(1 << n%32);;
    }

    void toggleBit(uint32_t n)
    {
        uint32_t *pByte = getDWord(n/32);
        *pByte ^= 1 << n%32;
    }

    uint32_t checkBit(uint32_t n)
    {
        uint32_t *pByte = getDWord(n/32);
        return (*pByte >> n) & 1;
    }

    void setBit(uint32_t n, uint32_t val)
    {
        uint32_t *pByte = getDWord(n/32);
        *pByte ^= (-val ^ *pByte) & (1 << n%32);
    }

    /////////////////////////////////////////////////
    // DWord operations

    void andDWord(uint32_t n, uint32_t val)
    {
        uint32_t *pByte = getDWord(n);
        *pByte &= val;
    }

    void orDWord(uint32_t n, uint32_t val)
    {
        uint32_t *pByte = getDWord(n);
        *pByte |= val;
    }

    uint32_t numberOfSetBits(uint32_t n)
    {
        uint32_t byte = * getDWord(n);
        byte = byte - ((byte >> 1) & 0x55555555);
        byte = (byte & 0x33333333) + ((byte >> 2) & 0x33333333);
        return (((byte + (byte >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
    }

    /////////////////////////////////////////////////
    // Array operations

    void and(const BitArray &rhs)
    {
        assert(size == rhs.size);

        for(uint32_t i=0; i<size; ++i)
            andDWord(i, rhs.v[i]);
    }

    void or(const BitArray &rhs)
    {
        assert(size == rhs.size);

        for(uint32_t i=0; i<size; ++i)
            orDWord(i, rhs.v[i]);
    }

    uint32_t numberOfSetBits()
    {
        uint32_t ret = 0;
        for(uint32_t i=0; i<size; ++i)
            ret += numberOfSetBits(i);
        return ret;
    }
};

template <uint32_t L>
class BitArrayT : public BitArray
{
public:
    BitArrayT()
        : BitArray(L)
    {}
};

//int main()
//{
//    BitArray ba(2); ba.v[0] = ba.v[1] = 0;
//
//    ba.setBit(2); assert(ba.v[0] == 4);
//    ba.setBit(32); assert(ba.v[1] == 1);
//    ba.clearBit(2); assert(ba.v[0] == 0);
//    ba.clearBit(32); assert(ba.v[1] == 0);
//    ba.toggleBit(3); assert(ba.v[0] == 8); assert(ba.checkBit(3) == 1);
//    ba.toggleBit(63); /*assert(ba.v[0] == 2147483648L);*/ assert(ba.checkBit(63) == 1);
//    ba.setBit(3,0); assert(ba.v[0] == 0); assert(ba.checkBit(3) == 0);
//    ba.setBit(63,0); assert(ba.v[0] == 0); assert(ba.checkBit(63) == 0);
//
//    ba.v[0] = 0x1AB; ba.andDWord(0, 0x100); assert(ba.v[0] == 0x100);
//    ba.v[1] = 0x88; ba.andDWord(1, 8); assert(ba.v[1] == 8);
//    ba.v[0] = 0x100; ba.orDWord(0, 0x1AB); assert(ba.v[0] == 0x1AB);
//    ba.v[1] = 0x80; ba.orDWord(1, 8); assert(ba.v[1] == 0x88);
//
//    ba.v[0] = 0x514841; assert(ba.numberOfSetBits(0) == 7);
//    ba.v[1] = 0x854052AF; assert(ba.numberOfSetBits(1) == 13);
//    assert(ba.numberOfSetBits() == 20);
//
//    BitArrayT<2> bm; bm.v[0] = 0xFF00AA; bm.v[1] = 0x00BF00;
//    *ba[0] = 0x00FF00; *ba[1] = 0xAA00CC;
//    ba.and(bm);
//    assert(*ba[0] == 0 && *ba[1] == 0);
//
//    *ba[0] = 0x00CD00; *ba[1] = 0x880011;
//    ba.or(bm);
//    assert(*ba[0] == 0xFFCDAA && *ba[1] == 0x88BF11);
//}
