using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class BouncedMailData
    {

        public int AddBouncedMail(BouncedMail bouncedMail)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                BouncedMail channelBatchFound = db.BouncedMails.FirstOrDefault(a => a.KeyId == bouncedMail.KeyId);

                if (channelBatchFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToBouncedMails(bouncedMail);
                    db.SaveChanges();
                }
            }

            return bouncedMail.KeyId;
        }





    }
}
