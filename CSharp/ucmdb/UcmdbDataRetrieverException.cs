using System;

namespace ucmdb
{
  class UcmdbDataRetrieverException : ApplicationException
  {
    public UcmdbDataRetrieverException(string message, Exception innerException) : base(message, innerException) {}
  }
}
