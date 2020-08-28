using System;
using System.Collections.Generic;

namespace TestWebApi.Models
{
    public partial class Customer
    {
        public int CustId { get; set; }
        public string CustName { get; set; }
        public int? CustAge { get; set; }
        public string CustAddress { get; set; }
        public string CustRole { get; set; }
        public string CustPhoneNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
