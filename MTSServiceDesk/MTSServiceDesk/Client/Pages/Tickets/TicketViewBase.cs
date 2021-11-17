using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Pages.Tickets
{
    public class TicketViewBase : ComponentBase
    {
        #region Properties And Parameters
        public TicketCreateUpdateRequest TicketRequest = new TicketCreateUpdateRequest();
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        protected List<SystemDetails> Systems = new List<SystemDetails>();
        protected UserCreateUpdateRequest UserRequest = new UserCreateUpdateRequest();
        protected List<TicketCommentDetails> comments;
        private int _selectedClientId;
        private int _selectedSystemId;

        [CascadingParameter]

        public IModalService Modal { get; set; }
        [Parameter] public int ClientId { get; set; }
        [Parameter] public string SystemId { get; set; }
        public string SelectedClientId
        {
            get
            {
                return _selectedClientId.ToString();
            }

            set
            {
                _selectedClientId = int.Parse(value);
                TicketRequest.ClientId = _selectedClientId;
               


            }

        }
        public string SelectedSystemID
        {
            get
            {
                return _selectedSystemId.ToString();
            }

            set
            {
                _selectedSystemId = int.Parse(value);
                TicketRequest.SystemId = _selectedSystemId;
                


            }

        }
        public string ClientName { get; set; }
        public string SystemName { get; set; }

        [Parameter] public int TicketId { get; set; }
        #endregion
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion
        protected override async Task OnInitializedAsync()
        {
            await PopulateCommentTableList();
            await PopulateClientList();
            _selectedClientId = (ClientId);
            //await PopulateSystemList();



          
                CurrentUser user = await httpClient.GetFromJsonAsync<CurrentUser>("api/auth/currentuserinfo");
                SelectedClientId = ClientId.ToString();
                //SelectedSystemID = Systems.FirstOrDefault().Id.ToString();
                //SelectedSystemID = SystemId;
                TicketRequest.StatusId = 1;
                TicketRequest.DateCreated = DateTime.Now;
                TicketRequest.CreatedBy = user.UserId;
           

                TicketDetails ticketDetails = await httpClient.GetFromJsonAsync<TicketDetails>("api/ticket/Get-Ticket/" + TicketId.ToString());
                //TicketCommentDetails commentDetails = await httpClient.GetFromJsonAsync<TicketCommentDetails>("api/ticket/Get-Comments-For-Ticket/" + Id.ToString());
                TicketRequest.ClientId = ticketDetails.ClientId;
                TicketRequest.Id = ticketDetails.Id;
                TicketRequest.DateCreated = ticketDetails.DateCreated;
                TicketRequest.CreatedBy = ticketDetails.CreatedBy;
                TicketRequest.Description = ticketDetails.Description;
                TicketRequest.StatusId = ticketDetails.StatusId;
                TicketRequest.SystemId = ticketDetails.SystemId;
            ClientName = ticketDetails.ClientName;
            SystemName = ticketDetails.SystemName;
            


        }
        //public string TicketID
        //{
        //    get
        //    {
        //        return TicketRequest.Id.ToString();
        //    }

        //    set
        //    {
        //        TicketRequest.Id = int.Parse(value);

        //    }
        //}
        protected async Task PopulateCommentTableList()
        {
            comments = await httpClient.GetFromJsonAsync<List<TicketCommentDetails>>("api/Ticket/Get-Comments-For-Ticket/" + TicketId.ToString());

        }
        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected async Task PopulateSystemList()
        {
            Systems = await httpClient.GetFromJsonAsync<List<SystemDetails>>("api/systems/Get-All-Systems-For-Client/" + _selectedClientId.ToString());
            StateHasChanged();
            SelectedSystemID = Systems.FirstOrDefault().Id.ToString();

        }
        protected void NewCommentClick()
        {
            navigationManager.NavigateTo("CreateUpdateComment/" + ClientId.ToString() + "/"+ TicketId + "/0");
        }
        protected void UpdateCommentClick(int commentId)
        {
            navigationManager.NavigateTo("CreateUpdateComment/" + ClientId.ToString() + "/"  + TicketId + "/" + commentId.ToString());
        }
    }
}
