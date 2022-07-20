using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaGE.Correspondence.Data;

namespace SaGE.Common.Correspondence
{
    public class TemplateCommon
    {
        public Email GetEmailData(string JobName)
        {
            TemplateData templateData = new TemplateData();

            return templateData.GetEmailData(JobName);
        }

        public Template GetTemplate(int templateId)
        {
            TemplateData templateData = new TemplateData();

            return templateData.GetTemplate(templateId);
        }
    }
}
