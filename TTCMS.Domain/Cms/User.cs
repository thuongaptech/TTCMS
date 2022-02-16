using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TTCMS.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Guid Token { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public bool IsUser { get; set; }
        public DateTime? Created { get; set; }
        public User()
        {
            Created = DateTime.Now;
        }
    }
}
