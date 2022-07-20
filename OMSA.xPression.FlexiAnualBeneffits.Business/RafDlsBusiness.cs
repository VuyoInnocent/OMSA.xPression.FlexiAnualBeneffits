using FileHelpers;
using OMSA.Gencorr.Common;
using OMSA.xPression.FlexiAnualBeneffits.Domain;
using OMSA.xPressions.Domain.RetailAffluent.DLS;
using SaGE.Common.Correspondence;
using SaGE.Common.Utils;
using SaGE.Correspondence.Data;
using SaGE.Correspondence.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Account = SaGE.Correspondence.Data.Account;
using Template = SaGE.Correspondence.Domain.Template;
using TransactionChannel = SaGE.Correspondence.Domain.TransactionChannel;

namespace OMSA.xPression.FlexiAnualBeneffits.Business
{
    public class RafDlsBusiness
    {
        FABRecord _fabRecors = new FABRecord();

        public bool ProcessTransactions(string inputFile, XmlSerializer headerSerializer, XmlSerializer packageSerializer)
        {
            _fabRecors = new FABRecord();
            xPressions.Domain.RetailAffluent.DLS.GenCorrLayouts genCorrLayouts = new xPressions.Domain.RetailAffluent.DLS.GenCorrLayouts();
            List<InitialTransaction> initialTransactionList = new List<InitialTransaction>();
            List<Template> templateList = LoadTemplateList(ConfigurationManager.AppSettings["templatelistpath"]);

            JobCommon jobCommon = new JobCommon();
            TransactionCommon transactionCommon = new TransactionCommon();

            AccountData accountData = new AccountData();
            Header header = new Header();

            int i = 0;
            int jobKey = 0;
            bool firstClient = true;

            string investorNumber = string.Empty;
            string serviceGroup = string.Empty;
            string serviceGroupV = string.Empty;
            string policyNumber = string.Empty;
            string productCode = string.Empty;
            string productInstance = string.Empty;
            string lateInterest = "False";
            string newGuid = string.Empty;

            string jobName = string.Empty;
            string account = string.Empty;

            FileUtils fileUtils = new FileUtils();
            fileUtils.SanitiseMainframeXML(inputFile);

            
            using (TextReader sr = new StreamReader(inputFile))
            {
                using (XmlReader reader = XmlReader.Create(sr))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.Equals("GENCORR"))
                                {
                                    using (StringReader elementReader = new StringReader(reader.ReadOuterXml()))
                                    {
                                        GenCorrLayoutsGENCORR genCorrLayoutsGencorr =
                                                 (GenCorrLayoutsGENCORR)headerSerializer.Deserialize(elementReader);
                                        genCorrLayouts.GENCORR = genCorrLayoutsGencorr;
                                        if (firstClient)
                                        {
                                            jobKey = jobCommon.AddJob(genCorrLayoutsGencorr.JOBNAME, inputFile, DateTime.Now, DateTime.Now, "SaGE.RAF.DLS.Pickup", "FAB WorkAround");

                                            jobName = genCorrLayoutsGencorr.JOBNAME;
                                            account = genCorrLayoutsGencorr.ACCOUNT;
                                            firstClient = false;
                                        }
                                    }
                                }
                                if (reader.Name.Equals("PACKAGE"))
                                {
                                    try
                                    {
                                        string packageData = reader.ReadOuterXml();

                                        using (StringReader elementReader = new StringReader(packageData))
                                        {
                                            i++;
                                            Console.WriteLine("Processing: {0}", i);
                                            SaGE.Correspondence.Domain.Transaction corrTransaction = new SaGE.Correspondence.Domain.Transaction();
                                            List<TransactionChannel> transactionChannels = new List<TransactionChannel>();
                                            TransactionChannel transactionChannel = null;

                                            string emailDataString = string.Empty;

                                            GenCorrLayoutsPACKAGE genCorrLayoutsPackage = (GenCorrLayoutsPACKAGE)packageSerializer.Deserialize(elementReader);
                                            GenCorrLayoutsPACKAGELOGGING genCorrLayoutsPackageLogging = new GenCorrLayoutsPACKAGELOGGING();

                                            foreach (
                                               GenCorrLayoutsPACKAGEDOCUMENT genCorrLayoutsPackagedocument in
                                                   genCorrLayoutsPackage.DOCUMENT)
                                            {

                                                corrTransaction.Guid = genCorrLayoutsPackage.DS_DCTM_PDF_ID;
                                                corrTransaction.DocSetCode = genCorrLayoutsPackagedocument.P_DOC_SET;
                                                corrTransaction.JobName = genCorrLayoutsPackage.JOBNAME.Trim();
                                                corrTransaction.JobId = jobKey;
                                                corrTransaction.Account = genCorrLayoutsPackage.ACCOUNT;
                                                genCorrLayoutsPackageLogging.DOC_SET_CODE =
                                                   genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DOC_SET_CODE;

                                                genCorrLayoutsPackageLogging.CS_CODE2 =
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_CODE2;

                                                genCorrLayoutsPackageLogging.CS_CODE3 =
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_CODE3;

                                                genCorrLayoutsPackage.LANGUAGE =
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.LANGUAGE;

                                                genCorrLayoutsPackage.JOBNAME = genCorrLayouts.GENCORR.JOBNAME;

                                                genCorrLayoutsPackage.ACCOUNT = genCorrLayouts.GENCORR.ACCOUNT;

                                                genCorrLayoutsPackage.JURISDICTION =
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.JURISDICTION;

                                                genCorrLayoutsPackage.EFFECTIVE_DATE =
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.EFFECTIVE_DATE;
                                                //VuyosLogic
                                                GenCorrLayoutsPACKAGEDOCUMENT[] document = genCorrLayoutsPackage.DOCUMENT;
                                                int jrdr = document.ToList().Count();
                                                for (int crdr = 0; crdr <= jrdr - 1; crdr++)
                                                {
                                                    if ((document[crdr].DLSCUSTOMER.CS_DOC_SET_CODE.TrimEnd() == ConfigurationManager.AppSettings["RDR_CS_DOC_SET_CODE"]) && (document[crdr].DLSCUSTOMER.CS_ACC1 != null))
                                                    {
                                                        document[crdr].DLSCUSTOMER.CS_ACC1 = ConfigurationManager.AppSettings["RDRFilePath"] + document[crdr].DLSCUSTOMER.CS_ACC1 + document[crdr].DLSCUSTOMER.CS_ACC2 + ".pdf";
                                                    }
                                                }

                                                if (
                                                    !string.IsNullOrEmpty(
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_NUM_FLAG))
                                                {
                                                    newGuid = genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_PDF_ID;
                                                    genCorrLayoutsPackageLogging.GCS_CALL = genCorrLayoutsPackage.LOGGING.GCS_CALL;
                                                    genCorrLayoutsPackageLogging.SOURCE_STATE = genCorrLayoutsPackage.LOGGING.SOURCE_STATE;
                                                    genCorrLayoutsPackage.DS_DCTM_BUS_AREA =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_BUS_AREA;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_BUS_AREA = null;

                                                    genCorrLayoutsPackage.DS_DCTM_DATE =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_DATE;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_DATE = null;

                                                    genCorrLayoutsPackage.DS_DCTM_DOC_PACK_CODE =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_DOC_PACK_CODE;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_DOC_PACK_CODE =
                                                        null;

                                                    genCorrLayoutsPackage.DS_DCTM_FORMAT =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_FORMAT;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_FORMAT = null;

                                                    genCorrLayoutsPackage.DS_DCTM_LOB =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_LOB;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_LOB = null;

                                                    genCorrLayoutsPackage.DS_DCTM_NUMBER =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_NUMBER.
                                                            TrimStart(
                                                                '0');
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_NUMBER = null;

                                                    genCorrLayoutsPackage.DS_DCTM_NUM_FLAG =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_NUM_FLAG;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_NUM_FLAG = null;

                                                    genCorrLayoutsPackage.DS_DCTM_PDF_ID = newGuid;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_PDF_ID = null;

                                                    genCorrLayoutsPackage.DS_DCTM_PROCESS =
                                                        genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_PROCESS;
                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_DCTM_PROCESS = null;

                                                    genCorrLayoutsPackage.DS_DCTM_PERMISSION = genCorrLayoutsPackage.DS_DCTM_PERMISSION;

                                                    genCorrLayoutsPackage.DS_DCTM_CATEGORY = genCorrLayoutsPackage.DS_DCTM_CATEGORY;

                                                    genCorrLayoutsPackage.DS_DCTM_DESCRIPTION = genCorrLayoutsPackage.DS_DCTM_DESCRIPTION;

                                                    genCorrLayoutsPackage.DS_TMPLT_GUID = newGuid;
                                                    genCorrLayoutsPackage.DS_AWD_ACCOUNT = genCorrLayoutsPackage.DS_AWD_ACCOUNT;
                                                    genCorrLayoutsPackage.CH_ARCHIVE = genCorrLayoutsPackage.CH_ARCHIVE;
                                                    genCorrLayoutsPackage.CH_EMAIL = genCorrLayoutsPackage.CH_EMAIL;
                                                    genCorrLayoutsPackage.DS_PDF_ID = genCorrLayoutsPackage.DS_PDF_ID;

                                                    genCorrLayoutsPackageLogging.TITLE = genCorrLayoutsPackage.LOGGING.TITLE;
                                                    genCorrLayoutsPackageLogging.SURNAME = genCorrLayoutsPackage.LOGGING.SURNAME;
                                                    genCorrLayoutsPackageLogging.FIRSTNAME = genCorrLayoutsPackage.LOGGING.FIRSTNAME;
                                                    genCorrLayoutsPackageLogging.CLIENT_STATUS = genCorrLayoutsPackage.LOGGING.CLIENT_STATUS;
                                                    genCorrLayoutsPackageLogging.DATE_OF_BIRTH = genCorrLayoutsPackage.LOGGING.DATE_OF_BIRTH;
                                                    genCorrLayoutsPackageLogging.GCS_ID = genCorrLayoutsPackage.LOGGING.GCS_ID;
                                                    genCorrLayoutsPackageLogging.ID_NO = genCorrLayoutsPackage.LOGGING.ID_NO;
                                                    

                                                    string businessArea = genCorrLayoutsPackage.DS_DCTM_BUS_AREA;

                                                    if (!string.IsNullOrEmpty(genCorrLayoutsPackage.LOGGING.CHANNEL))
                                                    {
                                                        if (genCorrLayoutsPackage.LOGGING.CHANNEL.ToUpper() == "EMAIL")
                                                        {
                                                            corrTransaction.ChannelId = 3;
                                                            genCorrLayoutsPackageLogging.CHANNEL = genCorrLayoutsPackage.LOGGING.CHANNEL;

                                                            EmailMeta email = new EmailMeta();
                                                            TemplateCommon template = new TemplateCommon();

                                                            Email emailData = template.GetEmailData(jobName);

                                                            if (emailData != null)
                                                            {
                                                                genCorrLayoutsPackage.CH_EMAIL = genCorrLayoutsPackage.CH_EMAIL;
                                                                genCorrLayoutsPackage.CH_PRINT = genCorrLayoutsPackage.CH_PRINT;
                                                                genCorrLayoutsPackage.DS_EMAIL_FROM_ADDRESS = "official_documents@oldmutual.com";
                                                               
                                                                genCorrLayoutsPackage.DS_EMAIL_SUBJECT = genCorrLayoutsPackage.DS_EMAIL_SUBJECT;
                                                                genCorrLayoutsPackage.DS_EMAIL_TO_ADDRESS = genCorrLayoutsPackage.DS_EMAIL_TO_ADDRESS;
                                                                genCorrLayoutsPackage.DS_EMAIL_PASSWORD = genCorrLayoutsPackage.DS_EMAIL_PASSWORD;


                                                                string language = genCorrLayoutsPackage.LANGUAGE == "ENG" ? "ENG" : "AFR";
                                                                if (language == "ENG")
                                                                {
                                                                    email.Template = templateList.FirstOrDefault(a => a.KeyId == emailData.EngTemplateId.Value.ToString()).TemplateName;
                                                                    email.FromAddressFriendly = "Old Mutual";
                                                                    email.PDFFriendly = "Annual Statement_Flexi and earlier product ranges.pdf";
                                                                }
                                                                else if (language == "AFR")
                                                                {
                                                                    email.Template = templateList.FirstOrDefault(a => a.KeyId == emailData.AfrTemplateId.Value.ToString()).TemplateName;
                                                                    email.FromAddressFriendly = "Old Mutual";
                                                                    email.PDFFriendly = "Jaarstaat vir Flexi-en vorige produkreekse.pdf";
                                                                }

                                                                email.Subject = language == "ENG" ? "Annual Statement: Flexi and earlier product ranges" : "Jaarstaat vir Flexi- en vorige produkreekse";
                                                                email.FromAddress = "official_documents@oldmutual.com";
                                                                email.ToAddress = genCorrLayoutsPackage.DS_EMAIL_TO_ADDRESS.ToLower();
                                                                email.PDF = jobName;
                                                                email.NPS = genCorrLayoutsPackage.DS_EMAIL_NPS;

                                                                EmailSearchReplace emailsearchreplace = new EmailSearchReplace();
                                                                List<EmailSearchReplace> emailsearchreplaces = new List<EmailSearchReplace>();

                                                                emailsearchreplace.Tag = "Title";
                                                                emailsearchreplace.Value = genCorrLayoutsPackage.DS_TMPLT_TITLE;
                                                                emailsearchreplaces.Add(emailsearchreplace);

                                                                emailsearchreplace = new EmailSearchReplace();
                                                                emailsearchreplace.Tag = "Surname";
                                                                emailsearchreplace.Value = genCorrLayoutsPackage.DS_TMPLT_SURNAME;
                                                                emailsearchreplaces.Add(emailsearchreplace);

                                                                emailsearchreplace = new EmailSearchReplace();
                                                                emailsearchreplace.Tag = "ReferenceNumber";
                                                                emailsearchreplace.Value = genCorrLayoutsPackage.DS_DCTM_NUM_FLAG + genCorrLayoutsPackage.DS_DCTM_NUMBER;
                                                                emailsearchreplaces.Add(emailsearchreplace);

                                                                email.SearchReplace = emailsearchreplaces.ToArray();
                                                                emailDataString = SerializationHelper.XmlSerialize(email);
                                                            }



                                                        }
                                                        if (genCorrLayoutsPackage.LOGGING.CHANNEL.ToUpper() == "PRINT")
                                                        {
                                                            corrTransaction.ChannelId = 2;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        corrTransaction.ChannelId = 1;

                                                    } 
                                                    genCorrLayoutsPackage.DS_PRINT_ACCOUNT = genCorrLayoutsPackage.DS_PRINT_ACCOUNT;
                                                    genCorrLayoutsPackage.DS_SFTP_ACCOUNT = genCorrLayoutsPackage.DS_PRINT_ACCOUNT;
                                                    genCorrLayoutsPackage.DS_SMS_NUMBER = genCorrLayoutsPackage.DS_SMS_NUMBER;
                                                    genCorrLayoutsPackage.DS_TMPLT_FNAME = genCorrLayoutsPackage.DS_TMPLT_FNAME;
                                                    genCorrLayoutsPackage.DS_TMPLT_INIT = genCorrLayoutsPackage.DS_TMPLT_INIT;
                                                    genCorrLayoutsPackage.DS_TMPLT_SURNAME = genCorrLayoutsPackage.DS_TMPLT_SURNAME;
                                                    genCorrLayoutsPackage.DS_TMPLT_TITLE = genCorrLayoutsPackage.DS_TMPLT_TITLE;
                                                    genCorrLayoutsPackage.DS_TMPLT_LANGUAGE = genCorrLayoutsPackage.LANGUAGE;
                                                    genCorrLayoutsPackage.DS_TMPLT_TYPE = genCorrLayoutsPackage.DS_TMPLT_TYPE;
                                                    genCorrLayoutsPackage.DS_PDF_PATH = genCorrLayoutsPackage.DS_PDF_PATH;

                                                    genCorrLayoutsPackageLogging.BUSINESS_AREA = genCorrLayoutsPackage.LOGGING.BUSINESS_AREA;

                                                    genCorrLayoutsPackageLogging.AGREEMENT_CLIENT_ID = genCorrLayoutsPackage.LOGGING.AGREEMENT_CLIENT_ID;

                                                    genCorrLayoutsPackageLogging.AGREEMENT_CLIENT_TYPE = genCorrLayoutsPackage.LOGGING.AGREEMENT_CLIENT_TYPE;

                                                    genCorrLayoutsPackageLogging.DESCRIPTION = genCorrLayoutsPackage.LOGGING.DESCRIPTION;

                                                    genCorrLayoutsPackageLogging.LOB_ID = genCorrLayoutsPackage.LOGGING.LOB_ID;

                                                    genCorrLayoutsPackageLogging.PDF_NAME = genCorrLayoutsPackage.LOGGING.PDF_NAME;

                                                    genCorrLayoutsPackageLogging.EFFECTIVE_DATE = genCorrLayoutsPackage.LOGGING.EFFECTIVE_DATE;

                                                    genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_CODE10 = genCorrLayoutsPackagedocument.DLSCUSTOMER.CS_CODE10;

                                                    
                                                    genCorrLayoutsPackageLogging.CONTACT_DETAIL = genCorrLayoutsPackage.LOGGING.CONTACT_DETAIL;
                                                    genCorrLayoutsPackageLogging.ARCHIVE = genCorrLayoutsPackage.LOGGING.ARCHIVE;

                                                    genCorrLayoutsPackagedocument.PACKAGEID = genCorrLayoutsPackagedocument.PACKAGEID;
                                                    genCorrLayoutsPackage.LOGGING = genCorrLayoutsPackageLogging;


                                                    string genCorrPackageXml = SerializationHelper.XmlSerialize(genCorrLayoutsPackage);
                                                    corrTransaction.Guid = newGuid;
                                                    corrTransaction.JobId = jobKey;
                                                    corrTransaction.JobName = jobName;
                                                    corrTransaction.LobId = genCorrLayoutsPackage.DS_DCTM_LOB;
                                                    corrTransaction.LobKey = genCorrLayoutsPackage.DS_DCTM_NUM_FLAG;
                                                    corrTransaction.State = "VALID";
                                                    corrTransaction.Account = account;
                                                    corrTransaction.ClientId = genCorrLayoutsPackage.DS_DCTM_NUMBER;

                                                    InitialTransaction initialTransaction = new InitialTransaction();

                                                    initialTransaction.Transaction = corrTransaction;
                                                    initialTransaction.PackageData = genCorrPackageXml;
                                                    initialTransaction.EmailData = emailDataString;
                                                    initialTransaction.TransactionChannels = transactionChannels;

                                                    initialTransactionList.Add(initialTransaction);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        Console.WriteLine(ex.Message + "-" + ex.StackTrace);
                                        return false;
                                    }
                                }

                                    break;
                        }
                    }
                }
            }

            ProcessInitialTransactionList(jobKey, "official_documents@oldmutual.com", initialTransactionList);
            return true;
        }
        private void ProcessInitialTransactionList(int jobId, string replyToAddress, List<InitialTransaction> initialTransactionList)
        {
            var transactionGroup = initialTransactionList.GroupBy(row => row.Transaction.ChannelId);
            int recordCount = initialTransactionList.Count;
            int recordCounter = 0;

            try
            {
                foreach (IGrouping<int, InitialTransaction> initialTransactions in transactionGroup)
                {
                    int groupKey = initialTransactions.Key;
                    string channelDescription = String.Empty;
                    string jobName = String.Empty;
                    string account = String.Empty;
                    string transactionGUID = string.Empty;

                    string fileName = String.Empty;
                    switch (groupKey)
                    {
                        case 1:

                            channelDescription = "Archive";

                            break;

                        case 2:

                            channelDescription = "Print";

                            break;

                        case 3:

                            channelDescription = "Email";

                            break;

                        case 4:

                            channelDescription = "SMS";

                            break;

                    }
                    string batchBucket = Path.Combine(ConfigurationManager.AppSettings["batchbucket"], "BatchBucketBAU.in");

                    jobName = initialTransactions.FirstOrDefault().Transaction.JobName;
                    account = initialTransactions.FirstOrDefault().Transaction.Account;
                    transactionGUID = initialTransactions.FirstOrDefault().Transaction.Guid;
                    fileName = jobName + "_" + jobId + "_" + channelDescription + "_";

                    StringBuilder sbCustomerData = new StringBuilder();
                    StringBuilder sbEmailData = new StringBuilder();

                    int numberToTake = 10000;
                    int fileCounter = 0;
                    int transactionCount = initialTransactions.Count();


                    int index = 0;

                    while (index < transactionCount)
                    {
                        sbCustomerData = new StringBuilder();
                        sbEmailData = new StringBuilder();

                        //Append XML Header.
                        sbCustomerData.AppendLine("<GenCorrLayouts>");
                        sbCustomerData.AppendLine("<GENCORR>");
                        sbCustomerData.AppendLine("\t<ENGINE_TYPE>xPression</ENGINE_TYPE>");
                        sbCustomerData.AppendLine($"\t<JOBNAME>{jobName}</JOBNAME>");
                        sbCustomerData.AppendLine($"\t<ACCOUNT>{account}</ACCOUNT>");
                        sbCustomerData.AppendLine($"\t<CHANNEL>{channelDescription}</CHANNEL>");
                        sbCustomerData.AppendLine("\t<ARCHIVE>Yes</ARCHIVE>");
                        sbCustomerData.AppendLine("</GENCORR>");

                        foreach (InitialTransaction initialTransaction in initialTransactions.Skip(index).Take(numberToTake))
                        {
                            recordCounter++;

                            Console.WriteLine("Writing client " + recordCounter + " of " + recordCount);

                            sbCustomerData.AppendLine(initialTransaction.PackageData);

                            if (channelDescription == "Email" && !string.IsNullOrEmpty(initialTransaction.EmailData))
                            {
                                sbEmailData.AppendLine(initialTransaction.Transaction.Guid + "|" + replyToAddress + "|" + initialTransaction.EmailData.Replace("\r\n", ""));
                            }
                        }

                        sbCustomerData.AppendLine("</GenCorrLayouts>");

                        index += numberToTake;

                        string tempFileName = fileName;

                        tempFileName += $"{fileCounter:000}";

                        using (StreamWriter swCustomerData = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["customerdata"], tempFileName + ".xml")))
                        {
                            swCustomerData.Write(sbCustomerData.ToString());
                        }

                        StringBuilder sbOverride = new StringBuilder();

                        sbOverride.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        sbOverride.Append("<JobDatasourceOverride>");
                        sbOverride.Append("<JobStepDatasourceOverride jobStepID=\"0"
                                          + "\" jobStepName=\"Printing"
                                          + "\" definedAssembleDatasource=\"" + "CTSTAXIT_DS"
                                          + "\" definedTriggerDatasource=\""
                                          + "\" overridenAssembleDatasource=\"" + "CTSTAXIT_DS"
                                          + "\" overridenAssembleDataFile=\"" + Path.Combine(ConfigurationManager.AppSettings["overridecustomerdata"], tempFileName + ".xml")
                                          + "\" overridenTriggerDatasource=\""
                                          + "\" overridenTriggerDataFile=\"\" />");
                        sbOverride.Append("</JobDatasourceOverride>");

                        using (StreamWriter swOverride = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["override"], tempFileName + ".xml")))
                        {
                            swOverride.Write(sbOverride.ToString());
                        }

                        int i = 0;

                        while (IsFileLocked(batchBucket) && i < 10)
                        {
                            i++;
                            Thread.Sleep(1000);
                        }

                        if (channelDescription == "Email")
                        {
                            using (StreamWriter swEmailMeta = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["emailmeta"], tempFileName + ".txt")))
                            {
                                swEmailMeta.Write(sbEmailData.ToString());
                            }
                        }

                        fileCounter += 1;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
        }
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (Exception e)
            {
                return true;
            }

            return false;
        }

        private List<Template> LoadTemplateList(string filePath)
        {
            List<Template> templateList = new List<Template>();

            FileHelperEngine<Template> templateEngine = new FileHelperEngine<Template>();

            templateList = templateEngine.ReadFileAsList(filePath);

           

            return templateList;
        }
    }
}
