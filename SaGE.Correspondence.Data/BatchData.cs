using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class BatchData
    {
        public int AddBatch(Batch batch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Batch batchFound = db.Batches.FirstOrDefault(a => a.KeyId == batch.KeyId);

                if (batchFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToBatches(batch);
                    db.SaveChanges();
                }

                return batch.KeyId;
            }
        }

        public void UpdateBatch(Batch batch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Batch batchFound = db.Batches.FirstOrDefault(a => a.KeyId == batch.KeyId);

                if (batchFound != null)
                {
                    batchFound.Name = batch.Name;
                    batchFound.StartDate = batch.StartDate;
                    batchFound.State = batch.State;
                    batchFound.Enabled = batch.Enabled;
                    batchFound.Engine = batch.Engine;
                    batchFound.ExpiryDate = batch.ExpiryDate;
                    batchFound.JobId = batch.JobId;
                    batchFound.JobType = batch.JobType;

                    db.SaveChanges();
                }
            }
        }

        public void RemoveBatch(Batch batch)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Batch batchFound = db.Batches.FirstOrDefault(a => a.Name == batch.Name);

                if (batchFound != null)
                {
                    db.DeleteObject(batchFound);
                    db.SaveChanges();
                }
            }
        }

        public List<Batch> GetBatches(string state, int engine)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Batches.Where(a => a.State == state && a.Engine == engine).ToList();
            }

            //using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            //{
            //    List<Batch> batches = db.Batches.Where(a => a.State == state && a.Engine == engine).ToList();

            //    if(batches.Any())
            //    {
            //        return batches;
            //    }
            //}

            //return null;
        }

        public Batch GetBatch(string state, string batchType, int engine)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Batches.FirstOrDefault(a => a.State == state && a.JobType == batchType && a.Engine == engine);
            }
        }

        public List<Batch> GetBatchesByJobId(int jobId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Batches.Where(a => a.JobId == jobId).ToList();
            }
        }
    }
}
