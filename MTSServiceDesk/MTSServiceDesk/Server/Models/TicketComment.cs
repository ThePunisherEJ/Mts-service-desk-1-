using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class TicketComment
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int TicketId { get; set; }
        public string Comment { get; set; }

        public virtual SupportTicket Ticket { get; set; }
    }
}
