using System;
using System.Runtime.Serialization;
using UcmdbFacade;

namespace UcmdbServiceFacade
{
  [DataContract]
  [UcmdbCiType("cc_organization_unit")]
  public struct OrgUnit
  {
    [UcmdbAttribute("id")] [DataMember]
    public string Id { get; set; }
    [UcmdbAttribute("name")][DataMember]
    public string Name { get; set; }
  }

  [DataContract]
  [UcmdbCiType("cc_employee")]
  public struct Employee
  {
    [UcmdbAttribute("id")] [DataMember]
    public string Id { get; set; }
    [UcmdbAttribute("last_modified_time")] [DataMember]
    public DateTime ModifiedTime { get; set; }
    [UcmdbAttribute("ca_employee_id")] [DataMember]
    public int EmployeeId { get; set; }
    [UcmdbAttribute("ca_login_name")] [DataMember]
    public string LoginName { get; set; }
    [UcmdbAttribute("name")] [DataMember]
    public string Name { get; set; }
    [UcmdbAttribute("ca_first_name")] [DataMember]
    public string FirstName { get; set; }
    [UcmdbAttribute("ca_first_name_en")] [DataMember]
    public string FirstNameEn { get; set; }
    [UcmdbAttribute("ca_last_name")] [DataMember]
    public string LastName { get; set; }
    [UcmdbAttribute("ca_middle_name")] [DataMember]
    public string MiddleName { get; set; }
    [UcmdbAttribute("ca_email")] [DataMember]
    public string Email { get; set; }
    [UcmdbAttribute("ca_location")] [DataMember]
    public string Location { get; set; }
    [UcmdbAttribute("ca_job_description")] [DataMember]
    public string JobDescription { get; set; }
    [UcmdbAttribute("ca_job_status")] [DataMember]
    public string JobStatus { get; set; }
    [UcmdbAttribute("ca_date_of_hire")] [DataMember]
    public string DateOfHire { get; set; }
    [UcmdbAttribute("ca_phone_direct")] [DataMember]
    public string PhoneDirect { get; set; }
    [UcmdbAttribute("ca_phone_internal")] [DataMember]
    public string PhoneInternal { get; set; }
    [UcmdbAttribute("ca_phone_mobile")] [DataMember]
    public string PhoneMobile { get; set; }

    [UcmdbAttribute("ca_outstaff")] [DataMember]
    public bool IsOutstaff { get; set; }
    [UcmdbAttribute("ca_director")] [DataMember]
    public bool IsDirector { get; set; }
    [UcmdbAttribute("ca_exbsgv")] [DataMember]
    public bool IsExBsgv { get; set; }

    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
    //[UcmdbAttribute("")] [DataMember]
    //public string  { get; set; }
  }
}
