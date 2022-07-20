using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using SaGE.Common.Logging;

namespace SaGE.Common.Utils
{
    public class  FileUtils //: LoggerBase
    {
        //protected readonly ILog Log = LogManager.GetLogger(typeof(FileUtils));
        protected readonly string TraceHeader = "SaGE.Common.Utils::FileUtils.";

        public string PrepAndArchiveFile(string downloadFile, string archivePath, string processPath)
        {
            string processFile = string.Empty;
            string archiveMonth = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                archivePath = Path.Combine(archivePath, archiveMonth);
                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }
                
                Trace.WriteLine(TraceHeader + "ArchiveFile - " + "Do some housework");
                Trace.WriteLine(TraceHeader + "ArchiveFile - " + "Copying file to archive folder");

                string oldArchiveFilePath = Path.Combine(archivePath, Path.GetFileName(downloadFile));
                string archiveFilePath = oldArchiveFilePath;

                int k = 1;
                while (File.Exists(archiveFilePath))
                {
                    string directoryName = Path.GetDirectoryName(archiveFilePath);
                    string fileName = Path.GetFileNameWithoutExtension(oldArchiveFilePath) + "." + k;
                    string fileExt = Path.GetExtension(archiveFilePath);
                    archiveFilePath = Path.Combine(directoryName, fileName + fileExt);
                    k++;
                }

                File.Copy(downloadFile, archiveFilePath);
                File.Copy(downloadFile, Path.Combine(processPath, Path.GetFileName(downloadFile)), true);
                Trace.WriteLine(TraceHeader + "FileProcess - " + "Deleting server file and trigger");

                File.Delete(downloadFile);
                string downloadPath = Path.GetDirectoryName(downloadFile);
                File.Delete(Path.Combine(downloadPath, Path.GetFileNameWithoutExtension(downloadFile) + ".trg"));
                processFile = Path.Combine(processPath, Path.GetFileName(downloadFile));

            }
            catch (Exception ex)
            {
                throw;
            }
         
            return processFile;
        }

        public void SanitiseMainframeXML(string inputFile)
        {
            string strTempFile = Path.GetTempFileName();

            //Encoding encoding = GetFileEncoding(inputFile);

            using (StreamReader sr = new StreamReader(inputFile))
            {
                using (StreamWriter sw = new StreamWriter(strTempFile, false, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        StringBuilder sbNewLine = new StringBuilder();

                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] words = line.Split(' ');

                            foreach (string word in words)
                            {
                                string newWord = word;
                                newWord = newWord.Contains("&#160;") ? newWord.Replace("&#160;", " ") : newWord;
                                newWord = newWord.Contains("&nbsp;") ? newWord.Replace("&nbsp;", "\u00a0") : newWord;
                                newWord = newWord.Contains("&<") ? newWord.Replace("&<", "&amp;<") : newWord;
                                newWord = newWord.Contains("&") ? newWord.Replace("&", "&amp;") : newWord;
                                newWord = newWord.Contains("Â") ? newWord.Replace("Â", string.Empty) : newWord;
                                newWord = newWord.Contains('`') ? newWord.Replace('`', ' ') : newWord;
                                newWord = newWord.Contains('~') ? newWord.Replace('~', ' ') : newWord;
                                newWord = newWord.Contains('\0') ? newWord.Replace('\0', ' ') : newWord;


                                sbNewLine.Append(newWord + ' ');
                                line = sbNewLine.ToString().Trim();
                            }
                        }

                        sw.WriteLine(line);
                    }
                }
            }

            //Shift files around.
            File.Delete(inputFile);
            File.Move(strTempFile, inputFile);
            File.Delete(strTempFile);
        }

        private Encoding GetFileEncoding(string inputFile)
        {
            using (var r = new StreamReader(inputFile, Encoding.Default))
            {
                string s = r.ReadLine();

                return r.CurrentEncoding;
            }
        }
    }
}
