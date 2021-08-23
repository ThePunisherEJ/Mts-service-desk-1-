using System;
using System.Collections.Generic;

namespace MTS.ServiceDesk.Server.Models
{
    public partial class Status
    {
        public Status()
        {
            SupportClient = new HashSet<SupportClient>();
            Systems = new HashSet<Systems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SupportClient> SupportClient { get; set; }
        public virtual ICollection<Systems> Systems { get; set; }
    }
}
