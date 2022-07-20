using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaGE.Correspondence.Data;
using SaGE.CorrespondenceHistory.Data;


namespace SaGE.Common.Correspondence
{
    public class TransactionCommon
    {
        public void AddInitialTransaction(SaGE.Correspondence.Data.Transaction transaction, string packageData, string emailData, List<TransactionChannel> transactionChannels)
        {
            TransactionalData transactionData = new TransactionalData();
            TransactionChannelData transactionChannelData = new TransactionChannelData();
            HistoryData historyData = new HistoryData();

            int transactionKey = -1;

            transactionKey = transactionData.AddTransaction(transaction);
            transactionData.AddTransactionData(transaction, "Package", packageData);

            if (!string.IsNullOrEmpty(emailData))
            {
                transactionData.AddTransactionData(transaction, "Email", emailData);
            }

            transactionChannelData.AddTransactionChannel(transaction, transactionChannels);

            //CorrespondenceHistory.Data.Transaction historyTransaction = new CorrespondenceHistory.Data.Transaction();

            //historyTransaction.BatchId = transaction.BatchId;
            //historyTransaction.Guid = transaction.Guid;
            //historyTransaction.JobId = transaction.JobId;
            //historyTransaction.TransactionId = transaction.KeyId;

            //if (string.IsNullOrEmpty(transaction.ClientId))
            //{
            //    Entity entity = clientData.GetLatestEntity(transaction.ClientId);

            //    historyTransaction.IMedId = entity.PartyType == "PERSON" ? clientData.GetPerson(entity.KeyId).PSIId : clientData.GetOrganisation(entity.KeyId).PSIId;

            //    historyTransaction.EntityKeyId = entity.KeyId;
            //}

            //historyData.AddTransaction(historyTransaction);
            //historyData.AddTransactionData(historyTransaction, packageData);
        }

        public List<SaGE.Correspondence.Data.Transaction> GetTransactionsForJob(int jobId)
        {
            TransactionalData transactionData = new TransactionalData();

            return transactionData.GetTransactionsForJobId(jobId);
        }

        public void SetTransactionLock(List<SaGE.Correspondence.Data.Transaction> transactions)
        {
            TransactionalData transactionData = new TransactionalData();

            transactionData.SetTransactionLock(transactions);
        }

        public void ReleaseTransactionLock(List<SaGE.Correspondence.Data.Transaction> transactions)
        {
            TransactionalData transactionData = new TransactionalData();

            transactionData.ReleaseTransactionLock(transactions);
        }

        public void UpdateTransactions(List<SaGE.Correspondence.Data.Transaction> transactions)
        {
            TransactionalData transactionData = new TransactionalData();

            foreach (var transaction in transactions)
            {
                transactionData.UpdateTransaction(transaction);
            }
        }
    }
}
