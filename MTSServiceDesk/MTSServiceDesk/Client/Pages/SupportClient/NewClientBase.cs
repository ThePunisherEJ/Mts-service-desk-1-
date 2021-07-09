using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Pages.SupportClient
{
    public class NewClientBase : ComponentBase
    {
        protected AddNew add = new AddNew();
        protected void Create()
        {
            Console.WriteLine(add.Name);
        }
        [CascadingParameter] public IModalService Modal { get; set; }


       protected void ShowConfirmationModal()
        {
            var options = new ModalOptions()
            {



            };

            Modal.Show<Shared.ModalConfirmation>("", options);

        }
    }
}
