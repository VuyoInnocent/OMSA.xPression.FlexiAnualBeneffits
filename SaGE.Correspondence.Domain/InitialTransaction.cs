using System.Collections.Generic;

namespace SaGE.Correspondence.Domain
{
	public class InitialTransaction
	{
		public Transaction Transaction;
		public string PackageData;
		public string EmailData;
		public List<TransactionChannel> TransactionChannels;
	}
}