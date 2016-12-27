using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TrueResize
{
    public class Win32Utils
    {
        // Pinvoke for API function
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
            out ulong lpFreeBytesAvailable,
            out ulong lpTotalNumberOfBytes,
            out ulong lpTotalNumberOfFreeBytes);

        /// <summary>
        /// This method will also accept UNC paths
        /// </summary>
        public static Nullable<ulong> GetFreeDiskSpace(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("folderName");
            }

            if (!path.EndsWith("\\"))
            {
                path += '\\';
            }

            ulong free = 0, dummy1 = 0, dummy2 = 0;

            if (GetDiskFreeSpaceEx(path, out free, out dummy1, out dummy2))
            {
                return free;
            }
            else
            {
                return null;
            }
        }
    }
}
