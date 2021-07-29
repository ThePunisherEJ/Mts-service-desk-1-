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
using Microsoft.AspNetCore.Components.Forms;
using BlazorInputFile;
using System.IO;

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


        //protected string status;
        protected string imageDataUri;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                ClientRequest = new SupportClientCreateUpdateRequest();
                ClientRequest.StatusId = 2;
            }
            else
            {
                SupportClientDetails clientDetails = await httpClient.GetFromJsonAsync<SupportClientDetails>("api/supportclient/Get-ById/" + Id.ToString());

                ClientRequest.DomainName = clientDetails.DomainName;
                ClientRequest.Id = clientDetails.Id;
                ClientRequest.Logo = clientDetails.Logo;
                ClientRequest.Name = clientDetails.Name;
                ClientRequest.StatusId = clientDetails.StatusId;
                if (ClientRequest.Logo != null)
                {
                    imageDataUri = $"data:image/jpeg;base64,{Convert.ToBase64String(ClientRequest.Logo)}";
                    //Convert.ToBase64String(ClientRequest.Logo);
                }
            }

        }


       protected async Task HandleSelection(IFileListEntry[] files)
        {
            var rawFile = files.FirstOrDefault();
            if (rawFile != null)
            {
                // Load as an image file in memory
                var format = "image/jpeg";
                var imageFile = await rawFile.ToImageFileAsync(format, 640, 480);
                var ms = new MemoryStream();
                await imageFile.Data.CopyToAsync(ms);

                // Make a data URL so we can display it
                imageDataUri = $"data:{format};base64,{Convert.ToBase64String(ms.ToArray())}";

                //status = $"Finished loading {ms.Length} bytes from {imageFile.Name}";
                ClientRequest.Logo = ms.ToArray();
            }
           
        }
        protected void Create()
        {
            Console.WriteLine(ClientRequest.Name);
        }
        


        protected void FormSave()
        {
            if (Id > 0)
            {
                ShowConfirmationModalForUpdate();
            }
            else
            {
                ShowConfirmationModalForCreate();
            }
        }

        protected async Task ShowConfirmationModalForUpdate()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true, 
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to edit existing client?");
            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters,options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            { 
               var response =  await httpClient.PostAsJsonAsync("api/SupportClient/update-supportclient", ClientRequest);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved();
                 
                    navigationManager.NavigateTo("SupportClientsHome");
                }
                else
                {
                    //Show error modal
                    await ModalFail();
                    //dont navigate away
                }
                

            }
        }

        protected async Task ShowConfirmationModalForCreate()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                HideHeader = true,


            };
            var parameters = new ModalParameters();
            parameters.Add(nameof(Shared.ModalConfirmation.confirmationMessage), "Are you sure you want to create new client?");
            

            

            var modalForm = Modal.Show<Shared.ModalConfirmation>("", parameters,options);
            ModalResult result = await modalForm.Result;
            modalForm.Close();


            if (result.Cancelled)
            {

            }
            else
            {
                var response = await httpClient.PostAsJsonAsync("api/SupportClient/create-supportclient", ClientRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Show success modal
                    await ModalSaved();

                    navigationManager.NavigateTo("SupportClientsHome");
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
            var modalForm = Modal.Show<Shared.ModalSuccess>("");
            ModalResult resultSuccess = await modalForm.Result;
           

            }

        protected async Task ModalFail()
        {
            var modalForm = Modal.Show<Shared.ModalFailed>("");
            ModalResult resultFail = await modalForm.Result;


        }

        protected void NewClientCancel()
        {
            //ShowConfirmationModal();
            navigationManager.NavigateTo("SupportClientsHome");
        }
    }
    

    
    }


