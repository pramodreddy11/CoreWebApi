using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Models
{
    public class UserModel
    {
        public int CustId { get; set; }
        public string CustName { get; set; }
        public int? CustAge { get; set; }
        public string CustAddress { get; set; }
        public string CustRole { get; set; }
        public string CustPhoneNo { get; set; }
        public string Email { get; set; }
        public string AuthCode { get; set; }
    }
}
