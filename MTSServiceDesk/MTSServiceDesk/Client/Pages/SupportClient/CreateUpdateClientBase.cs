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


        #endregion

       protected string status;
       protected string imageDataUri;
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

                status = $"Finished loading {ms.Length} bytes from {imageFile.Name}";
            }
        }
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


