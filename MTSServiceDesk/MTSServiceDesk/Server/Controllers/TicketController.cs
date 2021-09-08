using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTS.ServiceDesk.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private ApplicationDBContext _applicationDBContext;


        public TicketController(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }


        //Get all tickets for Client
        //Get open tickets for client
        //Get Ticket by ID
        //Get Comments for Ticket

        //create Ticket
        //update ticket
        //changeticket status

        // add comment


    }
}
