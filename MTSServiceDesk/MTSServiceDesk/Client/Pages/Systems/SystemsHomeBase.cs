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

namespace MTS.ServiceDesk.Client.Pages.Systems
{
    public class SystemsHomeBase : ComponentBase
    {
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion
        #region PropertiesAndParameters
        protected List<SystemDetails> allSystems;
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        private int _selectedClientId;
        [CascadingParameter]
        public IModalService Modal { get; set; }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();
            
            SelectedClientId = clients.FirstOrDefault().Id.ToString();
            allSystems = await httpClient.GetFromJsonAsync<List<SystemDetails>>("api/systems/Get-All-Systems-For-Client/1");

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
                PopulateSystemList();




            }

        }

        protected async Task PopulateSystemList()
        {
            allSystems = await httpClient.GetFromJsonAsync<List<SystemDetails>>("api/systems/Get-All-Systems-For-Client/" + SelectedClientId);
            StateHasChanged();
        }

        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected void NewSystemsClick()
        {
            navigationManager.NavigateTo("/CreateUpdateSystems/" + SelectedClientId + "/0");
        }

        protected void UpdateSystemClick(string systemID)
        {
            navigationManager.NavigateTo("CreateUpdateSystems/" + SelectedClientId + "/" + systemID);
        }

        protected async Task ShowConfirmationModalForEnable(string systemID)
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
                var response = await httpClient.PostAsJsonAsync("api/systems/enable-system/" + systemID.ToString(), "");
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
                await PopulateSystemList();

            }


        }

        protected async Task ShowConfirmationModalForDisable(string systemID)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalEnable.confirmationMessageEnableOrDisable), "Are you sure you want to disable?");
            var modalForm = Modal.Show<Shared.ModalEnable>("", parameters, options);
            ModalResult resultEnable = await modalForm.Result;
            modalForm.Close();


            if (resultEnable.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/systems/disable-system/" + systemID.ToString(), "");
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
                await PopulateSystemList();

            }


        }
    }
}
