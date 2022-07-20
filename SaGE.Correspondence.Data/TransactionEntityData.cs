using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class TransactionEntityData
    {
        public void AddTransactionEntity(TransactionEntity transactionEntity)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                db.AddToTransactionEntities(transactionEntity);

                db.SaveChanges();
            }
        }
    }
}
