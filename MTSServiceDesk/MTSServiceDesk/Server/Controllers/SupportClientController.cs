using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class SupportClientController : ControllerBase
    {
        private ApplicationDBContext _applicationDBContext;

        public SupportClientController(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        [Authorize]
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<SupportClientDetails> searchResult = (from client in _applicationDBContext.SupportClient
                                    join status in _applicationDBContext.Status
                                    on client.StatusId equals status.Id
                                    select new SupportClientDetails
                                    {
                                        Id = client.Id,
                                        Name = client.Name,
                                        Logo = client.Logo,
                                        DomainName = client.DomainName,
                                        StatusId = client.StatusId,
                                        StatusDescription = status.Name
                                    }).ToList<SupportClientDetails>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Get-ById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            try
            {
                int clientId = int.Parse(Id);

                SupportClientDetails searchResult = (from client in _applicationDBContext.SupportClient.Where(c => c.Id == clientId)
                                    join status in _applicationDBContext.Status
                                    on client.StatusId equals status.Id
                                    select new SupportClientDetails
                                    {
                                        Id = client.Id,
                                        Name = client.Name,
                                        Logo = client.Logo,
                                        DomainName = client.DomainName,
                                        StatusId = client.StatusId,
                                        StatusDescription = status.Name
                                    }).FirstOrDefault<SupportClientDetails>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [Authorize]
        [HttpPost("create-supportclient")]
        public async Task<IActionResult> CreateSupportClientAsync(SupportClientCreateUpdateRequest newSupportClient)
        {
            try
            {
                SupportClient supportClient = new SupportClient();

                supportClient.DomainName = newSupportClient.DomainName;
                supportClient.Logo = newSupportClient.Logo;
                supportClient.Name = newSupportClient.Name;
                supportClient.StatusId = newSupportClient.StatusId;

                _applicationDBContext.SupportClient.Add(supportClient);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [Authorize]
        [HttpPost("update-supportclient")]
        public async Task<IActionResult> UpdateSupportClientAsync(SupportClientCreateUpdateRequest updSupportClient)
        {
            try
            {
                SupportClient supportClient = _applicationDBContext.SupportClient.Where(sc => sc.Id == updSupportClient.Id).FirstOrDefault();

                supportClient.DomainName = updSupportClient.DomainName;
                supportClient.Logo = updSupportClient.Logo;
                supportClient.Name = updSupportClient.Name;
                supportClient.StatusId = updSupportClient.StatusId;

                _applicationDBContext.SupportClient.Update(supportClient);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("disable-supportclient/{Id}")]
        public async Task<IActionResult> DisableSupportClientAsync(string Id)
        {
            try
            {
                await ChangeSupportClientStatus(Id, 2);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("enable-supportclient/{Id}")]
        public async Task<IActionResult> EnableSupportClientAsync(string Id)
        {
            try
            {
                await ChangeSupportClientStatus(Id, 1);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        private async Task ChangeSupportClientStatus(string Id, int newStatus)
        {

                int supportClientId = int.Parse(Id);
                SupportClient supportClient = _applicationDBContext.SupportClient.Where(sc => sc.Id == supportClientId).FirstOrDefault();
                supportClient.StatusId = newStatus;

                _applicationDBContext.SupportClient.Update(supportClient);
                await _applicationDBContext.SaveChangesAsync();
        }
    }
}
