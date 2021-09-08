using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class SupportTicket
    {
        public SupportTicket()
        {
            TicketComment = new HashSet<TicketComment>();
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public int SystemId { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public DateTime? DateClosed { get; set; }
        public string ClosedBy { get; set; }
        public string AssignedTo { get; set; }

        public virtual SupportClient Client { get; set; }
        public virtual TicketStatus Status { get; set; }
        public virtual Systems System { get; set; }
        public virtual ICollection<TicketComment> TicketComment { get; set; }
    }
}
