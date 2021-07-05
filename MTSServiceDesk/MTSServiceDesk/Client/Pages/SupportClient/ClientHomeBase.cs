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
        protected SupportClientDetails[] clients;
        [Inject] protected HttpClient httpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            clients = await httpClient.GetFromJsonAsync<SupportClientDetails[]>("api/SupportClient/Get-All");

            //return base.OnInitializedAsync();
        }
    }
}
