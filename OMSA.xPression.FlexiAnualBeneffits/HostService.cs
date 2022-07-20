using OMSA.xPression.FlexiAnualBeneffits.Business;
using OMSA.xPressions.Domain.RetailAffluent.DLS;
using SaGE.Common.NTService;
using SaGE.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OMSA.xPression.FlexiAnualBeneffits
{
    public partial class HostService : NTService
    {
        private bool _processRun = true;

        public bool ProcessRun
        {
            get { return _processRun; }
        }

        protected override void AfterStart()
        {
            FileProcess();

            base.AfterStart();
        }

        protected override void OnStop()
        {
            _processRun = false;
        }

        public void FileProcess()
        {
            RafDlsBusiness rafDlsBusiness = new RafDlsBusiness();

            var headerAttribute = new XmlRootAttribute();
            headerAttribute.ElementName = "GENCORR";
            headerAttribute.IsNullable = true;

            XmlSerializer headerSerializer =
                    new XmlSerializer(typeof(GenCorrLayoutsGENCORR), headerAttribute);

            //XmlSerializer headerSerializer = new XmlSerializer(typeof(GenCorrLayouts), headerAttribute);

            var packageAttribute = new XmlRootAttribute();
            packageAttribute.ElementName = "PACKAGE";
            packageAttribute.IsNullable = true;

            XmlSerializer packageSerializer = new XmlSerializer(typeof(GenCorrLayoutsPACKAGE), packageAttribute);

            FileUtils fileUtils = new FileUtils();

            string downloadPath = ConfigurationManager.AppSettings["downloadpath"];
            string archivePath = ConfigurationManager.AppSettings["archivepath"];
            string processPath = ConfigurationManager.AppSettings["processpath"];
            string errorArchivePath = ConfigurationManager.AppSettings["errorarchivepath"];


            foreach (string fileName in Directory.GetFiles(downloadPath))
            {
                try
                {
                    string file = Path.ChangeExtension(fileName, ConfigurationManager.AppSettings["filetype"]);

                    string processFile = fileUtils.PrepAndArchiveFile(file, archivePath, processPath);

                    bool rc = rafDlsBusiness.ProcessTransactions(processFile, headerSerializer, packageSerializer);

                    if (!rc)
                    {
                        if (!Directory.Exists(errorArchivePath))
                        {
                            Directory.CreateDirectory(errorArchivePath);
                        }

                        File.Copy(processFile, Path.Combine(errorArchivePath, Path.GetFileName(processFile)), true);

                        File.Delete(processFile);
                    }

                    Thread.Sleep(0);

                    File.Delete(processFile);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message + " " + ex.StackTrace);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
