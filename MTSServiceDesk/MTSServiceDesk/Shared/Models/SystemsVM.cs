using System;
using System.Collections.Generic;
using System.Text;

namespace MTS.ServiceDesk.Shared.Models
{
    public class SystemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }
    }

    public class SystemCreateUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }

    }

}
