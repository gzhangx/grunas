using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace GRunAs
{
    [SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
    public class RunAs
    {        
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CreateProcessWithLogonW(String username, String domain, String pwd, int flags, string appName, StringBuilder commandLine, uint creationFlags, IntPtr environment, string currentDirectory, ref StartUpInfo sui, out ProcessInformation processInfo);

     
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);
     
        [StructLayout(LayoutKind.Sequential)]
        public struct StartUpInfo
        {
            /// <summary>
            /// Size of the structure, in bytes.
            /// </summary>
            public int cb;
            /// <summary>
            /// Reserved. Set this member to NULL before passing the structure to CreateProcess.
            /// </summary>
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpReserved;
            /// <summary>
            /// Pointer to a null-terminated string that specifies either the name of the desktop, or the name of both the desktop and window station for this process. A backslash in the string indicates that the string includes both the desktop and window station names.
            /// </summary>
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDesktop;
            /// <summary>
            /// For console processes, this is the title displayed in the title bar if a new console window is created. If NULL, the name of the executable file is used as the window title instead. This parameter must be NULL for GUI or console processes that do not create a new console window.
            /// </summary>
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpTitle;
            /// <summary>
            /// The x offset of the upper left corner of a window if a new window is created, in pixels.
            /// </summary>
            public int dwX;
            /// <summary>
            /// The y offset of the upper left corner of a window if a new window is created, in pixels.
            /// </summary>
            public int dwY;
            /// <summary>
            /// The width of the window if a new window is created, in pixels.
            /// </summary>
            public int dwXSize;
            /// <summary>
            /// The height of the window if a new window is created, in pixels.
            /// </summary>
            public int dwYSize;
            /// <summary>
            /// If a new console window is created in a console process, this member specifies the screen buffer width, in character columns.
            /// </summary>
            public int dwXCountChars;
            /// <summary>
            /// If a new console window is created in a console process, this member specifies the screen buffer height, in character rows.
            /// </summary>
            public int dwYCountChars;
            /// <summary>
            /// The initial text and background colors if a new console window is created in a console application.
            /// </summary>
            public int dwFillAttribute;
            /// <summary>
            /// Bit field that determines whether certain StartUpInfo members are used when the process creates a window.
            /// </summary>
            public int dwFlags;
            /// <summary>
            /// This member can be any of the SW_ constants defined in Winuser.h.
            /// </summary>
            public short wShowWindow;
            /// <summary>
            /// Reserved for use by the C Runtime; must be zero.
            /// </summary>
            public short cbReserved2;
            /// <summary>
            /// Reserved for use by the C Runtime; must be null.
            /// </summary>
            public IntPtr lpReserved2;
            /// <summary>
            /// A handle to be used as the standard input handle for the process.
            /// </summary>
            public IntPtr hStdInput;
            /// <summary>
            /// A handle to be used as the standard output handle for the process.
            /// </summary>
            public IntPtr hStdOutput;
            /// <summary>
            /// A handle to be used as the standard error handle for the process.
            /// </summary>
            public IntPtr hStdError;
        }

        /// <summary>
        /// The ProcessInformation structure is filled in by either the CreateProcess, CreateProcessAsUser, CreateProcessWithLogonW, or CreateProcessWithTokenW function with information about the newly created process and its primary thread.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            /// <summary>
            /// Handle to the newly created process. The handle is used to specify the process in all functions that perform operations on the process object.
            /// </summary>
            public IntPtr hProcess;
            /// <summary>
            /// Handle to the primary thread of the newly created process. The handle is used to specify the thread in all functions that perform operations on the thread object.
            /// </summary>
            public IntPtr hThread;
            /// <summary>
            /// Value that can be used to identify a process. The value is valid from the time the process is created until the time the process is terminated.
            /// </summary>
            public int dwProcessId;
            /// <summary>
            /// Value that can be used to identify a thread. The value is valid from the time the thread is created until the time the thread is terminated.
            /// </summary>
            public int dwThreadId;
        }

        /// <summary>
        /// The initial text and background colors if a new console window is created in a console application.
        /// </summary>
        [FlagsAttribute]
        public enum FillAttributes
        {
            /// <summary>
            /// Background color is intensified.
            /// </summary>
            BackgroundIntensity = 128,
            /// <summary>
            /// Background color contains red.
            /// </summary>
            BackgroundRed = 64,
            /// <summary>
            /// Background color contains green.
            /// </summary>
            BackgroundGreen = 32,
            /// <summary>
            /// Background color contains blue.
            /// </summary>
            BackgroundBlue = 16,
            /// <summary>
            /// Text color is intensified.
            /// </summary>
            ForegroundIntensity = 8,
            /// <summary>
            /// Text color contains red.
            /// </summary>
            ForegroundRed = 4,
            /// <summary>
            /// Text color contains green.
            /// </summary>
            ForegroundGreen = 2,
            /// <summary>
            /// Text color contains blue.
            /// </summary>
            ForegroundBlue = 1
        }

        /// <summary>
        /// Logon option. 
        /// </summary>
        [FlagsAttribute]
        public enum LogonFlags
        {
            /// <summary>
            /// Log on, then load the user's profile in the HKEY_USERS registry key. The function returns after the profile has been loaded. Loading the profile can be time-consuming, so it is best to use this value only if you must access the information in the HKEY_CURRENT_USER registry key.
            /// </summary>
            WithProfile = 1,
            /// <summary>
            /// Log on, but use the specified credentials on the network only. The new process uses the same token as the caller, but the system creates a new logon session within LSA, and the process uses the specified credentials as the default credentials.
            /// </summary>
            NetworkCredentialsOnly = 2
        }

        /// <summary>
        /// Controls how the process is created. The DefaultErrorMode, NewConsole, and NewProcessGroup flags are enabled by default� even if you do not set the flag, the system will function as if it were set.
        /// </summary>
        [FlagsAttribute]
        public enum CreationFlags
        {
            /// <summary>
            /// The primary thread of the new process is created in a suspended state, and does not run until the ResumeThread function is called.
            /// </summary>
            Suspended = 0x00000004,
            /// <summary>
            /// The new process has a new console, instead of inheriting the parent's console.
            /// </summary>
            NewConsole = 0x00000010,
            /// <summary>
            /// The new process is the root process of a new process group.
            /// </summary>
            NewProcessGroup = 0x00000200,
            /// <summary>
            /// This flag is only valid starting a 16-bit Windows-based application. If set, the new process runs in a private Virtual DOS Machine (VDM). By default, all 16-bit Windows-based applications run in a single, shared VDM. 
            /// </summary>
            SeperateWOWVDM = 0x00000800,
            /// <summary>
            /// Indicates the format of the lpEnvironment parameter. If this flag is set, the environment block pointed to by lpEnvironment uses Unicode characters.
            /// </summary>
            UnicodeEnvironment = 0x00000400,
            /// <summary>
            /// The new process does not inherit the error mode of the calling process.
            /// </summary>
            DefaultErrorMode = 0x04000000,
        }

        /// <summary>
        /// Controls the new process's priority class, which is used to determine the scheduling priorities of the process's threads.
        /// </summary>
        [FlagsAttribute]
        public enum PriorityFlags
        {
            /// <summary>
            /// Process with no special scheduling needs.
            /// </summary>
            NormalPriority = 0x00000020,
            /// <summary>
            /// Process whose threads run only when the system is idle and are preempted by the threads of any process running in a higher priority class. An example is a screen saver. The idle priority class is inherited by child processes.
            /// </summary>
            IdlePriority = 0x00000040,
            /// <summary>
            /// Process that performs time-critical tasks that must be executed immediately for it to run correctly. The threads of a high-priority class process preempt the threads of normal or idle priority class processes.
            /// </summary>
            HighPriority = 0x00000080,
            /// <summary>
            /// Process that has the highest possible priority. The threads of a real-time priority class process preempt the threads of all other processes, including operating system processes performing important tasks.
            /// </summary>
            RealTimePriority = 0x00000100,
            /// <summary>
            /// Process that has priority above idle but below normal processes.
            /// </summary>
            BelowNormalPriority = 0x00004000,
            /// <summary>
            /// Process that has priority above normal but below high processes.
            /// </summary>
            AboveNormalPriority = 0x00008000
        }

        /// <summary>
        /// Determines whether certain StartUpInfo members are used when the process creates a window.
        /// </summary>
        [FlagsAttribute]
        public enum StartUpInfoFlags : uint
        {
            /// <summary>
            /// If this value is not specified, the wShowWindow member is ignored.
            /// </summary>
            UseShowWindow = 0x0000001,
            /// <summary>
            /// If this value is not specified, the dwXSize and dwYSize members are ignored.
            /// </summary>
            UseSize = 0x00000002,
            /// <summary>
            /// If this value is not specified, the dwX and dwY members are ignored.
            /// </summary>
            UsePosition = 0x00000004,
            /// <summary>
            /// If this value is not specified, the dwXCountChars and dwYCountChars members are ignored.
            /// </summary>
            UseCountChars = 0x00000008,
            /// <summary>
            /// If this value is not specified, the dwFillAttribute member is ignored.
            /// </summary>
            UseFillAttribute = 0x00000010,
            /// <summary>
            /// Indicates that the process should be run in full-screen mode, rather than in windowed mode.
            /// </summary>
            RunFullScreen = 0x00000020,
            /// <summary>
            /// Indicates that the cursor is in feedback mode after CreateProcess is called. The system turns the feedback cursor off after the first call to GetMessage.
            /// </summary>
            ForceOnFeedback = 0x00000040,
            /// <summary>
            /// Indicates that the feedback cursor is forced off while the process is starting. The Normal Select cursor is displayed.
            /// </summary>
            ForceOffFeedback = 0x00000080,
            /// <summary>
            /// Sets the standard input, standard output, and standard error handles for the process to the handles specified in the hStdInput, hStdOutput, and hStdError members of the StartUpInfo structure. If this value is not specified, the hStdInput, hStdOutput, and hStdError members of the STARTUPINFO structure are ignored.
            /// </summary>
            UseStandardHandles = 0x00000100,
            /// <summary>
            /// When this flag is specified, the hStdInput member is to be used as the hotkey value instead of the standard-input pipe.
            /// </summary>
            UseHotKey = 0x00000200,
            /// <summary>
            /// When this flag is specified, the StartUpInfo's hStdOutput member is used to specify a handle to a monitor, on which to start the new process. This monitor handle can be obtained by any of the multiple-monitor display functions (i.e. EnumDisplayMonitors, MonitorFromPoint, MonitorFromWindow, etc...).
            /// </summary>
            UseMonitor = 0x00000400,
            /// <summary>
            /// Use the HICON specified in the hStdOutput member (incompatible with UseMonitor).
            /// </summary>
            UseIcon = 0x00000400,
            /// <summary>
            /// Program was started through a shortcut. The lpTitle contains the shortcut path.
            /// </summary>
            TitleShortcut = 0x00000800,
            /// <summary>
            /// The process starts with normal priority. After the first call to GetMessage, the priority is lowered to idle.
            /// </summary>
            Screensaver = 0x08000000
        }

        /// <summary>
        /// Creates a new process and its primary thread. The new process then runs the
        ///  specified executable file in the security context of the specified
        ///  credentials (user, domain, and password). It can optionally load the user
        ///  profile for the specified user.
        /// </summary>
        /// <remarks>
        /// This method is untested.
        /// </remarks>
        /// <param name="userName">
        /// This is the name of the user account to log on to. If you use the UPN format,
        ///  user@domain, the Domain parameter must be NULL. The user account must have
        ///  the Log On Locally permission on the local computer.
        /// </param>
        /// <param name="domain">
        /// Specifies the name of the domain or server whose account database contains the
        ///  user account. If this parameter is NULL, the user name must be specified in
        ///  UPN format.
        /// </param>
        /// <param name="password">
        /// Specifies the clear-text password for the user account.
        /// </param>
        /// <param name="logonFlags">
        /// Logon option. This parameter can be zero or one value from the LogonFlags enum.
        /// </param>
        /// <param name="applicationName">
        /// Specifies the module to execute. The specified module can be a Windows-based
        ///  application. It can be some other type of module (for example, MS-DOS or OS/2)
        ///  if the appropriate subsystem is available on the local computer. The string
        ///  can specify the full path and file name of the module to execute or it can
        ///  specify a partial name. In the case of a partial name, the function uses the
        ///  current drive and current directory to complete the specification. The function
        ///  will not use the search path. If the file name does not contain an extension,
        ///  .exe is assumed. Therefore, if the file name extension is .com, this parameter
        ///  must include the .com extension. The appname parameter can be NULL. In that
        ///  case, the module name must be the first white space-delimited token in the
        ///  commandline string. If the executable module is a 16-bit application, appname
        ///  should be NULL, and the string pointed to by commandline should specify the
        ///  executable module as well as its arguments.
        /// </param>
        /// <param name="commandLine">
        /// Specifies the command line to execute. The maximum length of this string is
        ///  32,000 characters. The commandline parameter can be NULL. In that case, the
        ///  function uses the string pointed to by appname as the command line. If the
        ///  file name does not contain an extension, .exe is appended. Therefore, if the
        ///  file name extension is .com, this parameter must include the .com extension.
        ///  If the file name ends in a period with no extension, or if the file name
        ///  contains a path, .exe is not appended. If the file name does not contain a
        ///  directory path, the system searches for the executable file.
        /// </param>
        /// <param name="creationFlags">
        /// Use CreationFlags and PriorityFlags enums. Controls how the process is created.
        ///  Also controls the new process's priority class, which is used to determine the
        ///  scheduling priorities of the process's threads.
        /// </param>
        /// <param name="currentDirectory">
        /// Specifies the full path to the current directory for the process. The string
        ///  can also specify a UNC path. If this parameter is NULL, the new process will
        ///  have the same current drive and directory as the calling process.
        /// </param>
        /// <param name="environment">
        /// Pointer to an environment block for the new process. If this parameter is NULL,
        ///  the new process uses the environment of the specified user instead of the
        ///  environment of the calling process.
        /// </param>
        /// <param name="startupInfo">
        /// Specifies the window station, desktop, standard handles, and appearance of the
        ///  main window for the new process.
        /// </param>
        /// <param name="processInfo">
        /// ProcessInformation structure that receives identification information for the
        ///  new process, including a handle to the process.
        /// </param>
        /// <returns>
        /// Returns a System.Diagnostic.Process which will be null if the call failed.
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// Throws a System.ComponentModel.Win32Exception containing the last error if the
        ///  call failed.
        /// </exception>
        public static ProcessResult StartProcess(StartProcessInfo creds, string cmd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cmd);
            ProcessInformation processInfo;
            StartUpInfo startupInfo = creds.getStartupInfo();
            bool retval = CreateProcessWithLogonW(creds.userName, creds.domain, creds.password,
                (int)creds.logonFlags, creds.applicationName, sb, (uint)creds.creationFlags, creds.environment,
                creds.currentDirectory, ref startupInfo, out processInfo);
            return new ProcessResult(processInfo, retval);            
        }


        public class ProcessResult : IDisposable
        {
            public ProcessInformation processInfo;
            public bool isStarted { get; private set; }
            public ProcessResult(ProcessInformation pc, bool started)
            {
                processInfo = pc;
                isStarted = started;
            }
            public System.Diagnostics.Process getProcess()
            {
                if (processInfo.dwProcessId != 0)
                    return System.Diagnostics.Process.GetProcessById(processInfo.dwProcessId);
                return null;
            }
            public void Dispose()
            {
                if (processInfo.hProcess != IntPtr.Zero)
                {
                    CloseHandle(processInfo.hProcess);
                    CloseHandle(processInfo.hThread);
                }
            }
        }
        public class StartProcessInfo
        {
            public string domain = "";
            public string userName { get; private set; }
            public string password { get; private set; }            
            private StartUpInfo startupInfo;
            public LogonFlags logonFlags = LogonFlags.WithProfile;
            public CreationFlags creationFlags = CreationFlags.NewConsole;
            public IntPtr environment = IntPtr.Zero;
            public string applicationName = null;
            public string currentDirectory = null;
            public StartUpInfo getStartupInfo()
            {
                return startupInfo;
            }                        

            public StartProcessInfo(string user, string pass)
            {
                userName = user;
                password = pass;
                startupInfo = new StartUpInfo();
                startupInfo.cb = Marshal.SizeOf(startupInfo);
                startupInfo.lpTitle = null;
                startupInfo.dwFlags = (int)StartUpInfoFlags.UseCountChars;
                startupInfo.dwYCountChars = 50;                
            }
        }
    }
   
}