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

namespace MTS.ServiceDesk.Client.Pages.UserMaintenance
{
    public class UserMaintenanceHomeBase : ComponentBase
    {
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion
        #region PropertiesAndParameters
        [CascadingParameter]
        public IModalService Modal { get; set; }
        //protected List<UserDetails> Users ;
        private int _selectedClientId;
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        #endregion

        protected List <UserDetails> allUsers;
        public string selectedClientId
        {
            get
            {
                return _selectedClientId.ToString();
            }

            set
            {
                _selectedClientId = int.Parse(value);
                PopulateUserList();

                    

                
            }

        }

        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();
           
            selectedClientId = clients.FirstOrDefault().Id.ToString();
           
            //return base.OnInitializedAsync();
        }
        protected void NewUserClick()
        {
            navigationManager.NavigateTo("CreateUpdateUser/" + selectedClientId  +"/"  + System.Guid.Empty.ToString());
        }
        protected void UpdateUserClick(string userID)
        {
            navigationManager.NavigateTo("CreateUpdateUser/" + selectedClientId + "/" + userID);
        }
        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected async Task PopulateUserList()
        {
            allUsers = await httpClient.GetFromJsonAsync<List<UserDetails>>("api/auth/get-users-by-clientid/" + selectedClientId);
            StateHasChanged();
        }

        protected async Task ShowConfirmationModalForEnable(string userID)
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


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
                var response = await httpClient.PostAsJsonAsync("api/auth/enable-user/" + userID.ToString(), "");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //var optionsSuccess = new ModalOptions()
                    //{
                    //    HideCloseButton = true,
                    //    HideHeader = true,


                    //};

                    //var modalApiResponse = Modal.Show<Shared.ModalSuccess>("", optionsSuccess);
                    //ModalResult resultSuccess = await modalApiResponse.Result;
                    await ModalSaved("User Enabled Successfully");

                }
                else
                {
                    //Show error modal
                    var modalApiResponse = Modal.Show<Shared.ModalFailed>("");
                    ModalResult resultFail = await modalApiResponse.Result;
                    //dont navigate away
                }
                await PopulateUserList();

            }


        }

        protected async Task ShowConfirmationModalForDisable(string userID)
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


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
                var response = await httpClient.PostAsJsonAsync("api/auth/disable-user/" + userID.ToString(), "");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //var optionsSuccess = new ModalOptions()
                    //{
                    //    HideCloseButton = true,
                    //    HideHeader = true,


                    //};

                    //var modalApiResponse = Modal.Show<Shared.ModalSuccess>("", optionsSuccess);
                    //ModalResult resultSuccess = await modalApiResponse.Result;
                    await ModalSaved("User Disabled Successfully");

                }
                else
                {
                    //Show error modal
                    var modalApiResponse = Modal.Show<Shared.ModalFailed>("");
                    ModalResult resultFail = await modalApiResponse.Result;
                    //dont navigate away
                }
                await PopulateUserList();

            }


        }

        protected async Task ModalSaved(string SuccessMessage)
        {


            var optionSuccess = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalSuccess.SuccessMessage), SuccessMessage);

            var ModalForm = Modal.Show<Shared.ModalSuccess>("", parameters, optionSuccess);
        }

    }
}
