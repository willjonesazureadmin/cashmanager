using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using cashmanager.web.spa.Models;
using Microsoft.AspNetCore.Components;

namespace cashmanager.web.spa.Services
{
    public class HistoryService : IHistoryService
    {
        private List<PageHistory> PageHistories = new List<PageHistory>();

        private NavigationManager _navManager;

        public HistoryService(NavigationManager navigationManager)
        {
            this._navManager = navigationManager;
            this.PageHistories.Add(new PageHistory(){ 
                PageName = "Home",
                Url =  _navManager.BaseUri
                });
        }

        public void AddPageToHistory(string pageName)
        {
            if((this.PageHistories.Last().Url != _navManager.Uri))
            {
            PageHistories.Add(new PageHistory(){ 
                PageName = pageName,
                Url =  _navManager.Uri
                });
            }

        }

        // public string BreadCrumb()
        // {
        //     var sb = new System.Text.StringBuilder();            
        //     foreach(var i in PageHistories.Skip(1).Take(3).ToList())            
        //     {
        //         sb.Append(i.PageName);
        //         sb.Append(" > ");
        //     }
        //     return sb.ToString();
        // }

        public List<PageHistory> BreadCrumb()
        {
            return this.PageHistories.TakeLast(3).ToList();
        }
        
        public void Back(PageHistory ph)
        {
            _navManager.NavigateTo(ph.Url);
        }

        public string Penultimate()
        {
            return PageHistories.Skip(1).LastOrDefault().PageName;
        }


        public string Latest()
        {
            return PageHistories.LastOrDefault().PageName;
        }

        public bool ShowHistory()
        {
            return (PageHistories.Count >= 2)  ? true : false;
        }
    }

    public class PageHistory
    {
        public string PageName { get; set; }
        public string Url { get; set; }
    }
}