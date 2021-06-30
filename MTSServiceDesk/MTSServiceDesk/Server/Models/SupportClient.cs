using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class SupportClient
    {
        public SupportClient()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string DomainName { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
