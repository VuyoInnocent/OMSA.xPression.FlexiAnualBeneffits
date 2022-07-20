using System;
using System.Collections.Generic;
using SaGE.Correspondence.Data;


namespace SaGE.Common.Correspondence
{
    public class JobCommon
    {
        public int AddJob(string jobName, string sourcePath, DateTime loadDate, DateTime deliveryDate, string process, string processState)
        {
            int jobAdded = -1;

            JobData jobData = new JobData();
            Job job = new Job();

            job.JobDefId = jobData.GetJobDefIdFromJobDefinition(jobName);
            job.JobName = jobName;

            if (!string.IsNullOrEmpty(job.JobName))
            {
                job.SourcePath = sourcePath;
                job.LoadDate = loadDate;
                job.DeliveryDate = deliveryDate;
                job.Process = process;
                job.State = processState;

                jobAdded = jobData.AddJob(job);
            }

            return jobAdded;
        }

        public void UpdateJobStatus(Job job)
        {
            JobData jobData = new JobData();

            jobData.UpdateJob(job);
        }


        public void UpdateJobStatus(int jobKey, string status)
        {
            JobData jobData = new JobData();

            Job job = new Job();

            job = jobData.GetJobFromJobKey(jobKey);

            job.State = status;

            jobData.UpdateJob(job);
        }

        public List<Job> GetJobList(string state)
        {
            List<Job> jobs = new List<Job>();
            JobData jobData = new JobData();

            List<Job> jobsFound = jobData.GetJobListByState(state);

            if (jobsFound.Count > 0)
            {
                jobs = jobsFound;
            }

            return jobs;
        }

        public List<SaGE.Correspondence.Data.Transaction> GetUpdateTransactionList(List<Job> jobs)
        {
            JobData jobData = new JobData();
            TransactionalData transactionData = new TransactionalData();

            List<SaGE.Correspondence.Data.Transaction> transactions = new List<SaGE.Correspondence.Data.Transaction>();

            foreach (Job job in jobs)
            {
                job.State = "updating client data";
                jobData.UpdateJob(job);

                transactions.AddRange(transactionData.GetTransactionsbyJob(job.KeyId));
            }

            return transactions;
        }
    }
}
