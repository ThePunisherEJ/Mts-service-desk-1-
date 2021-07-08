using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTS.ServiceDesk.Shared.Models
{
   public class AddNew
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DomainName { get; set; }


    }
}
