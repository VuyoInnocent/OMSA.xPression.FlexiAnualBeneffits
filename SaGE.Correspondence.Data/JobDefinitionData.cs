using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class JobDefinitionData
    {
        public int AddJobDefinition(JobDefinition jobDefinition)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                JobDefinition jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.KeyId == jobDefinition.KeyId);

                if (jobDefinitionFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToJobDefinitions(jobDefinition);
                    db.SaveChanges();
                }

                return jobDefinition.KeyId;
            }
        }

        public void UpdateJobDefinition(JobDefinition jobDefinition)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                JobDefinition jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.KeyId == jobDefinition.KeyId);

                if (jobDefinitionFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveJobDefinition(JobDefinition jobDefinition)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                JobDefinition jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.KeyId == jobDefinition.KeyId);

                if (jobDefinitionFound != null)
                {
                    db.DeleteObject(jobDefinitionFound);
                    db.SaveChanges();
                }
            }
        }

        public JobDefinition GetJobDefinitionByName(string jobName)
        {
            JobDefinition jobDefinitionFound = null;

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.JobName == jobName);
            }

            return jobDefinitionFound;
        }

        public JobDefinition GetJobDefinitionById(int jobDefinitionId)
        {
            JobDefinition jobDefinitionFound = null;

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.KeyId == jobDefinitionId);
            }

            return jobDefinitionFound;
        }
    }
}
