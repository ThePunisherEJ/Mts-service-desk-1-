using Microsoft.AspNetCore.Components;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private int _selectedClientId;
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
            }

        }

        [Parameter] public int Id { get; set; }
        #endregion
        protected void FormSave()
        {
            //if (Id == 0)
            //{

            //    ShowConfirmationModalForCreateSystem();
            //}
            //else
            //{
            //    ShowConfirmationModalForUpdateSystem();
            //}
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
        public string TicketDate
        {
            get
            {
                return TicketRequest.DateCreated.ToString();
            }
            set
            {
                TicketRequest.DateCreated = DateTime.Parse(value);
            }


        }
    }
}
