using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class TemplateData
    {
        public int AddTemplate(Template template)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Template templateFound = db.Templates.FirstOrDefault(a => a.KeyId == template.KeyId);

                if (templateFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToTemplates(template);
                    db.SaveChanges();
                }

                return template.KeyId;
            }
        }

        public void UpdateTemplate(Template template)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Template templateFound = db.Templates.FirstOrDefault(a => a.KeyId == template.KeyId);

                if (templateFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveTemplate(Template template)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Template templateFound = db.Templates.FirstOrDefault(a => a.KeyId == template.KeyId);

                if (templateFound != null)
                {
                    db.DeleteObject(templateFound);
                    db.SaveChanges();
                }
            }
        }


        public int AddTemplateGroup(TemplateGroup templateGroup)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                TemplateGroup templateGroupFound = db.TemplateGroups.FirstOrDefault(a => a.KeyId == templateGroup.KeyId);

                if (templateGroupFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToTemplateGroups(templateGroup);
                    db.SaveChanges();
                }

                return templateGroup.KeyId;
            }
        }

        public void UpdateTemplateGroup(TemplateGroup templateGroup)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                TemplateGroup templateGroupFound = db.TemplateGroups.FirstOrDefault(a => a.KeyId == templateGroup.KeyId);

                if (templateGroupFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveTemplateGroup(TemplateGroup templateGroup)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                TemplateGroup templateGroupFound = db.TemplateGroups.FirstOrDefault(a => a.KeyId == templateGroup.KeyId);

                if (templateGroupFound != null)
                {
                    db.DeleteObject(templateGroup);
                    db.SaveChanges();
                }
            }
        }

        public Email GetEmailData(string jobName)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Emails.Include("JobDefinition").FirstOrDefault(a => a.JobDefinition.JobName == jobName);
            }
        }

        public Template GetTemplate(int templateId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Templates.FirstOrDefault(a => a.KeyId == templateId);
            }
        }
    }
}
