using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Data
{

    public class BaseDAL : IDisposable
    {
      
        private SaGECorrespondenceEntities _context;
        private string _errorMessage;

        public  SaGECorrespondenceEntities Context
        {
            get
            {
                if (_context == null || disposed == true)
                {
                    _context = new SaGECorrespondenceEntities();
                    this.disposed = false;
                }

                return _context;
            }
        }


        public bool Save(bool dispose =false)
        {
            try
            {
                Context.SaveChanges();
               
                if (dispose)
                    Dispose();
                
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message + ex.StackTrace;
                return false;
            }
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(Context);
        }

  
    }





}
