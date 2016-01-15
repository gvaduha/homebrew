#include <stdexcept>
#include <assert.h>
#include <stdint.h>

template<typename T>
class BitArray
{
public:
    T *v;
    const uint32_t size;
    static const uint8_t chunkByteSize = sizeof(T);
    static const uint8_t chunkBitSize = chunkByteSize*8;

    BitArray(uint32_t size =1)
        : size(size)

    {
        v = new T [size];
        memset(v,0,size*chunkByteSize);
    }

    BitArray(const BitArray &rhs)
        : size(rhs.size)
    {
        v = new T [size];
        assignTo(rhs);
    }

    BitArray & operator=(const BitArray &rhs)
    {
        if (this == &rhs)
            return *this;

        if(size != rhs.size)
            throw std::invalid_argument("BitArray sizes unequal");

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

    bool operator==(const BitArray &rhs)
    {
        if(size != rhs.size)
            throw std::invalid_argument("BitArray sizes unequal");

        return !memcmp(this->v, rhs.v, size*chunkByteSize);
    }

    bool operator!=(const BitArray &rhs)
    {
        return !operator==(rhs);
    }

    //TODO:
    //R K::operator ~();
    //R K::operator &(S b);
    //R K::operator |(S b);
    //R K::operator ^(S b);
    //R K::operator <<(S b);
    //R K::operator >>(S b);

protected:

    T * getChunk(uint32_t n) const 
    {
        if (n >= size)
            throw std::invalid_argument("BitArray out of bounds");

        return v+n;
    }

    void assignTo(const BitArray &rhs)
    {
        // arrays size equality should be checked by public functions
        memcpy(v, rhs.v, size*chunkByteSize);
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
        T *pByte = getChunk(n/chunkBitSize);
        *pByte |= 1 << n%chunkBitSize;
    }

    void clearBit(uint32_t n)
    {
        T *pByte = getChunk(n/chunkBitSize);
        *pByte &= ~(1 << n%chunkBitSize);;
    }

    void toggleBit(uint32_t n)
    {
        T *pByte = getChunk(n/chunkBitSize);
        *pByte ^= 1 << n%chunkBitSize;
    }

    bool isBitSet(uint32_t n)
    {
        T *pByte = getChunk(n/chunkBitSize);
        return (*pByte >> n%chunkBitSize) & 1;
    }

#pragma warning (push)
#pragma warning (disable : 4146)
    void setBit(uint32_t n, uint8_t val)
    {
        T *pByte = getChunk(n/chunkBitSize);
        *pByte ^= (-val ^ *pByte) & (1 << n%chunkBitSize);
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

    void notChunk(uint32_t n)
    {
        T *pByte = getChunk(n);
        *pByte ~= *pByte;
    }

    uint32_t numberOfSetBits(uint32_t n) const
    {
        T byte = * getChunk(n);
        uint32_t c;
        for (c = 0; byte; c++)
            byte &= byte - 1; // clear the least significant bit set

        return c;
    }

    /////////////////////////////////////////////////
    // Array operations

    /*
    size_t len = size * chunkByteSize;
    size_t const loadsPerCycle = 4;
    size_t iters = len / sizeof(long long) / loadsPerCycle;

    long long *ptr = (long long *) v;
    long long *ptr2 = (long long *) rhs.v;

    for (size_t i = 0; i < iters; ++i) {
    size_t j = loadsPerCycle*i;
    ptr[j  ] OPER##= ptr2[j  ];
    ptr[j+1] OPER##= ptr2[j+1];
    ptr[j+2] OPER##= ptr2[j+2];
    ptr[j+3] OPER##= ptr2[j+3];
    }

    for (size_t i = iters * loadsPerCycle; i < len / sizeof(long long); ++i) {
    ptr[i] OPER##= ptr2[i];
    }

    for (size_t i = (len / sizeof(long long)) * sizeof(long long); i < len; i+=chunkByteSize) {
    v[i/chunkByteSize] OPER##= rhs.v[i/chunkByteSize];
    }
    */
#define FAST_ARRAY_OP(OPER) \
        size_t len = size * chunkByteSize; \
        size_t const loadsPerCycle = 4; \
        size_t iters = len / sizeof(long long) / loadsPerCycle; \
        \
        long long *ptr = (long long *) v; \
        long long *ptr2 = (long long *) rhs.v; \
        \
        for (size_t i = 0; i < iters; ++i) { \
            size_t j = loadsPerCycle*i; \
            ptr[j  ] OPER##= ptr2[j  ]; \
            ptr[j+1] OPER##= ptr2[j+1]; \
            ptr[j+2] OPER##= ptr2[j+2]; \
            ptr[j+3] OPER##= ptr2[j+3]; \
        } \
        \
        for (size_t i = iters * loadsPerCycle; i < len / sizeof(long long); ++i) { \
            ptr[i] OPER##= ptr2[i]; \
        } \
        \
        for (size_t i = (len / sizeof(long long)) * sizeof(long long); i < len; i+=chunkByteSize) { \
            v[i/chunkByteSize] OPER##= rhs.v[i/chunkByteSize]; \
        }

     void andOp(const BitArray &rhs)
     {
         FAST_ARRAY_OP(&)
     }

    void orOp(const BitArray &rhs)
    {
        FAST_ARRAY_OP(|)
    }

    void xorOp(const BitArray &rhs)
    {
        FAST_ARRAY_OP(^)
    }

    //TODO:
    //void notOp()

    uint32_t numberOfSetBits() const
    {
        uint32_t ret = 0;
        for(uint32_t i=0; i<size; ++i)
            ret += numberOfSetBits(i);
        return ret;
    }
};

template <typename T, uint32_t L>
class BitArrayT : public BitArray<T>
{
public:
    BitArrayT()
        : BitArray<T>(L)
    {}
};

//////////////////////////////////////////////////////////////////
/// BitArray Unit Tests
//

template<typename T>
T getResult(T val)
{
    T ret=0;

    for (uint8_t i=0; i<sizeof(T); ++i)
        ret += val << 8*i;

    return ret;
}

template<typename T>
void checkBigArray(size_t size)
{
    BitArray<T> lx(size);
    BitArray<T> ly(size);
    memset(ly.v, 0xCC, size*sizeof(T)); // 1100 1100 ...

    memset(lx.v, 0xAA, size*sizeof(T)); // 1010 1010 ...
    lx.andOp(ly);
    assert(*lx[0] == getResult<T>(0x88));
    assert(*lx[size-1] == getResult<T>(0x88));

    memset(lx.v, 0xAA, size*sizeof(T)); // 1010 1010 ...
    lx.orOp(ly);
    assert(*lx[0] == getResult<T>(0xEE));
    assert(*lx[size-1] == getResult<T>(0xEE));

    memset(lx.v, 0xAA, size*sizeof(T)); // 1010 1010 ...
    lx.xorOp(ly);
    assert(*lx[0] == getResult<T>(0x66));
    assert(*lx[size-1] == getResult<T>(0x66));
}

int main()
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

    assert(ba == bc);
    assert(bc != bb);

    ////////////////////////////////////////////////////////////////////

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

    assert(ba32 == bc32);
    assert(bc32 != bb32);

    // Big array tests /////////////////////////////////////////////////////

    const size_t max_vector_size = 32000;

    for (size_t i=1; i<max_vector_size; ++i)
    {
        checkBigArray<uint8_t>(i);
        checkBigArray<uint16_t>(i);
        checkBigArray<uint32_t>(i);
        checkBigArray<uint64_t>(i);
    }

    return 1;
}
