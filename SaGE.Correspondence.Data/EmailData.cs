using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class EmailData
    {
        public int AddEmail(Email email)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Email emailFound = db.Emails.FirstOrDefault(a => a.KeyId == email.KeyId);

                if (emailFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToEmails(email);
                    db.SaveChanges();
                }

                return email.KeyId;
            }
        }

        public void UpdateEmail(Email email)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Email emailFound = db.Emails.FirstOrDefault(a => a.KeyId == email.KeyId);

                if (emailFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveEmail(Email email)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Email emailFound = db.Emails.FirstOrDefault(a => a.KeyId == email.KeyId);

                if (emailFound != null)
                {
                    db.DeleteObject(emailFound);
                    db.SaveChanges();
                }
            }
        }
    }
}
