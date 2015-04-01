#include <stdint.h>
#include <assert.h>

/////////////////////////////////////////////////////////////////////////////////////////
struct BitArray
{
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

    uint32_t* getByte(uint32_t n)
    {
        assert(n < size);
        return v+n;
    }

    // Operations
    //number |= 1 << n; //set bit
    //number &= ~(1 << n); //clear a bit
    //number ^= 1 << n; //toggle a bit
    //bit = (number >> n) & 1; //check a bit
    //number ^= (-value ^ number) & (1 << n); //setting the nth bit to either 1 or 0

    void setBit(uint32_t n)
    {
        uint32_t* pByte = getByte(n/32);
        *pByte |= 1 << n%32;
    }

    void clearBit(uint32_t n)
    {
        uint32_t* pByte = getByte(n/32);
        *pByte &= ~(1 << n%32);;
    }

    void toggleBit(uint32_t n)
    {
        uint32_t* pByte = getByte(n/32);
        *pByte ^= 1 << n%32;
    }

    uint32_t checkBit(uint32_t n)
    {
        uint32_t* pByte = getByte(n/32);
        return (*pByte >> n) & 1;
    }

    void setBit(uint32_t n, uint32_t val)
    {
        uint32_t* pByte = getByte(n/32);
        *pByte ^= (-val ^ *pByte) & (1 << n%32);
    }
};
