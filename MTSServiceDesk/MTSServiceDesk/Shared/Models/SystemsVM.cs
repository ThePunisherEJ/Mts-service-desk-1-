using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool SysStatusBool
        {
            get
            {
                return StatusId == 1;
            }
            set
            {
                if (value == true)
                {
                    StatusId = 1;
                }
                else
                {
                    StatusId = 2;
                }
            }
        }
    }

    public class SystemCreateUpdateRequest
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }

    }
   

}
