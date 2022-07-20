using System;
using System.Runtime.InteropServices;

namespace SaGE.Common.NTService
{
    /// <summary>
    /// Summary description for NTService.
    /// </summary>
    public class NTService : System.ServiceProcess.ServiceBase
    {
        [DllImport("advapi32.dll")]
        public static extern int LockServiceDatabase(int hSCManager);

        [DllImport("advapi32.dll")]
        public static extern bool UnlockServiceDatabase(int hSCManager);

        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr pDst, SC_ACTION[] pSrc, int ByteLen);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int OpenSCManager(string lpMachineName, string lpDatabaseName, ServiceControlManagerType dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int DeleteService(int hService);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int OpenService(int hSCManager, string lpServiceName, ACCESS_TYPE dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int CloseServiceHandle(int hSCObject);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int CreateService(int hSCManager, string lpServiceName,
                                                string lpDisplayName, ACCESS_TYPE dwDesiredAccess,
                                                ServiceType dwServiceType, ServiceStartType dwStartType, ServiceErrorControl dwErrorControl,
                                                string lpBinaryPathName, string lpLoadOrderGroup, int lpdwTagId, string lpDependencies,
                                                String lpServiceStartName, String lpPassword);

        [DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private unsafe static extern int FormatMessage(int dwFlags, ref IntPtr lpSource,
                                                       int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, IntPtr* Arguments);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool ChangeServiceConfig2(int hService, InfoLevel dwInfoLevel, ref SERVICE_DESCRIPTION lpInfo);

        [DllImport("advapi32.dll")]
        public static extern bool ChangeServiceConfig2(int hService, InfoLevel dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_FAILURE_ACTIONS lpInfo);

        public enum InfoLevel
        {
            SERVICE_CONFIG_DESCRIPTION = 0x1,
            SERVICE_CONFIG_FAILURE_ACTIONS = 0x2
        }

        public struct SERVICE_DESCRIPTION
        {
            public string lpDescription;
        }


        public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        //public const int GENERIC_READ = 0x80000000;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int SERVICE_NO_CHANGE = 0xFFFFFFF;

        public enum ServiceType
        {
            SERVICE_KERNEL_DRIVER = 0x1,
            SERVICE_FILE_SYSTEM_DRIVER = 0x2,
            SERVICE_WIN32_OWN_PROCESS = 0x10,
            SERVICE_WIN32_SHARE_PROCESS = 0x20,
            SERVICE_INTERACTIVE_PROCESS = 0x100,
            SERVICETYPE_NO_CHANGE = SERVICE_NO_CHANGE
        }

        public enum ServiceStartType
        {
            SERVICE_BOOT_START = 0x0,
            SERVICE_SYSTEM_START = 0x1,
            SERVICE_AUTO_START = 0x2,
            SERVICE_DEMAND_START = 0x3,
            SERVICE_DISABLED = 0x4,
            SERVICESTARTTYPE_NO_CHANGE = SERVICE_NO_CHANGE
        }

        public enum ServiceErrorControl
        {
            SERVICE_ERROR_IGNORE = 0x0,
            SERVICE_ERROR_NORMAL = 0x1,
            SERVICE_ERROR_SEVERE = 0x2,
            SERVICE_ERROR_CRITICAL = 0x3,
            SERVICEERRORCONTROL_NO_CHANGE = SERVICE_NO_CHANGE
        }

        public enum ServiceStateRequest
        {
            SERVICE_ACTIVE = 0x1,
            SERVICE_INACTIVE = 0x2,
            SERVICE_STATE_ALL = (SERVICE_ACTIVE + SERVICE_INACTIVE)
        }

        public enum ServiceControlType
        {
            SERVICE_CONTROL_STOP = 0x1,
            SERVICE_CONTROL_PAUSE = 0x2,
            SERVICE_CONTROL_CONTINUE = 0x3,
            SERVICE_CONTROL_INTERROGATE = 0x4,
            SERVICE_CONTROL_SHUTDOWN = 0x5,
            SERVICE_CONTROL_PARAMCHANGE = 0x6,
            SERVICE_CONTROL_NETBINDADD = 0x7,
            SERVICE_CONTROL_NETBINDREMOVE = 0x8,
            SERVICE_CONTROL_NETBINDENABLE = 0x9,
            SERVICE_CONTROL_NETBINDDISABLE = 0xA,
            SERVICE_CONTROL_DEVICEEVENT = 0xB,
            SERVICE_CONTROL_HARDWAREPROFILECHANGE = 0xC,
            SERVICE_CONTROL_POWEREVENT = 0xD,
            SERVICE_CONTROL_SESSIONCHANGE = 0xE,
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x1,
            SERVICE_START_PENDING = 0x2,
            SERVICE_STOP_PENDING = 0x3,
            SERVICE_RUNNING = 0x4,
            SERVICE_CONTINUE_PENDING = 0x5,
            SERVICE_PAUSE_PENDING = 0x6,
            SERVICE_PAUSED = 0x7,
        }

        public enum ServiceControlAccepted
        {
            SERVICE_ACCEPT_STOP = 0x1,
            SERVICE_ACCEPT_PAUSE_CONTINUE = 0x2,
            SERVICE_ACCEPT_SHUTDOWN = 0x4,
            SERVICE_ACCEPT_PARAMCHANGE = 0x8,
            SERVICE_ACCEPT_NETBINDCHANGE = 0x10,
            SERVICE_ACCEPT_HARDWAREPROFILECHANGE = 0x20,
            SERVICE_ACCEPT_POWEREVENT = 0x40,
            SERVICE_ACCEPT_SESSIONCHANGE = 0x80
        }

        public enum ServiceControlManagerType
        {
            SC_MANAGER_CONNECT = 0x1,
            SC_MANAGER_CREATE_SERVICE = 0x2,
            SC_MANAGER_ENUMERATE_SERVICE = 0x4,
            SC_MANAGER_LOCK = 0x8,
            SC_MANAGER_QUERY_LOCK_STATUS = 0x10,
            SC_MANAGER_MODIFY_BOOT_CONFIG = 0x20,
            SC_MANAGER_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED +
                                    SC_MANAGER_CONNECT +
                                    SC_MANAGER_CREATE_SERVICE +
                                    SC_MANAGER_ENUMERATE_SERVICE +
                                    SC_MANAGER_LOCK +
                                    SC_MANAGER_QUERY_LOCK_STATUS +
                                    SC_MANAGER_MODIFY_BOOT_CONFIG
        }

        public enum ACCESS_TYPE
        {
            SERVICE_QUERY_CONFIG = 0x1,
            SERVICE_CHANGE_CONFIG = 0x2,
            SERVICE_QUERY_STATUS = 0x4,
            SERVICE_ENUMERATE_DEPENDENTS = 0x8,
            SERVICE_START = 0x10,
            SERVICE_STOP = 0x20,
            SERVICE_PAUSE_CONTINUE = 0x40,
            SERVICE_INTERROGATE = 0x80,
            SERVICE_USER_DEFINED_CONTROL = 0x100,
            SERVICE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED +
                                 SERVICE_QUERY_CONFIG +
                                 SERVICE_CHANGE_CONFIG +
                                 SERVICE_QUERY_STATUS +
                                 SERVICE_ENUMERATE_DEPENDENTS +
                                 SERVICE_START +
                                 SERVICE_STOP +
                                 SERVICE_PAUSE_CONTINUE +
                                 SERVICE_INTERROGATE +
                                 SERVICE_USER_DEFINED_CONTROL
        }

        public enum SC_ACTION_TYPE : int
        {
            SC_ACTION_NONE = 0,
            SC_ACTION_RESTART = 1,
            SC_ACTION_REBOOT = 2,
            SC_ACTION_RUN_COMMAND = 3,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SC_ACTION
        {
            public SC_ACTION_TYPE SCActionType;
            public int Delay;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SERVICE_FAILURE_ACTIONS
        {
            public int dwResetPeriod;
            public string lpRebootMsg;
            public string lpCommand;
            public int cActions;
            public int lpsaActions;
        }

        private System.Timers.Timer mTimer;

        SC_ACTION[] ScActions = new SC_ACTION[3];	//There should be one element for each

        IntPtr iScActionsPointer = new IntPtr();

        int schSCManager = 0;
        int schSCManagerLock = 0;
        int schService = 0;

        public NTService() // Constructor logic here.
        {
            mTimer = new System.Timers.Timer();
            mTimer.Interval = 2000;
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.tmr_Elapsed);
            mTimer.Enabled = false;
        }

        private void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer.Enabled = false;
            AfterStart();
        }

        private unsafe static string GetErrorMessage(int errorCode)
        {
            int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            int messageSize = 255;
            String lpMsgBuf = "";
            int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

            IntPtr ptrlpSource = IntPtr.Zero;
            IntPtr prtArguments = IntPtr.Zero;

            int retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0, ref lpMsgBuf, messageSize, &prtArguments);
            if (0 == retVal)
            {
                throw new System.Exception("Failed to format message for error code " + errorCode + ". ");
            }

            return lpMsgBuf;
        }

        protected virtual void AfterStart()
        {
        }

        private void OpenAndLock()
        {
            schSCManager = OpenSCManager(
                null,												// local machine 
                null,												// ServicesActive database 
                ServiceControlManagerType.SC_MANAGER_ALL_ACCESS);	// Access rights 

            //Check that it's open. If not throw an exception.
            if (schSCManager < 1)
            {
                throw new System.Exception("Unable to open the Services Manager.");
            }

            //Lock the Service Control Manager database.
            schSCManagerLock = LockServiceDatabase(schSCManager);

            //Check that it's locked. If not throw an exception.
            if (schSCManagerLock < 1)
            {
                throw new System.Exception("Unable to lock the Services Manager.");
            }

        }

        private void OpenService(string inServiceName)
        {
            //Obtain a handle to the relevant service, with appropriate rights.
            //This handle is sent along to change the settings. The second parameter
            //should contain the name you assign to the service.
            schService = OpenService(schSCManager, inServiceName,
                                     ACCESS_TYPE.SERVICE_ALL_ACCESS);

            //Check that it's open. If not throw an exception.
            if (schService < 1)
            {
                throw new System.Exception("Unable to open the Service for modification.");
            }

        }

        private void UnlockAndClose()
        {
            if (schService > 0)
            {
                CloseServiceHandle(schService);
            }

            if (schSCManagerLock > 0)
            {
                UnlockServiceDatabase(schSCManagerLock);
            }

            if (schSCManager != 0)
            {
                CloseServiceHandle(schSCManager);
            }

            CloseServiceHandle(schService);
        }

        public void Create(string BinaryPathName,
                           string ServiceName,
                           string ServiceDisplayName,
                           string Description,
                           ACCESS_TYPE AccessType,
                           ServiceType ServiceType,
                           ServiceStartType ServiceStart,
                           ServiceErrorControl ErrorControl,
                           String UserAccount,
                           String UserPassword)
        {
            int errorcode = 0;

            OpenAndLock();

            int schService = CreateService(
                schSCManager,					// SCManager database 
                ServiceName,					// name of service 
                ServiceDisplayName,				// service name to display 
                AccessType,						// desired access 
                ServiceType,					// service type 
                ServiceStart,					// start type 
                ErrorControl,					// error control type 
                BinaryPathName,					// service's binary 
                null,							// no load ordering group 
                0,								// no tag identifier 
                null,							// no dependencies 
                UserAccount,					// Account 
                UserPassword);					// Password 

            errorcode = Marshal.GetLastWin32Error();

            if (errorcode != 0 && errorcode != 997)
            {
                throw new ServiceException(GetErrorMessage(errorcode));
            }

            //OpenService(ServiceName);

            SERVICE_DESCRIPTION service_description;
            service_description.lpDescription = Description;

            bool bChangeServiceConfig2 = ChangeServiceConfig2(schService, InfoLevel.SERVICE_CONFIG_DESCRIPTION, ref service_description);

            errorcode = Marshal.GetLastWin32Error();

            if (errorcode != 0 && errorcode != 997)
            {
                throw new System.Exception(GetErrorMessage(errorcode));
            }

            UnlockAndClose();
        }

        public void SetFailActions(string ServiceName, int ResetPeriod, string Command, SC_ACTION[] inScActions)
        {
            SERVICE_FAILURE_ACTIONS ServiceFailureActions;

            OpenAndLock();

            OpenService(ServiceName);

            //To change the Service Failure Actions, create an instance of the
            //SERVICE_FAILURE_ACTIONS structure and set the members to your
            //desired values. See MSDN for detailed descriptions.
            ServiceFailureActions.dwResetPeriod = ResetPeriod; //600
            ServiceFailureActions.lpRebootMsg = "Service failed to start! Rebooting...";
            ServiceFailureActions.lpCommand = Command; //"SomeCommand.exe Param1 Param2";
            ServiceFailureActions.cActions = ScActions.Length;

            //The lpsaActions member of SERVICE_FAILURE_ACTIONS is a pointer to an
            //array of SC_ACTION structures. 

            //First order of business is to populate our array of SC_ACTION structures
            //with appropriate values.

            if (inScActions.GetUpperBound(0) <= 3)
            {
                //Once that's done, we need to obtain a pointer to a memory location
                //that we can assign to lpsaActions in SERVICE_FAILURE_ACTIONS.
                //We use 'Marshal.SizeOf(New modAPI.SC_ACTION) * 3' because we pass 
                //3 actions to our service. If you have less actions change the * 3 accordingly.
                iScActionsPointer = Marshal.AllocHGlobal(Marshal.SizeOf(new SC_ACTION()) * inScActions.GetUpperBound(0));

                //Once we have obtained the pointer for the memory location we need to
                //fill the memory with our structure. We use the CopyMemory API function
                //for this. Please have a look at it's declaration in modAPI.
                CopyMemory(iScActionsPointer, inScActions, Marshal.SizeOf(new SC_ACTION()) * inScActions.GetUpperBound(0));

                //We set the lpsaActions member of SERVICE_FAILURE_ACTIONS to the integer
                //value of our pointer.
                ServiceFailureActions.lpsaActions = iScActionsPointer.ToInt32();

                //We call bChangeServiceConfig2 with the relevant parameters.
                bool bChangeServiceConfig2 = ChangeServiceConfig2(schService, InfoLevel.SERVICE_CONFIG_FAILURE_ACTIONS, ref ServiceFailureActions);

                int errorcode = Marshal.GetLastWin32Error();

                //If the update of the failure actions are unsuccessful it is up to you to
                //throw an exception or not. The fact that the failure actions did not update
                //should not impact the functionality of your service.

                if (errorcode != 0 && errorcode != 997)
                {
                    if (bChangeServiceConfig2 == false)
                    {
                        throw new System.Exception(GetErrorMessage(errorcode));
                    }
                }
            }
            else
            {
                throw new System.Exception("Only three Service Failure Actions are allowed.");
            }

            UnlockAndClose();
        }

        public void Remove(string ServiceName)
        {
            int errorcode = 0;

            OpenAndLock();

            OpenService(ServiceName);

            DeleteService(schService);

            errorcode = Marshal.GetLastWin32Error();

            if (errorcode != 0 && errorcode != 997)
            {
                throw new System.Exception(GetErrorMessage(errorcode));
            }

            UnlockAndClose();

            return;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            mTimer.Enabled = true;
        }

        protected static void InstallService(
            string serviceName,
            string serviceDescription,
            string serviceUserAccount,
            string serviceUserAccountPassword)
        {
            NTService service = new NTService();
            service.Create(AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName,
                           serviceName,
                           serviceName,
                           serviceDescription,
                           ACCESS_TYPE.SERVICE_ALL_ACCESS,
                           ServiceType.SERVICE_WIN32_OWN_PROCESS,
                           ServiceStartType.SERVICE_AUTO_START,
                           ServiceErrorControl.SERVICE_ERROR_NORMAL,
                           serviceUserAccount == string.Empty ? null : serviceUserAccount,
                           serviceUserAccountPassword == string.Empty ? null : serviceUserAccountPassword);
        }

        protected static void UninstallService(string serviceName)
        {
            NTService service = new NTService();
            service.Remove(serviceName);
        }
    }

    public class ServiceException : ApplicationException
    {
        public ServiceException() : base() { }
        public ServiceException(string str) : base(str) { }
    }
}