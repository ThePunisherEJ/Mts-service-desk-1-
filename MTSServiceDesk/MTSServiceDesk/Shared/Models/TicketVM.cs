using System;
using System.Collections.Generic;
using System.Text;

namespace MTS.ServiceDesk.Shared.Models
{
    public class TicketStatusDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
     
    }


    public class TicketDetails
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public int SystemId { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string ClientName { get; set; }
        public string SystemName { get; set; }
        public string CreatedByName { get; set; }
    }

    public class TicketCommentDetails
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int TicketId { get; set; }
        public string Comment { get; set; }
        public string CreatedByName { get; set; }

    }


    public class TicketCreateUpdateRequest
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public int SystemId { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }

    }

    public class TicketCommentCreateUpdateRequest
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int TicketId { get; set; }
        public string Comment { get; set; }
        public string CreatedByName { get; set; }

    }

    public class TicketStatusUpdateRequest
    {
        public int TicketId { get; set; }
        public string UpdatedBy { get; set; }

    }

    public class ParkTicketRequest
    {
        public int TicketId { get; set; }
        public string ParkedBy { get; set; }
        public string ParkComment { get; set; }

    }

}
