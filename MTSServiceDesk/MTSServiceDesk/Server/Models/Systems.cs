using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class Systems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }

        public virtual SupportClient Client { get; set; }
        public virtual Status Status { get; set; }
    }
}
