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
    public class CreateUpdateTicketBase : ComponentBase
    {
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        #endregion
        #region Properties And Parameters
        public TicketCreateUpdateRequest TicketRequest = new TicketCreateUpdateRequest();
        protected List<SupportClientDetails> clients = new List<SupportClientDetails>();
        protected List<SystemDetails> Systems = new List<SystemDetails>();
        protected UserCreateUpdateRequest UserRequest = new UserCreateUpdateRequest();
        private int _selectedClientId;
        private int _selectedSystemId;

        [CascadingParameter]

        public IModalService Modal { get; set; }
        [Parameter] public string ClientId { get; set; }
        [Parameter] public string SystemId { get; set; }
        public string SelectedClientId
        {
            get
            {
                return _selectedClientId.ToString();
            }

            set
            {
                _selectedClientId = int.Parse(value);
                TicketRequest.ClientId = _selectedClientId;
                PopulateSystemList();
                

            }

        }
        public string SelectedSystemID
        {
            get
            {
                return _selectedSystemId.ToString();
            }

            set
            {
                _selectedSystemId = int.Parse(value);
                TicketRequest.SystemId = _selectedSystemId;


            }

        }

        [Parameter] public int Id { get; set; }
        #endregion
        protected override async Task OnInitializedAsync()
        {
            await PopulateClientList();
            _selectedClientId = int.Parse(ClientId);
            await PopulateSystemList();



            if (Id == 0 )
            {
                CurrentUser user = await httpClient.GetFromJsonAsync<CurrentUser>("api/auth/currentuserinfo");
                SelectedClientId = ClientId;
                SelectedSystemID = Systems.FirstOrDefault().Id.ToString();
                //SelectedSystemID = SystemId;
                TicketRequest.StatusId = 1;
                TicketRequest.DateCreated = DateTime.Now;
                TicketRequest.CreatedBy = user.UserId;
            }
            else
            {
                
                TicketDetails ticketDetails = await httpClient.GetFromJsonAsync<TicketDetails>("api/ticket/Get-Ticket/" + Id.ToString());
            
                TicketRequest.ClientId = ticketDetails.ClientId;
                TicketRequest.Id = ticketDetails.Id;
                TicketRequest.DateCreated = ticketDetails.DateCreated;
                TicketRequest.CreatedBy = ticketDetails.CreatedBy;
                TicketRequest.Description = ticketDetails.Description;
                TicketRequest.StatusId = ticketDetails.StatusId;
                TicketRequest.SystemId = ticketDetails.SystemId;
                
            }


        }
       
        public string TicketID {
            get 
            {
                return TicketRequest.Id.ToString();       
            }

            set
            {
                TicketRequest.Id = int.Parse(value);

            }
        }
        protected async Task PopulateClientList()
        {
            clients = await httpClient.GetFromJsonAsync<List<SupportClientDetails>>("api/SupportClient/Get-All");

        }
        protected async Task PopulateSystemList()
        {
            Systems = await httpClient.GetFromJsonAsync<List<SystemDetails>>("api/systems/Get-All-Systems-For-Client/" + _selectedClientId.ToString());
            StateHasChanged();
            SelectedSystemID = Systems.FirstOrDefault().Id.ToString();

        }
        protected void FormSave()
        {
            if (Id == 0)
            {

                ShowConfirmationModalForCreateTicket();
            }
            else
            {
                ShowConfirmationModalForUpdateTicket();
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
        protected async Task NewTicketCancel()
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
                navigationManager.NavigateTo("TicketsHome");
            }
        }

        protected async Task ModalFail()
        {
            var modalForm = Modal.Show<Shared.ModalFailed>("");
            ModalResult resultFail = await modalForm.Result;


        }

        protected async Task ShowConfirmationModalForCreateTicket()
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to create a new ticket?");




            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/ticket/create-ticket", TicketRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved(TicketRequest.CreatedBy + " Created Successfully");

                    navigationManager.NavigateTo("TicketsHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }


            }
        }

        protected async Task ShowConfirmationModalForUpdateTicket()
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to update the ticket?");
            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/ticket/update-ticket", TicketRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved(TicketRequest.CreatedBy + " Updated Successfully");

                    navigationManager.NavigateTo("TicketsHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }


            }
        }

    }
}
