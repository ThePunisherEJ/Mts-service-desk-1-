using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Shared
{
    public class ModalSuccessBase : ComponentBase
    {
        #region DI
        [Inject]
        protected NavigationManager navigationManager { get; set; }


        [CascadingParameter]
        protected BlazoredModalInstance blazoredModal { get; set; }
        [Parameter]
        public string SuccessMessage { get; set; }
        #endregion

        public void ModalSuccessShow()
        {
            blazoredModal.Close(ModalResult.Ok(""));
        }
    }
}
