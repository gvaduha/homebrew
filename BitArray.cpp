#include <assert.h>
#include <exception>
#include <stdint.h>

template<typename T>
class BitArray
{
public:
    T *v;
    const uint32_t size;
    static const uint8_t chunkSize = sizeof(T)*8;

    BitArray(uint32_t size =1)
        : size(size)

    {
        v = new T [size];
    }

    BitArray(const BitArray &rhs)
        : size(rhs.size)
    {
        v = new T [size];
        assignTo(rhs);
    }

    BitArray & operator=(const BitArray &rhs)
    {
        if(size != rhs.size);
            throw std::exception("BitArray sizes unequal");

        assignTo(rhs);
        return *this;
    }

    ~BitArray()
    {
        delete v;
    }

    T * operator[](uint32_t n) const 
    {
        return getChunk(n);
    }

protected:

    T * getChunk(uint32_t n) const 
    {
        if (n >= size)
            throw std::exception("BitArray out of bounds");

        return v+n;
    }

    void assignTo(const BitArray &rhs)
    {
        for(T i=0; i<size; ++i)
            *getChunk(i) = *rhs.getChunk(i);
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
        T *pByte = getChunk(n/chunkSize);
        *pByte |= 1 << n%chunkSize;
    }

    void clearBit(uint32_t n)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte &= ~(1 << n%chunkSize);;
    }

    void toggleBit(uint32_t n)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte ^= 1 << n%chunkSize;
    }

    T checkBit(uint32_t n)
    {
        T *pByte = getChunk(n/chunkSize);
        return (*pByte >> n) & 1;
    }

#pragma warning (push)
#pragma warning (disable : 4146)
    void setBit(uint32_t n, uint8_t val)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte ^= (-val ^ *pByte) & (1 << n%chunkSize);
    }
#pragma warning (pop)

    /////////////////////////////////////////////////
    // Chunk operations

    void andChunk(uint32_t n, T val)
    {
        T *pByte = getChunk(n);
        *pByte &= val;
    }

    void orChunk(uint32_t n, T val)
    {
        T *pByte = getChunk(n);
        *pByte |= val;
    }

    void xorChunk(uint32_t n, T val)
    {
        T *pByte = getChunk(n);
        *pByte ^= val;
    }

    T numberOfSetBits(uint32_t n) const
    {
        T byte = * getChunk(n);
        unsigned int c;
        for (c = 0; byte; c++)
            byte &= byte - 1; // clear the least significant bit set

        return c;
    }

    /////////////////////////////////////////////////
    // Array operations

    void andOp(const BitArray &rhs)
    {
        assert(size == rhs.size);

        for(T i=0; i<size; ++i)
            andChunk(i, rhs.v[i]);
    }

    void orOp(const BitArray &rhs)
    {
        assert(size == rhs.size);

        for(T i=0; i<size; ++i)
            orChunk(i, rhs.v[i]);
    }

    void xorOp(const BitArray &rhs)
    {
        assert(size == rhs.size);

        for(T i=0; i<size; ++i)
            xorChunk(i, rhs.v[i]);
    }

    T numberOfSetBits() const
    {
        T ret = 0;
        for(T i=0; i<size; ++i)
            ret += numberOfSetBits(i);
        return ret;
    }
};

template <typename T, uint32_t L>
class BitArrayT : public BitArray<T>
{
public:
    BitArrayT()
        : BitArray(L)
    {}
};

/// BitArray Unit Tests ///
//
void main()
{
    BitArray<uint8_t> ba(2); ba.v[0] = ba.v[1] = 0;
    
    ba.setBit(2); assert(ba.v[0] == 4);
    ba.setBit(8); assert(ba.v[1] == 0x01);
    ba.clearBit(2); assert(ba.v[0] == 0);
    ba.clearBit(8); assert(ba.v[1] == 0);
    ba.toggleBit(3); assert(ba.v[0] == 8); assert(ba.checkBit(3) == 1);
    ba.setBit(3,0); assert(ba.v[0] == 0); assert(ba.checkBit(3) == 0);
    ba.setBit(12,0); assert(ba.v[0] == 0); assert(ba.checkBit(12) == 0);
    
    ba.v[0] = 0xAB; ba.andChunk(0, 0xA0); assert(ba.v[0] == 0xA0);
    ba.v[1] = 0x88; ba.andChunk(1, 8); assert(ba.v[1] == 8);
    ba.v[0] = 0x11; ba.orChunk(0, 0x11); assert(ba.v[0] == 0x11);
    ba.v[1] = 0x80; ba.orChunk(1, 8); assert(ba.v[1] == 0x88);
    
    ba.v[0] = 0x83; assert(ba.numberOfSetBits(0) == 3);
    ba.v[1] = 0x06; assert(ba.numberOfSetBits(1) == 2);
    assert(ba.numberOfSetBits() == 5);
    
    BitArrayT<uint8_t, 2> bm; bm.v[0] = 0xF0; bm.v[1] = 0x1F;
    *ba[0] = 0x00; *ba[1] = 0x1F;
    ba.andOp(bm);
    assert(*ba[0] == 0 && *ba[1] == 0x1F);

    *ba[0] = 0x0F; *ba[1] = 0x10;
    ba.orOp(bm);
    assert(*ba[0] == 0xFF && *ba[1] == 0x1F);
    
    *ba[0] = 0xF0; *ba[1] = 0x10;
    ba.xorOp(bm);
    assert(*ba[0] == 0 && *ba[1] == 0x0F);

    *ba[0] = 0xFF; *ba[1] = 0x88;
    BitArray<uint8_t> bb(ba);
    assert(*ba[0] == *bb[0] && *ba[1] == *bb[1]);

    *ba[0] = 0xAA; *ba[1] = 0x88;
    BitArray<uint8_t> bc = ba;
    assert(*ba[0] == *bc[0] && *ba[1] == *bc[1]);

    //////////////////////////////////////////////////////////////////

    BitArray<uint32_t> ba32(2); ba32.v[0] = ba32.v[1] = 0;
    
    ba32.setBit(2); assert(ba32.v[0] == 4);
    ba32.setBit(32); assert(ba32.v[1] == 1);
    ba32.clearBit(2); assert(ba32.v[0] == 0);
    ba32.clearBit(32); assert(ba32.v[1] == 0);
    ba32.toggleBit(3); assert(ba32.v[0] == 8); assert(ba32.checkBit(3) == 1);
    ba32.toggleBit(63); /*assert(ba32.v[0] == 2147483648L);*/ assert(ba32.checkBit(63) == 1);
    ba32.setBit(3,0); assert(ba32.v[0] == 0); assert(ba32.checkBit(3) == 0);
    ba32.setBit(63,0); assert(ba32.v[0] == 0); assert(ba32.checkBit(63) == 0);
    
    ba32.v[0] = 0x1AB; ba32.andChunk(0, 0x100); assert(ba32.v[0] == 0x100);
    ba32.v[1] = 0x88;  ba32.andChunk(1, 8); assert(ba32.v[1] == 8);
    ba32.v[0] = 0x100; ba32.orChunk(0, 0x1AB); assert(ba32.v[0] == 0x1AB);
    ba32.v[1] = 0x80;  ba32.orChunk(1, 8); assert(ba32.v[1] == 0x88);
    
    ba32.v[0] = 0x514841; assert(ba32.numberOfSetBits(0) == 7);
    ba32.v[1] = 0x854052AF; assert(ba32.numberOfSetBits(1) == 13);
    assert(ba32.numberOfSetBits() == 20);
    
    BitArrayT<uint32_t, 2> bm32; bm32.v[0] = 0xFF00AA; bm32.v[1] = 0x00BF00;
    *ba32[0] = 0x00FF00; *ba32[1] = 0xAA00CC;
    ba32.andOp(bm32);
    assert(*ba32[0] == 0 && *ba32[1] == 0);
    
    *ba32[0] = 0x00CD00; *ba32[1] = 0x880011;
    ba32.orOp(bm32);
    assert(*ba32[0] == 0xFFCDAA && *ba32[1] == 0x88BF11);
    
    *ba32[0] = 0xFF00AA; *ba32[1] = 0x880011;
    ba32.xorOp(bm32);
    assert(*ba32[0] == 0 && *ba32[1] == 0x88BF11);
    
    *ba32[0] = 0xFF00AA; *ba32[1] = 0x880011;
    BitArray<uint32_t> bb32(ba32);
    assert(*ba32[0] == *bb32[0] && *ba32[1] == *bb32[1]);
    
    *ba32[0] = 0xCC00AA; *ba32[1] = 0x880088;
    BitArray<uint32_t> bc32 = ba32;
    assert(*ba32[0] == *bc32[0] && *ba32[1] == *bc32[1]);
}
