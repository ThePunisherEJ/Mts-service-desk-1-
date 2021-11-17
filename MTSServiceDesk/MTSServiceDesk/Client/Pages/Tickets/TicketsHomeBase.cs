using Blazored.Modal;
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
    public class TicketsHomeBase : ComponentBase
    {
        #region Properties And Parameters
        protected List<TicketDetails> allTickets;
        protected List<TicketStatusDetails> TicketStatus;
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        private int _selectedClientId;
        private int _selectedStatusId;
        
        public string SelectedStatusID
        {
            get
            {
                return _selectedStatusId.ToString();
            }
            set
            {
                _selectedStatusId = int.Parse(value);
                //PopulateTstatusList();
                PopulateTicketList();
            }
        }
        public  string SelectedClientId
        {
            get

            {
                return _selectedClientId.ToString();
            }

            set
            {
                _selectedClientId = int.Parse(value);
                PopulateTicketList();

            }

        }
        #endregion
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion



        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();
            await PopulateTstatusList();
            _selectedClientId = clients.FirstOrDefault().Id;
            _selectedStatusId = 0;
           await PopulateTicketList();
            //SelectedStatusID = allTickets.FirstOrDefault().StatusId.ToString();
            //allTickets = await httpClient.GetFromJsonAsync<List<TicketDetails>>("api/ticket/Get-Tickets-For-Client/" + SelectedClientId);

            //return base.OnInitializedAsync();
        }
      
        protected async Task PopulateTicketList()
        {
            allTickets = await httpClient.GetFromJsonAsync<List<TicketDetails>>("api/ticket/Get-Tickets-For-Client/" + SelectedClientId + "/"  + SelectedStatusID);
            //PopulateTstatusList();
            StateHasChanged();
        }
        protected async Task PopulateTstatusList()
        { TicketStatusDetails allStatus = new TicketStatusDetails();
            allStatus.Id = 0;
            allStatus.Name = "All";
            
            TicketStatus = await httpClient.GetFromJsonAsync<List<TicketStatusDetails>>("api/ticket/Get-Ticket-Statuses");
            TicketStatus.Add(allStatus);
            

        }
        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected void NewTicketClick()
        {
            navigationManager.NavigateTo("/CreateUpdateTicket" + "/" + SelectedClientId + "/0");
        }
        protected void UpdateTicketClick(int clientId, int ticketId)
        {
            navigationManager.NavigateTo("/CreateUpdateTicket" + "/" + clientId + "/"+ ticketId);
        }
        protected void ViewTicketClick(int clientId, int ticketId)
        {
            navigationManager.NavigateTo("/TicketView" + "/" + SelectedClientId + "/" + ticketId);
        }


    }
}
