/* Copyright (C) 2016-2020 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System.IO;

namespace DiskAccessLibrary
{
    public class CyclicRedundancyCheckException : IOException
    {
        public CyclicRedundancyCheckException() : this("Data Error (Cyclic Redundancy Check)")
        {
        }

        public CyclicRedundancyCheckException(string message) : base(message)
        {
            HResult = IOExceptionHelper.GetHResultFromWin32Error(Win32Error.ERROR_CRC);
        }
    }
}
