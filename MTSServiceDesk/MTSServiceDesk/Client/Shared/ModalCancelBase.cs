using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Shared
{
    public class ModalCancelBase : ComponentBase
    {
        #region DI
        [Inject]
        protected NavigationManager navigationManager { get; set; }



        #endregion
        #region Properties and Parameters
   

        [CascadingParameter]
        protected BlazoredModalInstance blazoredModal { get; set; }
        #endregion
        protected void ModalNo()
        {

            blazoredModal.Close(ModalResult.Cancel());

        }

        protected void ModalYes()
        {
            blazoredModal.Close(ModalResult.Ok(""));


        }
    }
}
