using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MTS.ServiceDesk.Client.Services;
using MTS.ServiceDesk.Shared.Models;

namespace MTS.ServiceDesk.Client.Pages.Authentication
{
    public class LoginBase : ComponentBase

    {
        [Inject] protected NavigationManager navigationManager { get; set; }
            [Inject] protected CustomStateProvider authStateProvider { get; set; }
       protected LoginRequest loginRequest { get; set; } = new LoginRequest();
      protected  string error { get; set; }


       protected async Task OnSubmit()
        {
            error = null;
            try
            {
                await authStateProvider.Login(loginRequest);
                navigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}
