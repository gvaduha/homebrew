using System;

namespace ucmdb.UnitTests
{
  [UcmdbCiType("test_type")]
  class TestType
  {
    [UcmdbAttribute("str")]
    public string Str { get; set; }
    [UcmdbAttribute("date")]
    public DateTime? Date { get; set; }
    [UcmdbAttribute("int")]
    public int? Int;
    [UcmdbAttribute("bool")]
    public bool? Bool;
  }
}
