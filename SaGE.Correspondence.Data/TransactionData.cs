using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class TransactionalData
    {
        public int AddTransaction(Transaction transaction)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Transaction transactionFound = db.Transactions.FirstOrDefault(a => a.KeyId == transaction.KeyId);

                if (transactionFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToTransactions(transaction);
                    db.SaveChanges();
                }

                return transaction.KeyId;
            }
        }

        public void UpdateTransaction(Transaction transaction)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Transaction transactionFound = db.Transactions.FirstOrDefault(a => a.KeyId == transaction.KeyId);

                if (transactionFound != null)
                {
                    if(transaction.JobId != null && transactionFound.JobId != transaction.JobId) transactionFound.JobId = transaction.JobId;

                    if (transaction.BatchId != null && transactionFound.BatchId != transaction.BatchId) transactionFound.BatchId = transaction.BatchId;

                    if (transaction.TransactionDefinitionId != null && transactionFound.TransactionDefinitionId != transaction.TransactionDefinitionId) transactionFound.TransactionDefinitionId = transaction.TransactionDefinitionId;

                    if (transaction.ChannelId != null && transactionFound.ChannelId != transaction.ChannelId) transactionFound.ChannelId = transaction.ChannelId;

                    if (transaction.Guid != null && transactionFound.Guid != transaction.Guid) transactionFound.Guid = transaction.Guid;

                    if (transaction.ClientId != null && transactionFound.ClientId != transaction.ClientId) transactionFound.ClientId = transaction.ClientId;

                    if (transaction.LOBId != null && transactionFound.LOBId != transaction.LOBId) transactionFound.LOBId = transaction.LOBId;

                    if (transaction.LOBKey != null && transactionFound.LOBKey != transaction.LOBKey) transactionFound.LOBKey = transaction.LOBKey;

                    if (transaction.State != null && transactionFound.State != transaction.State) transactionFound.State = transaction.State;

                    if (transaction.Locked != null && transactionFound.Locked != transaction.Locked) transactionFound.Locked = transaction.Locked;

                    if (transaction.LockDate != null && transactionFound.LockDate != transaction.LockDate) transactionFound.LockDate = transaction.LockDate;
                    
                    db.SaveChanges();
                }
            }
        }

        public void RemoveTransaction(Transaction transaction)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Transaction transactionFound = db.Transactions.FirstOrDefault(a => a.KeyId == transaction.KeyId);

                if (transactionFound != null)
                {
                    db.DeleteObject(transaction);
                    db.SaveChanges();
                }
            }
        }

        public void AddTransactionData(Transaction transaction, string dataType, string packageData)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                TransDataType transDataType = db.TransDataTypes.FirstOrDefault(a => a.Description == dataType);

                TransactionData transData = new TransactionData();

                if (transDataType != null) transData.DataTypeId = transDataType.KeyId;

                transData.TransactionId = transaction.KeyId;
                transData.Data = packageData;

                db.AddToTransactionDatas(transData);
                db.SaveChanges();
            }
        }

        public List<Transaction> GetTransactionsForJobId(int jobId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                transactions = db.Transactions.Where(a => a.JobId == jobId).ToList();
            }

            return transactions;
        }

        public List<Transaction> GetTransactionsForBatchId(int batchId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                transactions = db.Transactions.Where(a => a.BatchId == batchId).ToList();
            }

            return transactions;
        }

        public List<Transaction> GetTransactionsForJobIdByChannel(int jobId, int channelId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                transactions = db.Transactions.Where(a => a.JobId == jobId && a.ChannelId == channelId).OrderBy(a=>a.PostCode).ThenBy(k => k.KeyId).ToList();
            }

            return transactions;
        }


        public List<Transaction> GetTransactionsbyJob(int jobKey)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                List<Transaction> transactionsFound = db.Transactions.Where(a => a.JobId == jobKey).ToList();

                if (transactionsFound.Count > 0)
                {
                    transactions = transactionsFound;
                }
            }

            return transactions;
        }

        public List<Transaction> GetTransactionsOrderByJobThenTransactionId()
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Transactions.OrderBy(a=>a.JobId).ThenBy(a=>a.KeyId).ToList();
            }
        }


        public TransactionData GetTransactionDataForTransactionId(int transactionid, int dataType)
        {
            TransactionData transactionDataFound;

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                transactionDataFound = db.TransactionDatas.FirstOrDefault(a => a.TransactionId == transactionid && a.DataTypeId == dataType);
            }

            return transactionDataFound;
        }

        public void SetTransactionLock(List<Transaction> transactions)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
               //Lock for exclusive use.
                foreach (Transaction transaction in transactions)
                {
                    Transaction transactionFound = db.Transactions.FirstOrDefault(a => a.KeyId == transaction.KeyId);

                    if(transactionFound != null)
                    {
                        transactionFound.Locked = true;
                        transactionFound.LockDate = DateTime.Now;

                        db.SaveChanges();
                    }
                }
            }
        }

        public void ReleaseTransactionLock(List<Transaction> transactions)
        {   
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
               //Release lock
                foreach (Transaction transaction in transactions)
                {
                    Transaction transactionFound = db.Transactions.FirstOrDefault(a => a.KeyId == transaction.KeyId);

                    if (transactionFound != null)
                    {
                        transactionFound.Locked = null;
                        transactionFound.LockDate = null;

                        db.SaveChanges();
                    }
                }
            }
        }

        public void UpdateTransactionAddClientId(Transaction transaction)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Transaction transactionFound = db.Transactions.FirstOrDefault(a => a.KeyId == transaction.KeyId);

                if (transactionFound != null)
                {
                    transactionFound.ClientId = transaction.ClientId;
                    db.SaveChanges();
                }
            }
        }

        public List<Transaction> GetTransactionsForJobByChannel(int jobId, int channelId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                List<Transaction> transactionsFound = db.Transactions.Where(a => a.JobId == jobId && a.ChannelId == channelId).ToList();

                if (transactionsFound.Count > 0)
                {
                    transactions = transactionsFound;
                }
            }

            return transactions;
        }

        public List<Transaction> GetAllTransactions()
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Transactions.ToList();
            }
        }

        public List<TransactionData> GetAllTransactionData()
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.TransactionDatas.ToList();
            }
        }

        public List<TransactionData> GetAllTransactionDataByDataType(int dataType, int index)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.TransactionDatas.Where(a => a.DataTypeId == dataType && a.TransactionId > index).ToList();
            }
        }


        public Transaction GetTransactionForTransactionId(int keyId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Transactions.FirstOrDefault(a => a.KeyId == keyId);
            }
        }

        public Transaction GetTransactionForLOBId(string LOBId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Transactions.FirstOrDefault(a => a.LOBId == LOBId);
            }
        }

        public void UpdateTransactionData(TransactionData transactionData)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                TransactionData transactionDataFound = db.TransactionDatas.FirstOrDefault(a => a.KeyId == transactionData.KeyId);

                if (transactionDataFound != null)
                {
                    transactionDataFound.Data = transactionData.Data;
                    transactionDataFound.DataTypeId = transactionData.DataTypeId;
                    transactionDataFound.TransactionId = transactionData.TransactionId;

                    db.SaveChanges();
                }
            }
        }

        public List<TransactionData> GetAllPackageTransactionData()
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.TransactionDatas.Where(a=>a.DataTypeId == 1).ToList();
            }
        }

        public Transaction GetTransactionByKeyId(int keyId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Transactions.FirstOrDefault(a => a.KeyId == keyId);
            }
        }

        public Transaction GetTransactionByClientId(string clientId, int jobId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Transactions.FirstOrDefault(a => a.ClientId == clientId && a.JobId == jobId);
            }
        }
    }
}
