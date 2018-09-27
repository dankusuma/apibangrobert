using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.Services.Models
{

    public class UserClass
    {

        [Key]
        public string username { get; set; }
        public string password { get; set; }
        public string nama { get; set; }
        public string alamat { get; set; }
        public string telepon { get; set; }
    }
}
