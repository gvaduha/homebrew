using System;

namespace UcmdbFacade
{
  public class UcmdbFacadeException : ApplicationException
  {
    public UcmdbFacadeException(string message) : base(message) { }
    public UcmdbFacadeException(string message, Exception innerException) : base(message, innerException) { }
  }
}
