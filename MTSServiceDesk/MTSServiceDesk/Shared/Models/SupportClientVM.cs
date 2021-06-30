using System;
using System.Collections.Generic;
using System.Text;

namespace MTS.ServiceDesk.Shared.Models
{
    public class SupportClientDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string DomainName { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }

    }

    public class SupportClientCreateUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string DomainName { get; set; }
        public int StatusId { get; set; }

    }
}
