// ***************************************************************
// version:  2.0    date: 04/08/2008
//  -------------------------------------------------------------
// 
//  -------------------------------------------------------------
// previous version:  1.4    date: 05/11/2006
//  -------------------------------------------------------------
//  (C) 2006-2008   HXW.CodeHelpLi All Rights Reserved
// ***************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using  HXW.CodeHelpLi.Net.Tcp;
using System.Diagnostics;

namespace  HXW.CodeHelpLi.Net
{
    /// <summary>
    /// ���Թ���
    /// </summary>
    public static class NetDebuger
    {
        public static void PrintDebugMessage(string message)
        {
            sync.WaitOne();

            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            System.Diagnostics.Trace.WriteLine(string.Format("(0x{1:X8}){0}", message,
                Thread.CurrentThread.ManagedThreadId));

            Console.ForegroundColor = oldColor;
            sync.ReleaseMutex();
        }

        static Mutex sync = new Mutex();

        public static void PrintDebugMessage(TcpSession session, string message)
        {
            sync.WaitOne();

            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Trace.Write(string.Format("(0x{0:X8})", Thread.CurrentThread.ManagedThreadId));
            Console.ForegroundColor = ConsoleColor.White;
            Trace.Write(string.Format("{0}", session));
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.Write(" - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Trace.WriteLine(message);
            Console.ForegroundColor = oldColor;
            sync.ReleaseMutex();
        }

        public static void PrintErrorMessage(TcpSession session, string message)
        {
            sync.WaitOne();
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            System.Diagnostics.Trace.WriteLine(string.Format("(0x{2:X8}){0} - {1}", session, message,
                Thread.CurrentThread.ManagedThreadId));

            Console.ForegroundColor = oldColor;
            sync.ReleaseMutex();

        }

        public static void PrintErrorMessage(string message)
        {
            sync.WaitOne();

            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            System.Diagnostics.Trace.WriteLine(string.Format("(0x{1:X8}){0}", message,
                Thread.CurrentThread.ManagedThreadId));

            Console.ForegroundColor = oldColor;
            sync.ReleaseMutex();

        }

    }
}
