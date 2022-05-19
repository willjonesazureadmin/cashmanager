using System;

namespace cashmanager.web.spa.Helpers
{
    public class AppSettings
    {
        public class LocalConfigurations
        {

            public AzureAdSettings? AzureAd { get; set; }

            public APIs? apis { get; set; }

            public class APIs 
            {
                public API? AccountsAPI { get; set; }
                public API? TransactionsAPI { get; set; }
            }

            public class API
            {
                public string? ClientName { get; set; }
                public string[]? Scopes { get; set; }
                public string? BaseUrl { get; set; }
                public string? ApplicationId { get; set; }

                public string? ServiceName {get; set;}
            }

            public class AzureAdSettings
            {
                public string? Authority { get; set; }
                public string? ClientId { get; set; }
                public bool ValidateAuthority { get; set; }
            }
        }
    }
}
