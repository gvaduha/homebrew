using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LinqToLdap;
using LinqToLdap.Mapping;

namespace ldap
{
  [DirectorySchema("DC=domain,DC=rus,DC=org")]
  public class Employee
  {
    [DistinguishedName]
    public string DistinguishedName { get; set; }
    [DirectoryAttribute("cn", ReadOnly = true)]
    public string CommonName { get; set; }
    //public Collection<string> Members { get; set; }
    [DirectoryAttribute("whenChanged", ReadOnly = true)]
    public DateTime WhenChanged { get; set; }
    [DirectoryAttribute("co")]
    public string Country { get; set; }
    [DirectoryAttribute("company")]
    public string Company { get; set; }
    [DirectoryAttribute("description")]
    public string Description { get; set; }
    [DirectoryAttribute("displayName")]
    public string DisplayName { get; set; }
    [DirectoryAttribute("givenName")]
    public string GivenName { get; set; }
    [DirectoryAttribute("l")]
    public string Location { get; set; }
    [DirectoryAttribute("name")]
    public string Name { get; set; }
    [DirectoryAttribute("sAMAccountName")]
    public string SamAccountName { get; set; }
    [DirectoryAttribute("sn")]
    public string Surname { get; set; }
    [DirectoryAttribute("streetAddress")]
    public string  StreetAddress { get; set; }
    [DirectoryAttribute("telephoneNumber")]
    public string TelephoneNumber { get; set; }
    [DirectoryAttribute("title")]
    public string Title { get; set; }
    [DirectoryAttribute("userPrincipalName")]
    public string UserPrincipalName { get; set; }
    //[DirectoryAttribute("")]
    //public string  { get; set; }
  }

  class Program
  {
    static void Main()
    {
      var config = new LdapConfiguration().AddMapping(new AttributeClassMap<Employee>());
      config.ConfigureFactory("domain.rus.org").ConnectionTimeoutIn(100000).AuthenticateAs(new NetworkCredential("user","pass"));

      using (var context = config.CreateContext())
      {
        var where = context.Query<Employee>()
          .Where(e => e.CommonName == "Name Surname");

        Console.WriteLine(where);

        var x = where.First();
        x.Country = "Eng";
        
        context.Update(x);
      }

    }
  }
}
