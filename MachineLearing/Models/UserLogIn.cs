﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MachineLearing.Models
{
    public class UserLogIn
    {        
            [Required(ErrorMessage = "UserName is required")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
     }
}