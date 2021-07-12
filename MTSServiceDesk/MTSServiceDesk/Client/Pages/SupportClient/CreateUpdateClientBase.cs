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

namespace MTS.ServiceDesk.Client.Pages.SupportClient
{
    public class CreateUpdateClientBase : ComponentBase
    {
        #region DI
        [Inject] protected NavigationManager navigationManager { get; set; }
        [Inject] protected HttpClient httpClient { get; set; }
        #endregion
        #region Properties And Parameters
        [Parameter] public int Id { get; set; }
        protected SupportClientCreateUpdateRequest ClientRequest = new SupportClientCreateUpdateRequest();
        [CascadingParameter] public IModalService Modal { get; set; }
        #endregion


        protected void Create()
        {
            Console.WriteLine(ClientRequest.Name);
        }

        protected override async Task OnInitializedAsync()
        {
            if(Id == 0)
            {
                ClientRequest = new SupportClientCreateUpdateRequest();
                    }
            else
            {
                SupportClientDetails clientDetails = await httpClient.GetFromJsonAsync<SupportClientDetails>("api/supportclient/Get-ById/" + Id.ToString());
                ClientRequest.DomainName = clientDetails.DomainName;
                ClientRequest.Id = clientDetails.Id;
                ClientRequest.Logo = clientDetails.Logo;
                ClientRequest.Name = clientDetails.Name;
                ClientRequest.StatusId = clientDetails.StatusId;
                
            }
           
        }


        protected void ShowConfirmationModal()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true, 
                HideHeader = true,


            };

            Modal.Show<Shared.ModalConfirmation>("", options);

        }
        protected void NewClientCancel()
        {
            navigationManager.NavigateTo("SupportClientsHome");
        }
    }
    
}
