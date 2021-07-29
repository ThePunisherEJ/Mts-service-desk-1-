using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Shared
{
   
    public class ModalFailedBase : ComponentBase
    {
        #region DI
        [Inject]
        protected NavigationManager navigationManager { get; set; }


        [CascadingParameter]
        protected BlazoredModalInstance blazoredModal { get; set; }
        #endregion
        public void ModalFailedShow()
        {
            blazoredModal.Close();
        }
    }
}
