using System;

namespace cashmanager.api.accounts.Profiles
{
    public class AccountProfile : AutoMapper.Profile
    {
        public AccountProfile()
        {
            CreateMap<Db.Account, Models.GetAccountModel>();
            CreateMap<Models.AddAccountModel, Db.Account>();
        }
        
    }
}
