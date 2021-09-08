using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class TicketController : ControllerBase
    {
        private ApplicationDBContext _applicationDBContext;
        private readonly UserManager<ApplicationUser> _userManager;



        public TicketController(ApplicationDBContext applicationDBContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDBContext = applicationDBContext;
            _userManager = userManager;

        }


        //changeticket status



        [Authorize]
        [HttpGet("Get-Ticket-Statuses")]
        public async Task<IActionResult> GetTicketStatusesAsync()
        {
            try
            {
                List<TicketStatusDetails> searchResult = (from tstat in _applicationDBContext.TicketStatus
                                                    select new TicketStatusDetails
                                                    {
                                                        Id = tstat.Id,
                                                        Name = tstat.Name,
                                                    }).ToList<TicketStatusDetails>();

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [Authorize]
        [HttpGet("Get-All-Tickets-For-Client/{clientId}")]
        public async Task<IActionResult> GetAllTicketsForClientAsync(string clientId)
        {
            try
            {
                List<TicketDetails> searchResult = (from st in _applicationDBContext.SupportTicket.Where(st =>st.ClientId == int.Parse(clientId))
                                                        join cl in _applicationDBContext.SupportClient
                                                            on st.ClientId equals cl.Id
                                                        join sys in _applicationDBContext.Systems
                                                            on st.SystemId equals sys.Id
                                                        join status in _applicationDBContext.TicketStatus
                                                            on st.StatusId equals status.Id 
                                                          select new TicketDetails
                                                          {
                                                              Id = st.Id,
                                                              ClientId = st.ClientId,
                                                              ClientName=cl.Name,
                                                              CreatedBy=st.CreatedBy,
                                                              CreatedByName = "",
                                                              DateCreated = st.DateCreated,
                                                              Description = st.Description,
                                                              StatusId=st.StatusId,
                                                              StatusName=status.Name,
                                                              SystemId = st.SystemId,
                                                              SystemName = sys.Name
                                                          }).ToList<TicketDetails>();

                List<ApplicationUser> users = _userManager.Users.ToList();

                searchResult.ForEach(sr => sr.CreatedByName = users.Find(u => u.Id == sr.CreatedBy).FirstName + " " + users.Find(u => u.Id == sr.CreatedBy).LastName);


                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        
        [Authorize]
        [HttpGet("Get-Open-Tickest-For-Client/{clientId}")]
        public async Task<IActionResult> GetOpenTicketsForClientAsync(string clientId)
        {
            try
            {
                List<TicketDetails> searchResult = (from st in _applicationDBContext.SupportTicket.Where(st => st.ClientId == int.Parse(clientId) && (st.StatusId==1 || st.StatusId==2) )
                                                    join cl in _applicationDBContext.SupportClient
                                                        on st.ClientId equals cl.Id
                                                    join sys in _applicationDBContext.Systems
                                                        on st.SystemId equals sys.Id
                                                    join status in _applicationDBContext.TicketStatus
                                                        on st.StatusId equals status.Id
                                                    select new TicketDetails
                                                    {
                                                        Id = st.Id,
                                                        ClientId = st.ClientId,
                                                        ClientName = cl.Name,
                                                        CreatedBy = st.CreatedBy,
                                                        CreatedByName = "",
                                                        DateCreated = st.DateCreated,
                                                        Description = st.Description,
                                                        StatusId = st.StatusId,
                                                        StatusName = status.Name,
                                                        SystemId = st.SystemId,
                                                        SystemName = sys.Name
                                                    }).ToList<TicketDetails>();

                List<ApplicationUser> users = _userManager.Users.ToList();

                searchResult.ForEach(sr => sr.CreatedByName = users.Find(u => u.Id == sr.CreatedBy).FirstName + " " + users.Find(u => u.Id == sr.CreatedBy).LastName);

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



 
        [Authorize]
        [HttpGet("Get-Ticket/{ticketId}")]
        public async Task<IActionResult> GetTicketByIdAsync(string ticketId)
        {
            try
            {
                TicketDetails searchResult = (from st in _applicationDBContext.SupportTicket.Where(st => st.Id == int.Parse(ticketId))
                                                    join cl in _applicationDBContext.SupportClient
                                                        on st.ClientId equals cl.Id
                                                    join sys in _applicationDBContext.Systems
                                                        on st.SystemId equals sys.Id
                                                    join status in _applicationDBContext.TicketStatus
                                                        on st.StatusId equals status.Id
                                                    select new TicketDetails
                                                    {
                                                        Id = st.Id,
                                                        ClientId = st.ClientId,
                                                        ClientName = cl.Name,
                                                        CreatedBy = st.CreatedBy,
                                                        CreatedByName = "",
                                                        DateCreated = st.DateCreated,
                                                        Description = st.Description,
                                                        StatusId = st.StatusId,
                                                        StatusName = status.Name,
                                                        SystemId = st.SystemId,
                                                        SystemName = sys.Name
                                                    }).FirstOrDefault<TicketDetails>();

                ApplicationUser user = await _userManager.FindByIdAsync(searchResult.CreatedBy);

                searchResult.CreatedByName = user.FirstName + " " + user.LastName;

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize]
        [HttpPost("Close-Ticket")]
        public async Task<IActionResult> CloseTicketAsync(TicketStatusUpdateRequest updTicketReq)
        {
            try
            {
                SupportTicket updTicket = _applicationDBContext.SupportTicket.Where(st => st.Id == updTicketReq.TicketId).FirstOrDefault();

                updTicket.StatusId = 3;
                updTicket.DateClosed = DateTime.Now;
                updTicket.ClosedBy = updTicketReq.UpdatedBy;

                _applicationDBContext.SupportTicket.Update(updTicket);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("Park-Ticket")]
        public async Task<IActionResult> ParkTicketAsync(ParkTicketRequest updTicketReq)
        {
            try
            {
                #region Upate Ticket Status

                SupportTicket updTicket = _applicationDBContext.SupportTicket.Where(st => st.Id == updTicketReq.TicketId).FirstOrDefault();
                updTicket.StatusId = 4;
                _applicationDBContext.SupportTicket.Update(updTicket);
                await _applicationDBContext.SaveChangesAsync();
                #endregion

                #region Add Comment
                TicketComment newComment = new TicketComment();
                newComment.TicketId = updTicketReq.TicketId;
                newComment.Comment = updTicketReq.ParkComment;
                newComment.CreatedBy = updTicketReq.ParkedBy;
                newComment.DateCreated = DateTime.Now;

                _applicationDBContext.TicketComment.Add(newComment);
                await _applicationDBContext.SaveChangesAsync();
                #endregion



                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [Authorize]
        [HttpPost("Assign-Ticket")]
        public async Task<IActionResult> AssignTicketAsync(TicketStatusUpdateRequest updTicketReq)
        {
            try
            {
                #region Upate Ticket Status

                SupportTicket updTicket = _applicationDBContext.SupportTicket.Where(st => st.Id == updTicketReq.TicketId).FirstOrDefault();
                updTicket.StatusId = 2;
                updTicket.AssignedTo = updTicketReq.UpdatedBy;
                _applicationDBContext.SupportTicket.Update(updTicket);
                await _applicationDBContext.SaveChangesAsync();
                #endregion

                #region Add Comment
                TicketComment newComment = new TicketComment();
                newComment.TicketId = updTicketReq.TicketId;
                newComment.Comment = "Ticket assigned";
                newComment.CreatedBy = updTicketReq.UpdatedBy;
                newComment.DateCreated = DateTime.Now;

                _applicationDBContext.TicketComment.Add(newComment);
                await _applicationDBContext.SaveChangesAsync();
                #endregion



                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [Authorize]
        [HttpGet("Get-Comments-For-Ticket/{ticketId}")]
        public async Task<IActionResult> GetCommentsForTicketAsync(string ticketId)
        {
            try
            {
                List<TicketCommentDetails> searchResult = (from comm in _applicationDBContext.TicketComment.Where(comm => comm.TicketId == int.Parse(ticketId)).OrderByDescending(o => o.DateCreated)
                 
                                              select new TicketCommentDetails
                                              {
                                                  Id = comm.Id,
                                                  CreatedBy = comm.CreatedBy,
                                                  CreatedByName = "",
                                                  DateCreated = comm.DateCreated,
                                                  Comment = comm.Comment,
                                                  TicketId = comm.TicketId
                                              }).ToList<TicketCommentDetails>();

                List<ApplicationUser> users = _userManager.Users.ToList();

                searchResult.ForEach(sr => sr.CreatedByName = users.Find(u => u.Id == sr.CreatedBy).FirstName + " " + users.Find(u => u.Id == sr.CreatedBy).LastName);

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize]
        [HttpPost("Create-Ticket")]
        public async Task<IActionResult> CreateTicketAsync(TicketCreateUpdateRequest newTicketReq)
        {
            try
            {
                SupportTicket newTicket = new SupportTicket();

                newTicket.SystemId = newTicketReq.SystemId;
                newTicket.StatusId = newTicketReq.StatusId;
                newTicket.ClientId = newTicketReq.ClientId;
                newTicket.Description = newTicketReq.Description;
                newTicket.CreatedBy = newTicketReq.CreatedBy;
                newTicket.DateCreated = DateTime.Now;

            

                _applicationDBContext.SupportTicket.Add(newTicket);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [Authorize]
        [HttpPost("Update-Ticket")]
        public async Task<IActionResult> UpdateTicketAsync(TicketCreateUpdateRequest updTicketReq)
        {
            try
            {
                SupportTicket updTicket = _applicationDBContext.SupportTicket.Where(st => st.Id == updTicketReq.Id).FirstOrDefault();

                updTicket.SystemId = updTicketReq.SystemId;
                updTicket.StatusId = updTicketReq.StatusId;
                updTicket.ClientId = updTicketReq.ClientId;
                updTicket.Description = updTicketReq.Description;
                updTicket.CreatedBy = updTicketReq.CreatedBy;
                updTicket.DateCreated = DateTime.Now;



                _applicationDBContext.SupportTicket.Update(updTicket);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [Authorize]
        [HttpPost("Create-Comment")]
        public async Task<IActionResult> CreateTicketCommentAsync(TicketCommentCreateUpdateRequest newCommentReq)
        {
            try
            {
                TicketComment newComment = new TicketComment();

                newComment.TicketId = newCommentReq.TicketId;
                newComment.Comment = newCommentReq.Comment;
                newComment.CreatedBy = newCommentReq.CreatedBy;
                newComment.DateCreated = DateTime.Now;



                _applicationDBContext.TicketComment.Add(newComment);
                await _applicationDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
