using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class JobData
    {
        public int AddJob(Job job)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Job jobFound = db.Jobs.FirstOrDefault(a => a.KeyId == job.KeyId);

                if (jobFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToJobs(job);
                    db.SaveChanges();
                }

                return job.KeyId;
            }
        }

        public void UpdateJob(Job job)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Job jobFound = db.Jobs.FirstOrDefault(a => a.KeyId == job.KeyId);

                if (jobFound != null)
                {
                    jobFound.JobName = job.JobName;
                    jobFound.SourcePath = job.SourcePath;
                    jobFound.LoadDate = job.LoadDate;
                    jobFound.DeliveryDate = job.DeliveryDate;
                    jobFound.Process = job.Process;
                    jobFound.State = job.State;

                    db.SaveChanges();
                }
            }
        }

        public void UpdateJobStatus(int jobKey, string status)
        {
            JobData jobData = new JobData();

            Job job = new Job();

            job = jobData.GetJobFromJobKey(jobKey);

            job.State = status;

            jobData.UpdateJob(job);
        }


        public void RemoveJob(Job job)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Job jobFound = db.Jobs.FirstOrDefault(a => a.KeyId == job.KeyId);

                if (jobFound != null)
                {
                    db.DeleteObject(jobFound);
                    db.SaveChanges();
                }
            }
        }

        public List<Job> GetJobs()
        {
            List<Job> jobs = new List<Job>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                jobs = db.Jobs.ToList();
            }

            return jobs;
        }


        public Job GetJobFromJobKey(int jobKey)
        {
            Job returnJob = new Job();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Job jobFound = db.Jobs.FirstOrDefault(a => a.KeyId == jobKey);

                if (jobFound != null)
                {
                    returnJob = jobFound;
                }
            }

            return returnJob;
        }

        public string GetJobNameFromJobDefinition(string jobName)
        {
            string returnJobName = string.Empty;

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                JobDefinition jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.JobName == jobName);

                if (jobDefinitionFound != null)
                {
                    returnJobName = jobDefinitionFound.JobName;
                }
            }

            return returnJobName;
        }

        public int GetJobDefIdFromJobDefinition(string jobName)
        {
            int returnJobDefId = -1;

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                JobDefinition jobDefinitionFound = db.JobDefinitions.FirstOrDefault(a => a.JobName == jobName);

                if (jobDefinitionFound != null)
                {
                    returnJobDefId = jobDefinitionFound.KeyId;
                }
            }

            return returnJobDefId;
        }

        public List<Job> GetJobListByState(string status)
        {
            List<Job> jobs = new List<Job>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                List<Job> jobsFound = db.Jobs.Where(a => a.State == status).ToList();

                if (jobsFound.Count > 0)
                {
                    jobs = jobsFound;
                }
            }

            return jobs;
        }



        public List<JobDefinition> GetAllJobDefinitions()
        {
            List<JobDefinition> jobDefinitions = new List<JobDefinition>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                List<JobDefinition> jobDefinitionsFound = db.JobDefinitions.ToList();

                if (jobDefinitionsFound.Count > 0)
                {
                    jobDefinitions = jobDefinitionsFound;
                }
            }

            return jobDefinitions;
        }

        public Job GetJobByState(string state, string jobType, int engine)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Job job = db.Jobs.Include("JobDefinition").FirstOrDefault(a => a.State == state && a.JobDefinition.JobType == jobType && a.JobDefinition.EngineId == engine);

                return job;

            }
        }

        public Job GetJobByJobId(int jobId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Job job = db.Jobs.FirstOrDefault(a => a.KeyId == jobId);

                return job;
            }
        }
    }
}
