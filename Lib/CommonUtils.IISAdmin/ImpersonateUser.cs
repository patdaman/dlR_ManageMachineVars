using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;

namespace CommonUtils.IISAdmin
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    public class ImpersonateUser : IDisposable
    {
        private WindowsImpersonationContext impersonatedUser;
        private SafeTokenHandle safeTokenHandle;

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        //public WindowsIdentity GetUser(string domainName, string userName, string password)
        public WindowsImpersonationContext GetUser(string userName, string password, string domainName = null)
        {
            WindowsIdentity newId;
            if (string.IsNullOrWhiteSpace(domainName))
                domainName = Environment.UserDomainName;
            try
            {
                //const int LOGON32_PROVIDER_DEFAULT = 0;
                //const int LOGON32_LOGON_INTERACTIVE = 2;
                //bool returnValue = LogonUser(userName, domainName, password,
                //    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                //    out safeTokenHandle);
                const int LOGON_TYPE_NEW_CREDENTIALS = 9;
                const int LOGON32_PROVIDER_WINNT50 = 3;
                bool returnValue = LogonUser(userName, domainName, password,
                            LOGON_TYPE_NEW_CREDENTIALS, LOGON32_PROVIDER_WINNT50,
                            out safeTokenHandle);
                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ret);
                }
                newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle());
                impersonatedUser = newId.Impersonate();
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Exception occurred impersonating Domain user ", ex);
            }
            //return newId;
            return impersonatedUser;
        }
        public void Dispose()
        {
            this.impersonatedUser.Dispose();
            this.safeTokenHandle.Dispose();
        }
    }

    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true) { }
        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}