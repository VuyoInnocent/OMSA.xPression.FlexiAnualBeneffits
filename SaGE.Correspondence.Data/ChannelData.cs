using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class ChannelData
    {
        public void AddChannelSequence(ChannelSequence channelSequence)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelSequence channelSequenceFound = db.ChannelSequences.FirstOrDefault(a => a.JobDefId == channelSequence.JobDefId
                    && a.CurrentChannelId == channelSequence.CurrentChannelId);
                    //&& channelSequence.NextChannelIdSuccess.HasValue ? a.NextChannelIdSuccess == channelSequence.NextChannelIdSuccess : a.NextChannelIdSuccess == null
                    //&& channelSequence.NextChannelIdFail.HasValue ? a.NextChannelIdFail == channelSequence.NextChannelIdFail : a.NextChannelIdFail == null);

                if (channelSequenceFound == null)
                {
                    db.AddToChannelSequences(channelSequence);
                    db.SaveChanges();
                }

                //return channelSequence.KeyId;
            }
        }

        public void UpdateChannelSequence(ChannelSequence channelSequence)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelSequence channelSequenceFound = db.ChannelSequences.FirstOrDefault(a => a.KeyId == channelSequence.KeyId);

                if (channelSequenceFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemoveChannelSequence(ChannelSequence channelSequence)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                ChannelSequence channelSequenceFound = db.ChannelSequences.FirstOrDefault(a => a.KeyId == channelSequence.KeyId);

                if (channelSequenceFound != null)
                {
                    db.DeleteObject(channelSequenceFound);
                    db.SaveChanges();
                }
            }
        }
        
        public int AddChannel(Channel channel)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Channel channelFound = db.Channels.FirstOrDefault(a => a.Description == channel.Description);

                if (channelFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToChannels(channel);
                    db.SaveChanges();
                }

                return channel.KeyId;
            }
        }

        public int UpdateChannel(Channel channel)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Channel channelFound = db.Channels.FirstOrDefault(a => a.Description == channel.Description);

                if (channelFound != null)
                {
                    db.SaveChanges();
                }

                return channel.KeyId;
            }
        }

        public void RemoveChannel(Channel channel)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                Channel channelFound = db.Channels.FirstOrDefault(a => a.Description == channel.Description);

                if (channelFound != null)
                {
                    db.DeleteObject(channelFound);
                    db.SaveChanges();
                }
            }
        }

        public List<Channel> GetChannels()
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Channels.ToList();
            }
        }

        public List<ChannelSequence> GetChannelSequencesByJobDefinition(string jobName)
        {
            List<ChannelSequence> channelSequencesFound = new List<ChannelSequence>();

            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                JobDefinition jobDefinition = db.JobDefinitions.FirstOrDefault(a => a.JobName == jobName);
                
                if (jobDefinition != null)
                {
                    int jobDefId = jobDefinition.KeyId;

                    channelSequencesFound =  new List<ChannelSequence>(db.ChannelSequences.Where(a => a.JobDefId == jobDefId).ToList().OrderBy(a=>a.KeyId));
                }
            }

            return channelSequencesFound;
        }

        public Channel GetChannel(int channelId)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                return db.Channels.FirstOrDefault(a => a.KeyId == channelId);
            }
        }
    }
}
