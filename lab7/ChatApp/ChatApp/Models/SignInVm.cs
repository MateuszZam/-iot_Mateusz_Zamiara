using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class SignInVm
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(length:3, ErrorMessage = "Username can't be shorter than 3")]
        public string UserName { get; set; }

    }
}
