using System;

namespace cashmanager.web.spa.Models
{
    public class UpdateAccountModel
    {
        public Guid Id { get; set; }

        public string? FriendlyName { get; set; }   

        public string? Provider { get; set; }

        public string? AccountNumber { get; set; }

        public string? SortCode { get; set; }


        public UpdateAccountModel()
        {
            
        }

        public UpdateAccountModel(GetAccountModel model)
        {
            AccountNumber = model.AccountNumber;
            FriendlyName = model.FriendlyName;
            Id = model.Id;
            Provider = model.Provider;
            SortCode = model.SortCode;
        }
    }
}
