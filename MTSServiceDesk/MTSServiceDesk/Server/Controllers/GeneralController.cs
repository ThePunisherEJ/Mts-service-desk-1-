using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTS.ServiceDesk.Server.Data;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private ApplicationDBContext _applicationDBContext;

        public GeneralController(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        [HttpGet("GetGeneralStatuses")]
        public async Task<IActionResult> GetGeneralStatuses()
        {
            try
            {
                List<GeneralStatus> searchResult = (from status in _applicationDBContext.Status
                                                                  select new GeneralStatus
                                                                  {
                                                                      Id = status.Id,
                                                                      Name = status.Name
                                                                  }).ToList<GeneralStatus>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetUserStatuses")]
        public async Task<IActionResult> GetUserStatuses()
        {
            try
            {
                List<UserStatus> searchResult = (from status in _applicationDBContext.UserStatus
                                                    select new UserStatus
                                                    {
                                                        Id = status.Id,
                                                        Name = status.Name
                                                    }).ToList<UserStatus>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
