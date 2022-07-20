using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class AccountData
    {
        public int AddAccount(Account account)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Account accountFound = db.Accounts.FirstOrDefault(a => a.AccountName == account.AccountName);

                if(accountFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToAccounts(account);
                    db.SaveChanges();
                }

                return account.KeyId;
            }
        }

        public void UpdateAccountCount(int accountId, int count)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Account accountFound = db.Accounts.FirstOrDefault(a => a.KeyId == accountId);

                if (accountFound != null)
                {
	                accountFound.Counter = count;
					
					db.SaveChanges();
                }
            }
        }

        public Account GetAccount(int accountId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Accounts.FirstOrDefault(a => a.KeyId == accountId);
            }
        }

        public Account GetNewCorrespondenceNumber(string lineOfBusiness, string businessArea)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Account account = db.Accounts.FirstOrDefault(a => a.LineOfBusiness == lineOfBusiness && a.BusinessArea == businessArea);

                if (account != null)
                {
                    account.Counter = account.Counter + 1;

                    db.SaveChanges();
                }

                return account ?? null;
            }
        }

    }
}
