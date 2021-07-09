using Microsoft.AspNetCore.Components;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Pages.SupportClient
{
    public class ClientHomeBase : ComponentBase
    {
        protected List<SupportClientDetails>  clients;
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            clients = await httpClient.GetFromJsonAsync <List <SupportClientDetails>>("api/SupportClient/Get-All");


            //return base.OnInitializedAsync();
        }
        protected void NewClientClick()
        {
            navigationManager.NavigateTo("newclient");
        }
    }
}
