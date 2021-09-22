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
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        private int _selectedClientId;
        #endregion
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion



        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();
            SelectedClientId = clients.FirstOrDefault().Id.ToString();
            allTickets = await httpClient.GetFromJsonAsync<List<TicketDetails>>("api/ticket/Get-Tickets-For-Client/1/0");

            //return base.OnInitializedAsync();
        }
        public string SelectedClientId
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
        protected async Task PopulateTicketList()
        {
            allTickets = await httpClient.GetFromJsonAsync<List<TicketDetails>>("api/ticket/Get-Tickets-For-Client/" + SelectedClientId);
            StateHasChanged();
        }
        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected void NewTicketClick()
        {
            navigationManager.NavigateTo("/CreateUpdateTicket" /*+ "/"+ SelectedClientId*/ + "/0");
        }
    }
}
