using System;

namespace cashmanager.web.spa.Models
{
    public class AddAccountModel
    {
        public double Balance { get; set; }
        public string? FriendlyName { get; set; }   

        public string? Provider { get; set; }

        public string? AccountNumber { get; set; }

        public string? SortCode { get; set; }   
    }
}
