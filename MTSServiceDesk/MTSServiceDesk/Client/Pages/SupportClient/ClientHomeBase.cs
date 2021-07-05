//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MTS.ServiceDesk.Client.Pages.SupportClient
//{
//    public class ClientHomeBase : ComponentBase
//    {
//        private SupportClientDetails[] clients;

//        protected override async Task OnInitializedAsync()
//        {
//            clients = await httpClient.GetFromJsonAsync<SupportClientDetails[]>("api/SupportClient/Get-All");

//            //return base.OnInitializedAsync();
//        }
//    }
//}
