using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaGE.Correspondence.Data;

namespace SaGE.Common.Correspondence
{
    public class TransactionChannelCommon
    {
        public void AddTransactionChannel(Transaction transaction, List<TransactionChannel> transactionChannels)
        {
            TransactionChannelData transactionChannelData = new TransactionChannelData();

            transactionChannelData.AddTransactionChannel(transaction, transactionChannels);
        }
    }
}
