using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

namespace Deployment.Helper
{
    //used in calling WNetAddConnection2[StructLayout (LayoutKind.Sequential)]
    public struct NETRESOURCE
    {

    }

    public class Impersonate
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);


        //WIN32API - WNetAddConnection2
        [DllImport("mpr.dll",
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int WNetAddConnection2A(
            [MarshalAs(UnmanagedType.LPArray)]NetResource[] lpNetResource,
            [MarshalAs(UnmanagedType.LPStr)]string lpPassword,
            [MarshalAs(UnmanagedType.LPStr)]string lpUserName,
            int dwFlags);

        //WIN32API - WNetCancelConnection2
        [DllImport("mpr.dll",
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]

        private static extern int WNetCancelConnection2A(
            [MarshalAs(UnmanagedType.LPStr)]string lpName, int dwFlags, int fForce);

        //This parameter causes LogonUser to create a primary token.
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;


        public Impersonate()
        {
        }

        public IntPtr StartUserSession(string userName, string domainName, string password)
        {

            IntPtr tokenHandle = new IntPtr(0);
            IntPtr dupeTokenHandle = new IntPtr(0);
            try
            {
                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(userName, domainName, password,
                    LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT,
                    ref tokenHandle);

                if (true == returnValue)
                {
                    return tokenHandle;
                }
            }
            catch
            {

            }
            return IntPtr.Zero;
        }

        public void EndUserSession(IntPtr tokenHandle)
        {
            // Free the tokens.
            if (tokenHandle != IntPtr.Zero)
                CloseHandle(tokenHandle);
        }

        public static int ConnectNetResource(string domain, string username, string password)
        {
            NetResource[] nr = new NetResource[1];
            nr[0].lpRemoteName = domain;
            nr[0].lpLocalName = "";
            nr[0].dwScope = 0;
            nr[0].dwUsage = 0;
            nr[0].lpComment = "";
            nr[0].lpProvider = "";

            int flag = ConnectionFlag.CONNECT_INTERACTIVE;
            return WNetAddConnection2A(nr, password, username, flag);
        }
    }
}
