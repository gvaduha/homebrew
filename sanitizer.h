
#ifndef __SANITIZERSIMPL_H__
#define __SANITIZERSIMPL_H__

namespace Sanitization
{

const uint8_t kBnfSeparatorsSize = 19;
const unsigned char kBnfSeparators[kBnfSeparatorsSize+1] = "()<>@,;:\\\"/[]?={} \t";


/*
 * HTTP Headers sanitization (ad-hoc as it didn't respect symbols below 0x20)
 * https://www.w3.org/Protocols/rfc2616/rfc2616-sec2.html#sec2.2
 * https://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4.2
*/
class HttpHeadersSanitizer
{
public:
    /* 
    * Remove all except 0x19 < char < 0x80
    *  also remove "(" | ")" | "<" | ">" | "@" | "," | ";" | ":" | "\" | <">
    *            | "/" | "[" | "]" | "?" | "=" | "{" | "}" | SP | HT
    */
    static bool sanitizeHeader(std::string &header) const;
    static bool sanitizeHeader(const char *header, size_t headerSize, char *outHeader, size_t &outSize) const;
    static bool headerHasIllegalSymbols(const std::string &header) const;
    static bool headerHasIllegalSymbols(const char *header, size_t headerSize) const;

    /*
    * Remove all except 0x19 < char < 0xFF
    */
    static bool sanitizeValue(std::string &value) const;
    static bool sanitizeValue(const char *value, size_t valueSize, char *outValue, size_t &outSize) const;
    static bool valueHasIllegalSymbols(const std::string &value) const;
    static bool valueHasIllegalSymbols(const char *value, size_t valueSize) const;
};

//---------------------------------------------------------------------------//
template<class argchar_t>
struct SymbolInRange
{
    typedef argchar_t argument_type;
    argument_type low, hi;

    SymbolInRange(argument_type low, argument_type hi)
        : low(low), hi(hi)
    {}

    bool operator()(argument_type input) const
    {
        return input >= low && input <= hi;
    }
};

//---------------------------------------------------------------------------//
template<class argchar_t>
struct SymbolNotInRangeOrList
{
    typedef argchar_t argument_type;
    argument_type low, hi;
    const argchar_t *list;
    uint8_t listSize;

    SymbolNotInRangeOrList(argument_type low, argument_type hi, const argument_type *list, uint8_t listSize)
        : low(low), hi(hi), list(list), listSize(listSize)
    {}

    bool operator()(argument_type input) const
    {
        const argument_type *listend = list + listSize*sizeof(argument_type);
        return input < low || input > hi || std::find(list, listend, input) != listend;
    }
};

//-----------------------------------------------------------------------//
template<class argchar_t, class predicate_t>
bool sanitize_if(std::basic_string<argchar_t> &input, predicate_t predicate)
{
    typedef std::basic_string<argchar_t> string_t;
    string_t::iterator scanpos = input.begin();
    string_t::iterator rewritepos = NULL;
    string_t::const_iterator endit = input.end();
    bool sanitized = false;

    for(; scanpos!=endit; ++scanpos)
    {
        if (predicate(*scanpos))
        {
            if (!sanitized)
            {
                rewritepos = scanpos;
                sanitized = true;
            }
        }
        else if (sanitized)
        {
            *rewritepos++ = *scanpos;
        }
    }

    if (sanitized)
        input.resize(rewritepos - input.begin());

    return sanitized;
}

//-----------------------------------------------------------------------//
template<class string_t, class predicate_t>
bool sanitize_if(const string_t *input, size_t inplen, string_t *output, size_t &outlen, predicate_t predicate)
{
    outlen = 0;
    string_t *outstr = output;
    bool sanitized = false;

    for(;inplen;--inplen)
    {
        if (predicate(*input))
            sanitized = true;
        else
            *outstr++ = *input, ++outlen;

        ++input;
    }

    if (sanitized)
        *outstr = 0;

    return sanitized;
}

} // namespace Sanitization

#endif // __SANITIZERSIMPL_H__

