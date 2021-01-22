using System;
using System.Collections.Generic;
using System.Text;

namespace PFMA.Models
{
    public class LoginToken
    {
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string tokenType { get; set; }
        public int validationStart { get; set; }
        public int validationEnd { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
    }
}
