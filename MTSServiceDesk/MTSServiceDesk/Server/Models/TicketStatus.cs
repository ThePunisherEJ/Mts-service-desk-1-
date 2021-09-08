using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class TicketStatus
    {
        public TicketStatus()
        {
            SupportTicket = new HashSet<SupportTicket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SupportTicket> SupportTicket { get; set; }
    }
}
