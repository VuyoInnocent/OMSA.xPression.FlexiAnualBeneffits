using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class MergeBatchData
    {

        public int AddMergeBatch(MergeBatch mergeBatch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                MergeBatch mergeBatchFound = db.MergeBatches.FirstOrDefault(a => a.KeyId == mergeBatch.KeyId);

                if (mergeBatchFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToMergeBatches(mergeBatch);
                    db.SaveChanges();
                }
            }

            return mergeBatch.KeyId;
        }

        public void UpdateMergeBatch(MergeBatch mergeBatch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                MergeBatch mergeBatchFound = db.MergeBatches.FirstOrDefault(a => a.KeyId == mergeBatch.KeyId);

                if (mergeBatchFound != null)
                {
                    if (mergeBatchFound.Name != mergeBatch.Name) mergeBatchFound.Name = mergeBatch.Name;
                    if (mergeBatchFound.State != mergeBatch.State) mergeBatchFound.State = mergeBatch.State;
                    if (mergeBatchFound.ChannelId != mergeBatch.ChannelId) mergeBatchFound.ChannelId = mergeBatch.ChannelId;
                    if (mergeBatchFound.Date != mergeBatch.Date) mergeBatchFound.Date = mergeBatch.Date;
                    if (mergeBatchFound.Enabled != mergeBatch.Enabled) mergeBatchFound.Enabled = mergeBatch.Enabled;
                    if (mergeBatchFound.Engine != mergeBatch.Engine) mergeBatchFound.Engine = mergeBatch.Engine;
                    if (mergeBatchFound.JobId != mergeBatch.JobId) mergeBatchFound.JobId = mergeBatch.JobId;

                    db.SaveChanges();
                }
            }
        }

        public void RemoveMergeBatch(MergeBatch mergeBatch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                MergeBatch mergeBatchFound = db.MergeBatches.FirstOrDefault(a => a.KeyId == mergeBatch.KeyId);

                if (mergeBatchFound != null)
                {
                    db.DeleteObject(mergeBatchFound);
                    db.SaveChanges();
                }
            }
        }

        public int AddMergeBatchTransaction(MergeBatchTransaction mergeBatchTransaction)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                MergeBatchTransaction mergeBatchTransactionFound = db.MergeBatchTransactions.FirstOrDefault(a => a.KeyId == mergeBatchTransaction.KeyId);

                if (mergeBatchTransactionFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToMergeBatchTransactions(mergeBatchTransaction);
                    db.SaveChanges();
                }
            }

            return mergeBatchTransaction.KeyId;
        }

        public List<MergeBatch> GetMergeBatchesByState(int state, int engine)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatches.Where(a => a.State == state && a.Engine == engine).ToList();
            }
        }

        public List<MergeBatch> GetMergeBatchesByChannelIdAndState(int channelId, int state)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatches.Where(a => a.ChannelId == channelId && a.State == state).ToList();
            }
        }

        public List<MergeBatchTransaction> GetMergeBatchTransactionByMergeBatchAndChannel(int mergeBatchId, int channel)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatchTransactions.Where(a => a.MergeBatchId == mergeBatchId && a.MergeBatchId == channel).ToList();
            }
        }

        public List<MergeBatchTransaction> GetMergeBatchTransactionByMergeBatch(int mergeBatchId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatchTransactions.Where(a => a.MergeBatchId == mergeBatchId).OrderBy(a=>a.KeyId).ToList();
            }
        }

        public MergeBatch GetMergeBatchByBatchId(int mergeBatchId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatches.FirstOrDefault(a => a.KeyId == mergeBatchId);
            }
        }

        public MergeBatchTransaction GetMergeBatchTransactionByTransactionId(int? transactionId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatchTransactions.FirstOrDefault(a => a.TransactionId == transactionId.Value);
            }
        }

        public MergeBatch GetMergeBatchByMergeBatchId(int batchId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.MergeBatches.FirstOrDefault(a => a.KeyId == batchId);
            }
        }
    }
}
