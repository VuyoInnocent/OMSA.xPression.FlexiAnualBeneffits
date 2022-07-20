using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class TransactionChannelData
    {
        public void AddTransactionChannel(Transaction transaction, List<TransactionChannel> transactionChannels)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                foreach (TransactionChannel transactionChannel in transactionChannels)
                {
                    transactionChannel.TransactionId = transaction.KeyId;

                    db.AddToTransactionChannels(transactionChannel);
                }

                db.SaveChanges();
            }
        }

        public List<TransactionChannel> GetTransactionChannelsByTransactionId(int transactionId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
               return db.TransactionChannels.Where(a => a.TransactionId == transactionId).ToList();
            }
        }

        public List<TransactionChannel> GetTransactionChannelsByEmailAddress(string emailAddress)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.TransactionChannels.Where(a => a.ContactString == emailAddress).ToList();
            }
        }

        public List<TransactionChannel> GetTransactionChannelsByChannelOrderByPostCode(int transactionId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.TransactionChannels.Where(a => a.ChannelId == 2 && a.TransactionId >= transactionId).OrderBy(a=>a.PostalCode).ToList();
            }
        }

    }
}
