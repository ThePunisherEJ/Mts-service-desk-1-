using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using MTS.ServiceDesk.Client.Services;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Pages.Tickets
{
    public class CreateUpdateCommentBase : ComponentBase
    {
        #region PropertiesAndParameters
        [CascadingParameter] public IModalService Modal { get; set; }

        protected TicketCommentCreateUpdateRequest TicketCommentRequest = new TicketCommentCreateUpdateRequest();
        #endregion
        #region DI
        [Inject] protected HttpClient httpClient { get; set; }
        [Inject] protected IAuthService authService { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        [Parameter] public int TicketId { get; set; }
        //[Parameter] public int ClientId { get; set; }
        [Parameter] public int CommentId { get; set; }
        [Parameter] public int ClientId { get; set; }

        #endregion
        protected override async Task OnInitializedAsync()
        {
            



            if (CommentId < 1)
            {
                TicketCommentDetails commentDetails = new TicketCommentDetails();
                var currentUser = await authService.CurrentUserInfo();

                TicketCommentRequest.Comment = "";
                TicketCommentRequest.CreatedBy = currentUser.UserId;
                TicketCommentRequest.TicketId = TicketId;
                

                //TicketCommentRequest.CreatedBy = user.UserId;

            }
            else
            {
                TicketCommentDetails commentDetails = await httpClient.GetFromJsonAsync<TicketCommentDetails>("api/ticket/Get-Comment-byid/" + CommentId.ToString());
                TicketCommentRequest.Comment = commentDetails.Comment;
                TicketCommentRequest.Id = commentDetails.Id;

                
                
            }


        }
        protected void FormSave()
        {
            if (CommentId < 1)
            {
                ShowConfirmationModalForCreateComment();
            }
            else
            {
                ShowConfirmationModalForUpdateComment();
            }
           
           
        }
        protected async Task ShowConfirmationModalForCreateComment()
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to create a new comment?");




            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/ticket/create-comment", TicketCommentRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved(TicketCommentRequest.CreatedBy + " Created Successfully");

                    navigationManager.NavigateTo("TicketView/" + ClientId.ToString() + "/" + TicketId.ToString());
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }
            }

            }
        protected async Task ShowConfirmationModalForUpdateComment()
        {
            var options = new ModalOptions()
            {
                Class = "MTS-CSS-modal-variant-02",
                HideCloseButton = true,
                DisableBackgroundCancel = true


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to update comment?");




            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters, options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/ticket/update-comment", TicketCommentRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved(TicketCommentRequest.CreatedBy + " updated Successfully");


                    navigationManager.NavigateTo("TicketView/" + ClientId.ToString() + "/" + TicketId.ToString());
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
        protected async Task NewCommentCancel()
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
                navigationManager.NavigateTo("TicketView/" + ClientId.ToString() + "/" + TicketId.ToString());
            }
        }
    }
}
