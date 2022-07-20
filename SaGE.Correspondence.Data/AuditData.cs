using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class AuditData : BaseDAL
    {


        public void InsertAudit(Audit audit, bool saveChanges =true)
        {
            if (audit == null)
                throw new ArgumentNullException("audit cannot be null");

            Context.AddToAudits(audit);
            if (saveChanges)
                Save();
        }

        
        public Audit GetAuditById(int Id) 
        {
            if (Id == 0)
                return null;

            var query = from q in Context.Audits where q.Id == Id select q;

            return query.FirstOrDefault();
        }

       
        public Audit GetAuditByFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var query = from q in Context.Audits where q.Filename.ToLower().Trim() == name.ToLower().Trim() select q;

            return query.FirstOrDefault();
        }

        public void UpdateAudit(Audit audit)
        {
            if (audit == null)
                throw new ArgumentNullException("Audit cannot be null");

            var query = from q in Context.Audits where q.Id == audit.Id select q;
           
            if (query.Count() < 1) 
                throw new ArgumentNullException("Record was not found in the database");

           query.First().DateSent = audit.DateSent;           
           Save();

        }

    }
}
