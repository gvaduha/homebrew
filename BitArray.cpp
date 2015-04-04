#include <stdint.h>
#include <exception>
#include <assert.h>

template<typename T>
class BitArray
{
public:
    T *v;
    const T size;
    static const uint8_t chunkSize = sizeof(T)*8;

    BitArray(T size =1)
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
        assert(size == rhs.size);
        assignTo(rhs);
        return *this;
    }

    ~BitArray()
    {
        delete v;
    }

    T * operator[](T n) const 
    {
        return getChunk(n);
    }

protected:

    T * getChunk(T n) const 
    {
        if (n < size)
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

    void setBit(T n)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte |= 1 << n%chunkSize;
    }

    void clearBit(T n)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte &= ~(1 << n%chunkSize);;
    }

    void toggleBit(T n)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte ^= 1 << n%chunkSize;
    }

    T checkBit(T n)
    {
        T *pByte = getChunk(n/chunkSize);
        return (*pByte >> n) & 1;
    }

#pragma warning (push)
#pragma warning (disable : 4146)
    void setBit(T n, T val)
    {
        T *pByte = getChunk(n/chunkSize);
        *pByte ^= (-val ^ *pByte) & (1 << n%chunkSize);
    }
#pragma warning (pop)

    /////////////////////////////////////////////////
    // DWord operations

    void andChunk(T n, T val)
    {
        T *pByte = getChunk(n);
        *pByte &= val;
    }

    void orChunk(T n, T val)
    {
        T *pByte = getChunk(n);
        *pByte |= val;
    }

    void xorChunk(T n, T val)
    {
        T *pByte = getChunk(n);
        *pByte ^= val;
    }

    T numberOfSetBits(T n) const
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

template <typename T, uint8_t L>
class BitArrayT : public BitArray<T>
{
public:
    BitArrayT()
        : BitArray<T>(L)
    {}
};

//int main()
//{
//    /// BitArray Unit Tests ///
//    //
//    BitArray<uint32_t> ba(2); ba.v[0] = ba.v[1] = 0;
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
//    BitArrayT<uint32_t, 2> bm; bm.v[0] = 0xFF00AA; bm.v[1] = 0x00BF00;
//    *ba[0] = 0x00FF00; *ba[1] = 0xAA00CC;
//    ba.andOp(bm);
//    assert(*ba[0] == 0 && *ba[1] == 0);
//
//    *ba[0] = 0x00CD00; *ba[1] = 0x880011;
//    ba.orOp(bm);
//    assert(*ba[0] == 0xFFCDAA && *ba[1] == 0x88BF11);
//
//    *ba[0] = 0xFF00AA; *ba[1] = 0x880011;
//    ba.xorOp(bm);
//    assert(*ba[0] == 0 && *ba[1] == 0x88BF11);
//
//    *ba[0] = 0xFF00AA; *ba[1] = 0x880011;
//    BitArray<uint32_t> bb(ba);
//    assert(*ba[0] == *bb[0] && *ba[1] == *bb[1]);
//
//    *ba[0] = 0xCC00AA; *ba[1] = 0x880088;
//    BitArray<uint32_t> bc = ba;
//    assert(*ba[0] == *bc[0] && *ba[1] == *bc[1]);
////////////////////////////////////////////////////////////////////////////
//BitArray<uint8_t> ba(2); ba.v[0] = ba.v[1] = 0;
//
//ba.setBit(2); assert(ba.v[0] == 4);
//ba.setBit(8); assert(ba.v[1] == 0x01);
//ba.clearBit(2); assert(ba.v[0] == 0);
//ba.clearBit(8); assert(ba.v[1] == 0);
//ba.toggleBit(3); assert(ba.v[0] == 8); assert(ba.checkBit(3) == 1);
//ba.setBit(3,0); assert(ba.v[0] == 0); assert(ba.checkBit(3) == 0);
//ba.setBit(12,0); assert(ba.v[0] == 0); assert(ba.checkBit(12) == 0);
//
//ba.v[0] = 0xAB; ba.andChunk(0, 0xA0); assert(ba.v[0] == 0xA0);
//ba.v[1] = 0x88; ba.andChunk(1, 8); assert(ba.v[1] == 8);
//ba.v[0] = 0x11; ba.orChunk(0, 0x11); assert(ba.v[0] == 0x11);
//ba.v[1] = 0x80; ba.orChunk(1, 8); assert(ba.v[1] == 0x88);
//
//ba.v[0] = 0x83; assert(ba.numberOfSetBits(0) == 3);
//ba.v[1] = 0x06; assert(ba.numberOfSetBits(1) == 2);
//assert(ba.numberOfSetBits() == 5);
//
//BitArrayT<uint8_t, 2> bm; bm.v[0] = 0xF0; bm.v[1] = 0x1F;
//*ba[0] = 0x00; *ba[1] = 0x1F;
//ba.andOp(bm);
//assert(*ba[0] == 0 && *ba[1] == 0x1F);

//*ba[0] = 0x0F; *ba[1] = 0x10;
//ba.orOp(bm);
//assert(*ba[0] == 0xFF && *ba[1] == 0x1F);
//
//*ba[0] = 0xF0; *ba[1] = 0x10;
//ba.xorOp(bm);
//assert(*ba[0] == 0 && *ba[1] == 0x0F);

//*ba[0] = 0xFF; *ba[1] = 0x88;
//BitArray<uint8_t> bb(ba);
//assert(*ba[0] == *bb[0] && *ba[1] == *bb[1]);

//*ba[0] = 0xAA; *ba[1] = 0x88;
//BitArray<uint8_t> bc = ba;
//assert(*ba[0] == *bc[0] && *ba[1] == *bc[1]);
//}
