using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class DocStoreData
    {
        public int AddDocStore(DocStore docStore)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                DocStore docStoreFound = db.DocStores.FirstOrDefault(a => a.KeyId == docStore.KeyId);

                if (docStoreFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToDocStores(docStore);
                    db.SaveChanges();
                }

                return docStore.KeyId;
            }
        }

        public void UpdateDocStore(DocStore docStore)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                DocStore docStoreFound = db.DocStores.FirstOrDefault(a => a.KeyId == docStore.KeyId);

                if (docStoreFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveDocStore(DocStore docStore)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                DocStore docStoreFound = db.DocStores.FirstOrDefault(a => a.KeyId == docStore.KeyId);

                if (docStoreFound != null)
                {
                    db.DeleteObject(docStoreFound);
                    db.SaveChanges();
                }
            }
        }
    }
}
