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
    public class CreateUpdateSystemsBase : ComponentBase
    {
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion

        #region PropertiesAndParameters
        protected List<SystemDetails> allSystems;
        [Parameter] public string ClientId { get; set; }

        [Parameter] public string UserId { get; set; }
        
        public SystemCreateUpdateRequest systemsRequest = new SystemCreateUpdateRequest();
        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Parameter] public int Id { get; set; }
        private int _selectedClientId;

        public string SysID
        {
            get
            {
                return systemsRequest.Id.ToString();
            }

            set
            {
                systemsRequest.Id = int.Parse(value);
                




            }

        }
        public string SysClientID
        {
            get
            {
                return systemsRequest.ClientId.ToString();
            }

            set
            {
                systemsRequest.ClientId = int.Parse(value);





            }

        }
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        public string SelectedClientId
        {
            get
            {
                return _selectedClientId.ToString();
            }

            set
            {
                _selectedClientId = int.Parse(value);
                systemsRequest.ClientId = _selectedClientId;
                PopulateSystemList();




            }

        }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();
            SelectedClientId = clients.FirstOrDefault().Id.ToString();

            if (Id == 0)
            {
                //systemsRequest = new SystemCreateUpdateRequest();
                systemsRequest.StatusId = 2;
            }
            else
            {
                SystemDetails systemsDetails = await httpClient.GetFromJsonAsync<SystemDetails>("api/systems/Get-System-By-Id/" + Id.ToString());

                systemsRequest.Name = systemsDetails.Name;
                systemsRequest.Id = systemsDetails.Id;
                systemsRequest.StatusId = systemsDetails.StatusId;
                systemsRequest.Description = systemsDetails.Description;
                systemsRequest.ClientId = systemsDetails.ClientId;

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
        protected void FormSave()
        {
            if (Id == 0)
            {

                ShowConfirmationModalForCreateSystem();
            }
            else
            {
                ShowConfirmationModalForUpdateSystem();
            }
        }
        protected async Task ShowConfirmationModalForUpdateSystem()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to edit existing system?");
            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/systems/update-system", systemsRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved();

                    navigationManager.NavigateTo("SystemsHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }


            }
        }

        protected async Task ShowConfirmationModalForCreateSystem()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to create new system?");




            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/systems/create-system", systemsRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved();

                    navigationManager.NavigateTo("SystemsHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }


            }
        }
        protected async Task ModalSaved()
        {


            var optionSuccess = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var modalForm = Modal.Show<Shared.ModalSuccess>("", optionSuccess);
            ModalResult resultSuccess = await modalForm.Result;
        }

        protected async Task ModalFail()
        {
            var modalForm = Modal.Show<Shared.ModalFailed>("");
            ModalResult resultFail = await modalForm.Result;


        }

        protected async Task NewSystemCancel()
        {
            var cancelOptions = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };


            var modalForm = Modal.Show<Shared.ModalCancel>("", cancelOptions);
            ModalResult resultCancel = await modalForm.Result;
            //modalForm.Close();
            if (resultCancel.Cancelled)
            {

            }
            else
            {
                navigationManager.NavigateTo("SystemsHome");
            }
        }
        protected bool SystemIsEnabled
        {
            get
            {
                return systemsRequest.StatusId == 1;
            }
            set
            {
                if (value == true)
                {
                    systemsRequest.StatusId = 1;
                }
                else
                {
                    systemsRequest.StatusId = 2;
                }
            }
        }
    }
}
