using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class ChannelBatchData
    {
        public int AddChannelBatch(ChannelBatch channelBatch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelBatch channelBatchFound = db.ChannelBatches.FirstOrDefault(a => a.KeyId == channelBatch.KeyId);

                if (channelBatchFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToChannelBatches(channelBatch);
                    db.SaveChanges();
                }
            }

            return channelBatch.KeyId;
        }

        public void UpdateChannelBatch(ChannelBatch channelBatch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelBatch channelBatchFound = db.ChannelBatches.FirstOrDefault(a => a.KeyId == channelBatch.KeyId);

                if (channelBatchFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveChannelBatch(ChannelBatch channelBatch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelBatch channelBatchFound = db.ChannelBatches.FirstOrDefault(a => a.KeyId == channelBatch.KeyId);

                if (channelBatchFound != null)
                {
                    db.DeleteObject(channelBatchFound);
                    db.SaveChanges();
                }
            }
        }

        public int AddChannelBatchTransaction(ChannelBatchTransaction channelBatchTransaction)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelBatchTransaction channelBatchTransactionFound = db.ChannelBatchTransactions.FirstOrDefault(a => a.KeyId == channelBatchTransaction.KeyId);

                if (channelBatchTransactionFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToChannelBatchTransactions(channelBatchTransaction);
                    db.SaveChanges();
                }
            }

            return channelBatchTransaction.KeyId;
        }


    }
}
