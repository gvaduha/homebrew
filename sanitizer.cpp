#include <functional>
#include <algorithm>

#include "Sanitizers.h"

using std::string;
using std::find_if;

SymbolNotInRangeOrList<unsigned char> httpHeaderSymbols(0x20,0x7F, kBnfSeparators, kBnfSeparatorsSize);
SymbolInRange<unsigned char> httpHeaderValueSymbols(0x20,0xFF);


///////////////////////////////////////////////////////////////////////////////
// sanitizers
//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::sanitizeHeader(string &header) const
{
    return sanitize_if(header, httpHeaderSymbols);
}

//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::sanitizeHeader(const char *header, size_t headerSize, char *outHeader, size_t &outSize) const
{
    return sanitize_if(header, headerSize, outHeader, outSize, httpHeaderSymbols);
}

//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::sanitizeValue(string &value) const
{
    return sanitize_if(value, httpHeaderValueSymbols);
}

//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::sanitizeValue(const char *value, size_t valueSize, char *outValue, size_t &outSize) const
{
    return sanitize_if(value, valueSize, outValue, outSize, httpHeaderValueSymbols);
}

///////////////////////////////////////////////////////////////////////////////
// checkers
//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::headerHasIllegalSymbols(const string &header) const
{
    return header.end() != find_if(header.begin(), header.end(), httpHeaderSymbols);
}

//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::headerHasIllegalSymbols(const char *header, size_t headerSize) const
{
    const char *headerend = header + headerSize*sizeof(char);
    return headerend != find_if(header, headerend, httpHeaderSymbols);
}

//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::valueHasIllegalSymbols(const string &value) const
{
    return value.end() != find_if(value.begin(), value.end(), std::not1(httpHeaderValueSymbols));
}

//---------------------------------------------------------------------------//
bool HttpHeadersSanitizer::valueHasIllegalSymbols(const char *value, size_t valueSize) const
{
    const char *valueend = value + valueSize*sizeof(char);
    return valueend != find_if(value, valueend, std::not1(httpHeaderValueSymbols));
}
