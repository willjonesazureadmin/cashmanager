using System;

namespace cashmanager.api.transactions.Profiles
{
    public class TransactionProfile : AutoMapper.Profile
    {
        public TransactionProfile()
        {
            CreateMap<Db.Transaction, Models.GetTransactionModel>();
            CreateMap<Models.GetTransactionModel, Db.Transaction>();
            CreateMap<Db.Transaction, Models.AddTransactionModel>();
            CreateMap<Models.AddTransactionModel, Db.Transaction>();

        }
    }
}
