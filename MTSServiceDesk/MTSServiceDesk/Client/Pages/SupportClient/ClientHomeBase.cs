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
    public class ClientHomeBase : ComponentBase
    {
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion
        #region PropertiesAndParameters
        [CascadingParameter]
        public IModalService Modal { get; set; }
        
        #endregion
        protected List<SupportClientDetails>  clients;
       
       
        protected override async Task OnInitializedAsync()
        {

            await PopulateList();

            //return base.OnInitializedAsync();
        }
        protected async Task PopulateList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected void NewClientClick()
        {
            navigationManager.NavigateTo("CreateUpdateClient/0");
        }
        protected void UpdateClientClick(int clientID)
        {
            navigationManager.NavigateTo("CreateUpdateClient/" + clientID.ToString());
        }

       
        protected async Task ShowConfirmationModalForEnable(int clientID)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalEnable.confirmationMessageEnableOrDisable), "Are you sure you want to enable?");
            var modalForm = Modal.Show<Shared.ModalEnable>("", parameters, options);
            ModalResult resultEnable = await modalForm.Result;
            modalForm.Close();


            if (resultEnable.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/SupportClient/enable-supportclient/" + clientID.ToString(), "");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var optionsSuccess = new ModalOptions()
                    {
                        HideCloseButton = true,
                        HideHeader = true,


                    };

                    var modalApiResponse = Modal.Show<Shared.ModalSuccess>("",optionsSuccess);
                    ModalResult resultSuccess = await modalApiResponse.Result;

                }
                else
                {
                    //Show error modal
                    var modalApiResponse = Modal.Show<Shared.ModalFailed>("");
                    ModalResult resultFail = await modalApiResponse.Result;
                    //dont navigate away
                }
                await PopulateList();

            }


        }

        protected async Task ShowConfirmationModalForDisable(int clientID)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalEnable.confirmationMessageEnableOrDisable), "Are you sure you want to Disable?");
            var modalForm = Modal.Show<Shared.ModalEnable>("", parameters, options);
            ModalResult resultDisable = await modalForm.Result;
            modalForm.Close();


            if (resultDisable.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/SupportClient/disable-supportclient/" + clientID.ToString(), "");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var optionsSuccess = new ModalOptions()
                    {
                        HideCloseButton = true,
                        HideHeader = true,


                    };

                    var modalApiResponse = Modal.Show<Shared.ModalSuccess>("", optionsSuccess);
                    ModalResult resultSuccess = await modalApiResponse.Result;

                }
                else
                {
                    //Show error modal
                    var modalApiResponse = Modal.Show<Shared.ModalFailed>("");
                    ModalResult resultFail = await modalApiResponse.Result;
                    //dont navigate away
                }
                await PopulateList();

            }

        }

    }
}
