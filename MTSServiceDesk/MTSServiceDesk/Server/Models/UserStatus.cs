using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
