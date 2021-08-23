using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Shared
{
    public class ModalConfirmationBase : ComponentBase
    {
        #region DI
        [Inject] 
        protected NavigationManager navigationManager { get; set; }


        [CascadingParameter] 
        protected BlazoredModalInstance blazoredModal { get; set; }
        #endregion
        #region Properties and Parameters
        [Parameter]
        public string confirmationMessage { get; set; }
        #endregion


        protected void ModalCancel()
        {

            blazoredModal.Close(ModalResult.Cancel());
            
        }

        protected  void ModalSave()
        {
            //navigationManager.NavigateTo("");
            blazoredModal.Close(ModalResult.Ok(""));

        }
    }
}
