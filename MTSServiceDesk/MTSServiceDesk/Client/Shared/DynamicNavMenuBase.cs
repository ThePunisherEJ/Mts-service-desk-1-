using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Client.Shared
{
    public class DynamicNavMenuBase : ComponentBase
    {

        protected struct DynMenuItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ParentID { get; set; }
            public string NavTo { get; set; }
            public bool IsLeaf { get; set; }
            public string RequiredPermissions { get; set; }
            public bool AllowAnonymous { get; set; }


        }

        protected List<DynMenuItem> fullMenu;
        //protected bool[] expandSubNavList;

        protected Dictionary<int,bool> expandSubNavDict;

        private int currentCount = 0;

        protected bool expandSubNav1;
        protected bool expandSubNav2;
        protected int activeParentID = 0;

        protected bool collapseNavMenu = true;

        protected string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        protected void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        protected void MenuOnClick(int newparentID)
        {

            //if (newparentID == 1 && activeParentID != newparentID)
            //{
            //    expandSubNav1 = !expandSubNav1;
            //}

            //if (newparentID == 2 && activeParentID != newparentID)
            //{
            //    expandSubNav2 = !expandSubNav2;
            //}

            if (activeParentID != newparentID)
            {
                //expandSubNavList[newparentID] = !expandSubNavList[newparentID];
                expandSubNavDict[newparentID] = !expandSubNavDict[newparentID];
            }

        }

        protected override Task OnInitializedAsync()
        {
            fullMenu = new List<DynMenuItem>();

            DynMenuItem menu1 = new DynMenuItem();
            menu1.ID = 1;
            menu1.ParentID = 0;
            menu1.Name = "Log Ticket";
            menu1.NavTo = "LogTicket";
            menu1.IsLeaf = true;
            menu1.AllowAnonymous = false;
            menu1.RequiredPermissions = "Admin, User, Consultant";




            DynMenuItem menu5 = new DynMenuItem();
            menu5.ID = 5;
            menu5.ParentID = 0;
            menu5.Name = "Admin";
            menu5.NavTo = "mnuAdm";
            menu5.IsLeaf = false;
            menu5.AllowAnonymous = false;
            menu5.RequiredPermissions = "Admin";



            DynMenuItem menu6 = new DynMenuItem();
            menu6.ID = 6;
            menu6.ParentID = 5;
            menu6.Name = "Clients";
            menu6.NavTo = "SupportClientsHome";
            menu6.IsLeaf = true;
            menu6.AllowAnonymous = false;
            menu6.RequiredPermissions = "Admin";

            DynMenuItem menu7 = new DynMenuItem();
            menu7.ID = 7;
            menu7.ParentID = 5;
            menu7.Name = "User Maintenace";
            menu7.NavTo = "UserMaintenanceHome";
            menu7.IsLeaf = true;
            menu7.AllowAnonymous = false;
            menu7.RequiredPermissions = "Admin";


            fullMenu.Add(menu1);

            fullMenu.Add(menu5);
            fullMenu.Add(menu6);
            fullMenu.Add(menu7);


            //expandSubNavList = new bool[fullMenu.Count + 1];

            expandSubNavDict = new Dictionary<int, bool>();

            foreach (DynMenuItem item in fullMenu.Where(m => m.IsLeaf == false))
            {
                expandSubNavDict.Add(item.ID, false);
            }


            return base.OnInitializedAsync();
        }

        protected void IncrementCount()
        {
            currentCount++;
        }
    }
}
