using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class TransactionContactPointData
    {
        public void AddTransactionContactPoints(List<TransactionContactPoint> transactionContactPoints)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                foreach (TransactionContactPoint transactionContactPoint in transactionContactPoints)
                {
                    db.AddToTransactionContactPoints(transactionContactPoint);
                }

                db.SaveChanges();
            }
        }
    }
}
