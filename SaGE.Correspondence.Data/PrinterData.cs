using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{
    public class PrinterData
    {
        public int AddPrinterDetail(PrinterDetail printerDetail)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrinterDetail printerDetailFound = db.PrinterDetails.FirstOrDefault(a => a.KeyId == printerDetail.KeyId);

                if (printerDetailFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToPrinterDetails(printerDetail);
                    db.SaveChanges();
                }

                return printerDetail.KeyId;
            }
        }

        public void UpdatePrinterDetail(PrinterDetail printerDetail)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrinterDetail printerDetailFound = db.PrinterDetails.FirstOrDefault(a => a.KeyId == printerDetail.KeyId);

                if (printerDetailFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public int RemovePrinterDetail(PrinterDetail printerDetail)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrinterDetail printerDetailFound = db.PrinterDetails.FirstOrDefault(a => a.KeyId == printerDetail.KeyId);

                if (printerDetailFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToPrinterDetails(printerDetail);
                    db.SaveChanges();
                }

                return printerDetail.KeyId;
            }
        }



        public int AddPrinterTray(PrinterTray printerTray)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrinterTray printerTrayFound = db.PrinterTrays.FirstOrDefault(a => a.KeyId == printerTray.KeyId);

                if (printerTrayFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToPrinterTrays(printerTray);
                    db.SaveChanges();
                }

                return printerTray.KeyId;
            }
        }

        public void UpdatePrinterTray(PrinterTray printerTray)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrinterTray printerTrayFound = db.PrinterTrays.FirstOrDefault(a => a.KeyId == printerTray.KeyId);

                if (printerTrayFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemovePrinterTray(PrinterTray printerTray)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrinterTray printerTrayFound = db.PrinterTrays.FirstOrDefault(a => a.KeyId == printerTray.KeyId);

                if (printerTrayFound != null)
                {
                    db.DeleteObject(printerTrayFound);
                    db.SaveChanges();
                }
            }
        }
        


        public int AddPrintSupplier(PrintSupplier printSupplier)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrintSupplier printSupplierFound = db.PrintSuppliers.FirstOrDefault(a => a.KeyId == printSupplier.KeyId);

                if (printSupplierFound != null)
                {
                    db.SaveChanges();
                }
                else
                {
                    db.AddToPrintSuppliers(printSupplier);
                    db.SaveChanges();
                }

                return printSupplier.KeyId;
            }
        }

        public void UpdatePrintSupplier(PrintSupplier printSupplier)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrintSupplier printSupplierFound = db.PrintSuppliers.FirstOrDefault(a => a.KeyId == printSupplier.KeyId);

                if (printSupplierFound != null)
                {
                    db.SaveChanges();
                }
            }
        }

        public void RemovePrintSupplier(PrintSupplier printSupplier)
        {
            using (SaGECorrespondenceEntities db = new SaGECorrespondenceEntities())
            {
                PrintSupplier printSupplierFound = db.PrintSuppliers.FirstOrDefault(a => a.KeyId == printSupplier.KeyId);

                if (printSupplierFound != null)
                {
                    db.DeleteObject(printSupplierFound);
                    db.SaveChanges();
                }
            }
        }

    }
}
