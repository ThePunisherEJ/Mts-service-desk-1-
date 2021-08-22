using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTS.ServiceDesk.Server.Data;
using MTS.ServiceDesk.Server.Models;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemsController : ControllerBase
    {
        private ApplicationDBContext _applicationDBContext;

        public SystemsController(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        [Authorize]
        [HttpGet("Get-All-Systems-For-Client/{clientId}")]
        public async Task<IActionResult> GetAllSystemsForCient(string clientId)
        {
            try
            {
                List<SystemDetails> searchResult = (from sys in _applicationDBContext.Systems.Where(s=>s.ClientId == int.Parse(clientId))
                                                                  select new SystemDetails
                                                                  {
                                                                      Id = sys.Id,
                                                                      Name = sys.Name,
                                                                      ClientId = sys.ClientId,
                                                                      StatusId = sys.StatusId,
                                                                      Description = sys.Description,
                                                                      
                                                                  }).ToList<SystemDetails>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Get-Active-Systems-For-Client/{clientId}")]
        public async Task<IActionResult> GetActiveSystemsForCient(string clientId)
        {
            try
            {
                List<SystemDetails> searchResult = (from sys in _applicationDBContext.Systems.Where(s => s.ClientId == int.Parse(clientId) && s.StatusId == 1)
                                                    select new SystemDetails
                                                    {
                                                        Id = sys.Id,
                                                        Name = sys.Name,
                                                        ClientId = sys.ClientId,
                                                        StatusId = sys.StatusId,
                                                        Description = sys.Description,

                                                    }).ToList<SystemDetails>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Get-System-By-Id/{systemId}")]
        public async Task<IActionResult> GetSystemById(string systemId)
        {
            try
            {
                SystemDetails searchResult = (from sys in _applicationDBContext.Systems.Where(s => s.Id == int.Parse(systemId))
                                                    select new SystemDetails
                                                    {
                                                        Id = sys.Id,
                                                        Name = sys.Name,
                                                        ClientId = sys.ClientId,
                                                        StatusId = sys.StatusId,
                                                        Description = sys.Description,

                                                    }).FirstOrDefault();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize]
        [HttpPost("create-system")]
        public async Task<IActionResult> CreateSystemAsync(SystemCreateUpdateRequest newSystemReq)
        {
            try
            {
                Systems newSys = new Systems();

                newSys.Name = newSystemReq.Name;
                newSys.StatusId = newSystemReq.StatusId;
                newSys.ClientId = newSystemReq.ClientId;
                newSys.Description = newSystemReq.Description;

                _applicationDBContext.Systems.Add(newSys);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [Authorize]
        [HttpPost("update-system")]
        public async Task<IActionResult> UpdateSystemAsync(SystemCreateUpdateRequest updSystemReq)
        {
            try
            {
                Systems updSys = _applicationDBContext.Systems.Where(sys => sys.Id == updSystemReq.Id).FirstOrDefault();


                updSys.Name = updSystemReq.Name;
                updSys.StatusId = updSystemReq.StatusId;
                updSys.ClientId = updSystemReq.ClientId;
                updSys.Description = updSystemReq.Description;

                _applicationDBContext.Systems.Update(updSys);

                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [Authorize]
        [HttpPost("disable-system/{Id}")]
        public async Task<IActionResult> DisableSystemAsync(string Id)
        {
            try
            {
                await ChangeSystemStatus(Id, 2);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("enable-system/{Id}")]
        public async Task<IActionResult> EnableSystemAsync(string Id)
        {
            try
            {
                await ChangeSystemStatus(Id, 1);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        private async Task ChangeSystemStatus(string Id, int newStatus)
        {

            int systemId = int.Parse(Id);
            Systems targetSys = _applicationDBContext.Systems.Where(s => s.Id == systemId).FirstOrDefault();
            targetSys.StatusId = newStatus;

            _applicationDBContext.Systems.Update(targetSys);
            await _applicationDBContext.SaveChangesAsync();
        }


    }
}
