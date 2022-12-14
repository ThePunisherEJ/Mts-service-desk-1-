using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class SupportClient
    {
        public SupportClient()
        {
            SupportTicket = new HashSet<SupportTicket>();
            Systems = new HashSet<Systems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string DomainName { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual ICollection<SupportTicket> SupportTicket { get; set; }
        public virtual ICollection<Systems> Systems { get; set; }
    }
}
