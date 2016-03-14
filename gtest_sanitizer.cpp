#include <gtest/gtest.h>

#include <functional>
#include <algorithm>

#include "Sanitizers.h"

namespace test_sanitizers
{
DECLARE_STD_STREAM_OPERATORS_IN_THE_NAMESPACE;

SymbolNotInRangeOrList<unsigned char> httpHeaderSymbols(0x20,0x7F, kBnfSeparators, kBnfSeparatorsSize);
SymbolInRange<unsigned char> httpHeaderValueSymbols(0x20,0xFF);

template<class argchar_t>
bool sanitize_for_http_header_value(std::basic_string<argchar_t> &input)
{
    return sanitize_if(input, std::not1(httpHeaderValueSymbols));
}

bool sanitize_for_http_header_value(const char *input, size_t inplen, char *output, size_t &outlen)
{
    return sanitize_if(input, inplen, output, outlen, std::not1(httpHeaderValueSymbols));
}

bool sanitize_for_http_header_name(std::string &input)
{
    return sanitize_if(input, httpHeaderSymbols);
}

bool sanitize_for_http_header_name(const char *input, size_t inplen, char *output, size_t &outlen)
{
    return sanitize_if(input, inplen, output, outlen, httpHeaderSymbols);
}


TEST(sanitizers, sanitize_n_check_for_unprintable_ascii_stl_strings)
{
    IHttpHeadersSanitizerPtr san = get_service<IHttpHeadersSanitizer>();

    std::string test1_in = "\x19 \b\n\tAb$2&_%^+~ \tMV\r<\nA\r\nas\r\r\n\nd,mz\n/|';\b asdf !ZzAa XXX\a\x80\x88";
    std::string test1_ok = "Ab$2&_%^+~MVAasdmz|'asdf!ZzAaXXX";
    std::string test2_in = "()<>@,;:\\\"/[]?={} \t\x19\a\n\rAaZz\baAzZ\n\n \x80";
    std::string test2_ok = "AaZzaAzZ";
    std::string test3_in = "\n\tAb$2&_%^+~ \tMV\r<\nA\r\nas\r\r\n\nd,mz\n/|';\b asdf !ZzAa \x88XXX\a";
    std::string test3_ok = "Ab$2&_%^+~ MV<Aasd,mz/|'; asdf !ZzAa \x88XXX";
    std::string test4_in = "\a\n\rA@aZz\taAzZ \n\n\x19";
    std::string test4_ok = "A@aZzaAzZ ";

    ASSERT_TRUE(san->headerHasIllegalSymbols(test1_in));
    ASSERT_FALSE(san->headerHasIllegalSymbols(test1_ok));
    ASSERT_TRUE(sanitize_for_http_header_name(test1_in));
    ASSERT_EQ(test1_in, test1_ok);

    ASSERT_TRUE(san->headerHasIllegalSymbols(test2_in));
    ASSERT_FALSE(san->headerHasIllegalSymbols(test2_ok));
    ASSERT_TRUE(sanitize_for_http_header_name(test2_in));
    ASSERT_EQ(test2_in, test2_ok);

    ASSERT_TRUE(san->valueHasIllegalSymbols(test3_in));
    ASSERT_FALSE(san->valueHasIllegalSymbols(test3_ok));
    ASSERT_TRUE(sanitize_for_http_header_value(test3_in));
    ASSERT_EQ(test3_in, test3_ok);

    ASSERT_TRUE(san->valueHasIllegalSymbols(test4_in));
    ASSERT_FALSE(san->valueHasIllegalSymbols(test4_ok));
    ASSERT_TRUE(sanitize_for_http_header_value(test4_in));
    ASSERT_EQ(test4_in, test4_ok);

    ASSERT_FALSE(sanitize_for_http_header_name(test1_ok));
    ASSERT_FALSE(sanitize_for_http_header_name(test2_ok));
    ASSERT_FALSE(sanitize_for_http_header_value(test3_ok));
    ASSERT_FALSE(sanitize_for_http_header_value(test4_ok));
}

TEST(sanitizers, sanitize_n_check_for_unprintable_ascii_char_pointers)
{
    IHttpHeadersSanitizerPtr san = get_service<IHttpHeadersSanitizer>();

    const char *test1_in = " \b\n\tAb$2&_%^+~ \tMV\r<\nA\r\nas\r\r\n\nd,mz\n/|';\b asdf !ZzAa XXX\a\x80";
    const char *test1_ok = "Ab$2&_%^+~MVAasdmz|'asdf!ZzAaXXX";
    const size_t test1_oklen =strlen(test1_ok);
    const size_t test1_inplen=strlen(test1_in);
    const char *test2_in = "()<>@,;:\\\"/[]?={} \t\x19\a\n\rAaZz\baAzZ\n\n \x20";
    const char *test2_ok = "AaZzaAzZ";
    const size_t test2_oklen =strlen(test2_ok);
    const size_t test2_inplen=strlen(test2_in);
    const char *test3_in = "\n\tAb$2&_%^+~ \tMV\r<\nA\r\nas\r\r\n\nd,mz\n/|';\b asdf !ZzAa \x88XXX\a";
    const char *test3_ok = "Ab$2&_%^+~ MV<Aasd,mz/|'; asdf !ZzAa \x88XXX";
    const size_t test3_oklen =strlen(test3_ok);
    const size_t test3_inplen=strlen(test3_in);
    const char *test4_in = "\x88 \a\n\rA@aZz\taAzZ\n\n ";
    const char *test4_ok = "\x88 A@aZzaAzZ ";
    const size_t test4_oklen =strlen(test4_ok);
    const size_t test4_inplen=strlen(test4_in);

    size_t test_outlen;
    char test_out[1024];
    int cmpres;

    ASSERT_TRUE(san->headerHasIllegalSymbols(test1_in, test1_inplen));
    ASSERT_FALSE(san->headerHasIllegalSymbols(test1_ok, test1_oklen));
    // sanitize bad string
    ASSERT_TRUE(sanitize_for_http_header_name(test1_in, test1_inplen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test1_oklen);
    cmpres = strncmp(test_out, test1_ok, test1_oklen);
    ASSERT_TRUE(cmpres == 0);
    // sanitize good string, output should be OK
    ASSERT_FALSE(sanitize_for_http_header_name(test1_ok, test1_oklen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test1_oklen);
    cmpres = strncmp(test_out, test1_ok, test1_oklen);
    ASSERT_TRUE(cmpres == 0);

    ASSERT_TRUE(san->headerHasIllegalSymbols(test2_in, test2_inplen));
    ASSERT_FALSE(san->headerHasIllegalSymbols(test2_ok, test2_oklen));
    // sanitize bad string
    ASSERT_TRUE(sanitize_for_http_header_name(test2_in, test2_inplen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test2_oklen);
    cmpres = strncmp(test_out, test2_ok, test2_oklen);
    ASSERT_TRUE(cmpres == 0);
    // sanitize good string, output should be OK
    ASSERT_FALSE(sanitize_for_http_header_name(test2_ok, test2_oklen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test2_oklen);
    cmpres = strncmp(test_out, test2_ok, test2_oklen);
    ASSERT_TRUE(cmpres == 0);

    ASSERT_TRUE(san->valueHasIllegalSymbols(test3_in, test3_inplen));
    ASSERT_FALSE(san->valueHasIllegalSymbols(test3_ok, test3_oklen));
    // sanitize bad string
    ASSERT_TRUE(sanitize_for_http_header_value(test3_in, test3_inplen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test3_oklen);
    cmpres = strncmp(test_out, test3_ok, test3_oklen);
    ASSERT_TRUE(cmpres == 0);
    // sanitize good string, output should be OK
    ASSERT_FALSE(sanitize_for_http_header_value(test3_ok, test3_oklen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test3_oklen);
    cmpres = strncmp(test_out, test3_ok, test3_oklen);
    ASSERT_TRUE(cmpres == 0);

    ASSERT_TRUE(san->valueHasIllegalSymbols(test4_in, test4_inplen));
    ASSERT_FALSE(san->valueHasIllegalSymbols(test4_ok, test4_oklen));
    // sanitize bad string
    ASSERT_TRUE(sanitize_for_http_header_value(test4_in, test4_inplen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test4_oklen);
    cmpres = strncmp(test_out, test4_ok, test4_oklen);
    ASSERT_TRUE(cmpres == 0);
    // sanitize good string, output should be OK
    ASSERT_FALSE(sanitize_for_http_header_value(test4_ok, test4_oklen, test_out, test_outlen));
    ASSERT_TRUE(test_outlen == test4_oklen);
    cmpres = strncmp(test_out, test4_ok, test4_oklen);
    ASSERT_TRUE(cmpres == 0);
}

} // test_sanitizers
