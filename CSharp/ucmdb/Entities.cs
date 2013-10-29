using System;

namespace ucmdb
{
  [UcmdbCiType("cc_organization_unit")]
  struct OrgUnit
  {
    [UcmdbAttribute("id")]
    public string Id { get; set; }
    [UcmdbAttribute("name")]
    public string Name { get; set; }
  }

  [UcmdbCiType("cc_employee")]
  struct Employee
  {
    [UcmdbAttribute("id")]
    public string Id { get; set; }
    [UcmdbAttribute("last_modified_time")]
    public DateTime ModifiedTime { get; set; }
    [UcmdbAttribute("ca_employee_id")]
    public int EmployeeId { get; set; }
    [UcmdbAttribute("ca_login_name")]
    public string LoginName { get; set; }
    [UcmdbAttribute("name")]
    public string Name { get; set; }
    [UcmdbAttribute("ca_first_name")]
    public string FirstName { get; set; }
    [UcmdbAttribute("ca_first_name_en")]
    public string FirstNameEn { get; set; }
    [UcmdbAttribute("ca_last_name")]
    public string LastName { get; set; }
    [UcmdbAttribute("ca_middle_name")]
    public string MiddleName { get; set; }
    [UcmdbAttribute("ca_email")]
    public string Email { get; set; }
    [UcmdbAttribute("ca_location")]
    public string Location { get; set; }
    [UcmdbAttribute("ca_job_description")]
    public string JobDescription { get; set; }
    [UcmdbAttribute("ca_job_status")]
    public string JobStatus { get; set; }
    [UcmdbAttribute("ca_date_of_hire")]
    public string DateOfHire { get; set; }
    [UcmdbAttribute("ca_phone_direct")]
    public string PhoneDirect { get; set; }
    [UcmdbAttribute("ca_phone_internal")]
    public string PhoneInternal { get; set; }
    [UcmdbAttribute("ca_phone_mobile")]
    public string PhoneMobile { get; set; }

    [UcmdbAttribute("ca_outstaff")]
    public bool IsOutstaff { get; set; }
    [UcmdbAttribute("ca_director")]
    public bool IsDirector { get; set; }
    [UcmdbAttribute("ca_exbsgv")]
    public bool IsExBsgv { get; set; }

    //[UcmdbAttribute("")]
    //public string  { get; set; }
    //[UcmdbAttribute("")]
    //public string  { get; set; }
    //[UcmdbAttribute("")]
    //public string  { get; set; }
    //[UcmdbAttribute("")]
    //public string  { get; set; }
    //[UcmdbAttribute("")]
    //public string  { get; set; }
    //[UcmdbAttribute("")]
    //public string  { get; set; }
    //[UcmdbAttribute("")]
    //public string  { get; set; }
  }
}
