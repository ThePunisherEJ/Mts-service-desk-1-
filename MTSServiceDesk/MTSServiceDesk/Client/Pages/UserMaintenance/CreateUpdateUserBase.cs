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
    public class CreateUpdateUserBase : ComponentBase
    {
        #region DI
        [Inject] protected NavigationManager navigationManager { get; set; }
        [Inject] protected HttpClient httpClient { get; set; }
        #endregion
        #region Properties And Parameters
        //protected UserDetails[] allUsers;
        [Parameter] public string UserId { get; set; }
        [Parameter] public string ClientId { get; set; }
        protected UserCreateUpdateRequest UserRequest = new UserCreateUpdateRequest();
        [CascadingParameter] public IModalService Modal { get; set; }
        //protected List<UserDetails> Users;
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
     

        public string selectedClientId
        {
            get 
            {
                return UserRequest.ClientId.ToString();
            }

            set 
            {
                UserRequest.ClientId = int.Parse(value);
            }
        
        }
        public string selectedUserType
        {
            get
            {
                return ((int)UserRequest.TypeOfUser).ToString();

            }

            set
            {

                //int uType = (int)UserType.;
                UserRequest.TypeOfUser = (UserType)int.Parse(value);
            }

        }
        //public Task OnValueChanged(string value)
        //{
            
        //    UserRequest.TypeOfUser = (UserType)int.Parse(value);
        //    return Task.CompletedTask;
        //}
        #endregion


        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();

            if (Guid.Parse(UserId) ==  Guid.Empty)
            {
                UserRequest = new UserCreateUpdateRequest();
                UserRequest.UserStatus = 2;
                selectedClientId = ClientId;
                UserRequest.TypeOfUser = UserType.User;
                
            }
            else
            {
                UserDetails usersDetails = await httpClient.GetFromJsonAsync<UserDetails>("api/auth/get-user-by-id/" + UserId.ToString());

                
                UserRequest.UserId = usersDetails.UserId;
                UserRequest.Email = usersDetails.Email;
                UserRequest.FirstName = usersDetails.FirstName;
                UserRequest.LastName = usersDetails.LastName;
                UserRequest.ClientId = usersDetails.ClientId;
                UserRequest.TypeOfUser = usersDetails.TypeOfUser; //radio button group
                UserRequest.UserStatus = usersDetails.UserStatus;
                selectedClientId = usersDetails.ClientId.ToString();
            }
            
        }

        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }

        protected void FormSave()
        {
            if (UserId == Guid.Empty.ToString())
            {
                
                ShowConfirmationModalForCreateUser();
            }
            else
            {
                ShowConfirmationModalForUpdateUser();
            }
        }

        protected async Task ShowConfirmationModalForCreateUser()
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to create new user?");




            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/auth/create-user", UserRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved("User : " + UserRequest.FirstName + " " + UserRequest.LastName + " Succesfully Created");

                    navigationManager.NavigateTo("UserMaintenanceHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }


            }
        }

        protected async Task ShowConfirmationModalForUpdateUser()
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to update current user?");




            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/auth/update-user", UserRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved("User : " + UserRequest.FirstName + " " + UserRequest.LastName + " Succesfully Updated");

                    navigationManager.NavigateTo("UserMaintenanceHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }


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

        protected async Task ModalFail()
        {

            var modalForm = Modal.Show<Shared.ModalFailed>("");
            ModalResult resultFail = await modalForm.Result;


        }

        protected async Task NewUserCancel()
        {
            var cancelOptions = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };


            var modalForm = Modal.Show<Shared.ModalCancel>("", cancelOptions);
            ModalResult resultCancel = await modalForm.Result;
            //modalForm.Close();
            if (resultCancel.Cancelled)
            {

            }
            else
            {
                navigationManager.NavigateTo("UserMaintenanceHome");
            }
        }

        protected bool UserIsEnabled
        {
            get
            {
                return UserRequest.UserStatus == 1;
            }
            set
            {
                if (value == true)
                {
                    UserRequest.UserStatus = 1;
                }
                else
                {
                    UserRequest.UserStatus = 2;
                }
            }
        }

    }
}
