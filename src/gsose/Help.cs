﻿using System;
using System.Runtime.InteropServices;
using ClrMDExports;
using Microsoft.Diagnostics.Runtime;
using RGiesecke.DllExport;

namespace gsose
{
    public partial class DebuggerExtensions
    {
        [DllExport("Help")]
        public static void Help(IntPtr client, [MarshalAs(UnmanagedType.LPStr)] string args)
        {
            DebuggingContext.Execute(client, args, OnHelp);
        }
        [DllExport("help")]
        public static void help(IntPtr client, [MarshalAs(UnmanagedType.LPStr)] string args)
        {
            DebuggingContext.Execute(client, args, OnHelp);
        }

        private const string _help =
        "-------------------------------------------------------------------------------\r\n" +
        "gsose is a debugger extension DLL designed to dig into CLR data structures.\r\n" +
        "Functions are listed by category and shortcut names are listed in parenthesis.\r\n" +
        "Type \"!help <functionname>\" for detailed info on that function.\r\n" +
        "\r\n" +
        "Thread Pool                       Threads\r\n" +
        "-----------------------------     -----------------------------\r\n" +
        "TpQueue(tpq)                      ParallelStacks (pstacks)\r\n" +
        "TpRunning(tpr)\r\n" +
        "\r\n" +
        "Tasks                             Timers\r\n" +
        "-----------------------------     -----------------------------\r\n" +
        "TkState (tks)                     TimerInfo (ti)\r\n" +
        "GetMethodName (gmn)                                    \r\n" +
        "\r\n" +
        "Data structures                  Strings\r\n" +
        "-----------------------------    -----------------------------\r\n" +
        "DumpConcurrentDictionary (dcd)   StringDuplicates (sd)\r\n" +
        "DumpConcurrentQueue (dcq)         \r\n" +
        "\r\n" +
        "Garbage Collector\r\n" +
        "-----------------------------\r\n" +
        "GCInfo (gci)\r\n" +
        "PinnedObjects (po)\r\n" +
        "\r\n"
        ;
        //
        //
        const string _tpqHelp =
        "-------------------------------------------------------------------------------\r\n" +
        "!TpQueue \r\n" +
        "\r\n" +
        "!TpQueue lists the enqueued workitems in the Clr Thread Pool followed by a summary of\r\n"+
        "the different tasks/work items.\r\n" +
        "The global queue is first iterated before local per-thread queues.\r\n" +
        "The name of the method to be called (on which instance if any) is also provided when available.\r\n" +
        "\r\n" +
        "    0:000> !tpq\r\n" +
        "    global work item queue________________________________\r\n" +
        "    0x00000265CC2A92C8 Task | ThreadPoolUseCases.MainWindow.b__10_0\r\n" +
        "    0x00000265CC2A9408 Task | ThreadPoolUseCases.MainWindow.b__10_0\r\n" +
        "    0x00000265CC2A9548 Task | ThreadPoolUseCases.MainWindow.b__10_0\r\n" +
        "\r\n" +
        "    local per thread work items_____________________________________\r\n" +
        "\r\n" +
        "        3 Task ThreadPoolUseCases.MainWindow.b__10_0\r\n" +
        "     ----\r\n" +
        "        3\r\n";
        //
        //
        const string _tprHelp =
        "-------------------------------------------------------------------------------\r\n" +
        "!TpRunning \r\n" +
        "\r\n" +
        "!TpRunning lists the work items current run by the Clr Thread Pool threads followed by\r\n" +
        "a summary of the different tasks/work items.\r\n" +
        "The consummed CPU is displayed with the number of running/dead/max threads in the Thread Pool.\r\n" +
        "For each thread, its ID, ThreadOBJ address, number of locks and details are provided.\r\n" +
        "The details contain the name of the callback method and the synchronization object address on which\r\n"+
        "it is blocked it any (as a parameter of the corresponding method such as WaitHandle.WaitOneNative).\r\n" +
        "\r\n" +
        "0:000> !tpr\r\n"+
        "\r\n" +
        "CPU = 12%% for 50 threads (#idle = 0 + #running = 50 | #dead = 0 | #max = 50)\r\n" +
        "\r\n" +
        "  ID ThreadOBJ        Locks  Details\r\n" +
        "-----------------------------------------------------------------------------------\r\n" +
        "  24 000001DB2F549430  0001  Task | ThreadPoolUseCases.MainWindow.b__13_2(System.Object)\r\n" +
        "  34 000001DB359FE750  0001  Work | ThreadPoolUseCases.MainWindow.b__13_3(System.Object)\r\n" +
        "   4 000001DB2F516180        Task | ThreadPoolUseCases.MainWindow.b__13_0(System.Object)\r\n" +
        "                          => WaitHandle.WaitOneNative(0x2040489605328 : SafeWaitHandle\r\n" +
        "                          ...\r\n" +
        "  52 000001DB359CCCD0        Work | ThreadPoolUseCases.MainWindow.b__13_4(System.Object)\r\n" +
        "  53 000001DB359CF3E0        Work | ThreadPoolUseCases.MainWindow.b__13_4(System.Object)\r\n" +
        "\r\n" +
        "\r\n" +
        "____________________________________________________________________________________________________\r\n" +
        "Count Details\r\n" +
        "----------------------------------------------------------------------------------------------------\r\n" +
        "    1 Task | ThreadPoolUseCases.MainWindow.b__13_2(System.Object)\r\n" +
        "    1 Work | ThreadPoolUseCases.MainWindow.b__13_3(System.Object)\r\n" +
        "    9 Task | ThreadPoolUseCases.MainWindow.b__13_2(System.Object)\r\n" +
        "                                  => Monitor.Enter\r\n" +
        "\r\n" +
        "    9 Work | ThreadPoolUseCases.MainWindow.b__13_3(System.Object)\r\n" +
        "                                  => Monitor.Enter\r\n" +
        "\r\n" +
        "   10 Task | ThreadPoolUseCases.MainWindow.b__13_0(System.Object)\r\n" +
        "                                  => WaitHandle.WaitOneNative(0x2040489605328 : SafeWaitHandle\r\n" +
        "\r\n" +
        "   10 Work | ThreadPoolUseCases.MainWindow.b__13_1(System.Object)\r\n" +
        "                                  => WaitHandle.WaitOneNative(0x2040489605328 : SafeWaitHandle\r\n" +
        "\r\n" +
        "   10 Work | ThreadPoolUseCases.MainWindow.b__13_4(System.Object)\r\n" +
        " ----\r\n" +
        "   50\r\n" +
        "\r\n";
        //
        //
        const string _tiHelp =
        "-------------------------------------------------------------------------------\r\n" +
        "!TimerInfo \r\n" +
        "\r\n" +
        "!TimerInfo lists all the running timers followed by a summary of the different items.\r\n" +
        "The global queue is first iterated before local per-thread queues.\r\n" +
        "The name of the method to be called (on which instance if any) is also provided when available.\r\n" +
        "\r\n" +
        "0:000> !ti\r\n" +
        "0x0000022836D57410 @    2000 ms every     2000 ms |  0000022836D573D8 (ThreadPoolUseCases.MainWindow+TimerInfo) -> ThreadPoolUseCases.MainWindow.OnTimerCallback\r\n" +
        "0x0000022836D575A0 @    5000 ms every     5000 ms |  0000022836D57568 (ThreadPoolUseCases.MainWindow+TimerInfo) -> ThreadPoolUseCases.MainWindow.OnTimerCallback\r\n" +
        "\r\n" +
        "   2 timers\r\n" +
        "-----------------------------------------------\r\n" +
        "   1 | 0x0000022836D57410 @    2000 ms every     2000 ms |  0000022836D573D8 (ThreadPoolUseCases.MainWindow+TimerInfo) -> ThreadPoolUseCases.MainWindow.OnTimerCallback\r\n" +
        "   1 | 0x0000022836D575A0 @    5000 ms every     5000 ms |  0000022836D57568 (ThreadPoolUseCases.MainWindow+TimerInfo) -> ThreadPoolUseCases.MainWindow.OnTimerCallback\r\n";
        //
        //
        const string _tksHelp =
        "-------------------------------------------------------------------------------\r\n" +
        "!TkState [hexa address]\r\n" +
        "         [decimal state value]\r\n" +
        "\r\n" +
        "!TkState translates a Task m_stateFlags field value into text.\r\n" +
        "It supports direct decimal value or hexacimal address correspondig to a task instance.\r\n" +
        "\r\n" +
        "0:000> !tkstate 000001db16cf98f0\r\n" +
        "Task state = Running\r\n" +
        "0:000> !tkstate 204800\r\n" +
        "Task state = Running\r\n";
        //
        //
        const string _sdHelp =
        "-------------------------------------------------------------------------------\r\n" +
        "!StringDuplicates [duplication threshold]\r\n" +
        "\r\n" +
        "!StringDuplicates lists strings duplicated more than the given threshold (100 by default)" +
        "sorted by memory consumption.\r\n" +
        "Note that new lines are replaced by '##' to keep each string on one line.\r\n" +
        "\r\n" +
        "0:000> !sd 5\r\n" +
        "       6           24 fr\r\n" +
        "       6           60 Color\r\n" +
        "       9           90 fr-FR\r\n" +
        "      10          100 Value\r\n" +
        "       6          120 Background\r\n" +
        "      10          220 application\r\n" +
        "      35          280 Name\r\n" +
        "       8         1968 System.Configuration.IgnoreSection, System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\r\n" +
        "-------------------------------------------------------------------------\r\n" +
        "                    0 MB\r\n";
        //
        //
        const string _gmnHelp =
            "-------------------------------------------------------------------------------\r\n" +
            "!GetMethodName [hexa address]\r\n" +
            "\r\n" +
            "!GetMethodName translates an address to a method into this method name (namespaces.type.method)" +
            "\r\n" +
            "0:000> !gmn 7fe886000b0\r\n" +
            "TestTimers.TimerTester.ValidateScore\r\n" +
            "-------------------------------------------------------------------------\r\n" +
            "\r\n";
        //
        //
        const string _poHelp =
            "-------------------------------------------------------------------------------\r\n" +
            "!PinnedObjects [minimum instance count threshold to be listed]\r\n" +
            "\r\n" +
            "!PinnedObjects lists pinned objects (Pinned/asyncPinned) per generation sorted by type name" +
            "\r\n" +
            "0:000> !po 3\r\n" +
            "Gen0: 64\r\n" +
            "   System.String : 64\r\n" +
            "   -      Pinned | 1c50235d7c0\r\n" +
            "   ...\r\n" +
            "   -      Pinned | 1c5024d5c60\r\n" +
            "Gen1: 0\r\n" +
            "Gen2: 115\r\n" +
            "LOH: 71\r\n" +
            "   System.Object[] : 7\r\n" +
            "   - AsyncPinned | 1c512231038\r\n" +
            "   ...\r\n" +
            "   - AsyncPinned | 1c5122518f8\r\n" +
            "   System.String : 64\r\n" +
            "   -      Pinned | 1c512252130\r\n" +
            "   ...\r\n" +
            "   -      Pinned | 1c512c8abe0\r\n" +
            "-------------------------------------------------------------------------\r\n" +
            "Total: 250 pinned object\r\n" +
            "\r\n";
        //
        //
        const string _gciHelp =
            "-------------------------------------------------------------------------------\r\n" +
            "!GCInfo\r\n" +
            "\r\n" +
            "!GCInfo lists generations per segments. Show pinned objects with -pinned and object instances count/size with -stat (by default)" +
            "\r\n" +
            "0:000> !gci [-stat] [-pinned]\r\n" +
            "\r\n" +
            "\r\n";
        //
        //
        private const string _dcdHelp =
            "-------------------------------------------------------------------------------\r\n" +
            "!DumpConcurrentDictionary\r\n" +
            "\r\n" +
            "!dcd lists all items in the given concurrent dictionary" +
            "\r\n" +
            "0:000> !dcd 000001d10df6daa0\r\n" +
            "System.Collections.Concurrent.ConcurrentDictionary<System.Int32, NetCoreConsoleApp.InstanceInConcurrentDataStructures>\r\n" +
            " 2237 buckets\r\n" +
            "0 = 0x000001D10DF5B420 (NetCoreConsoleApp.InstanceInConcurrentDataStructures)\r\n" +
            "1 = 0x000001D10DF5B438 (NetCoreConsoleApp.InstanceInConcurrentDataStructures)\r\n" +
            "2 = 0x000001D10DF5B450 (NetCoreConsoleApp.InstanceInConcurrentDataStructures)\r\n" +
            "3 = 0x000001D10DF5B468 (NetCoreConsoleApp.InstanceInConcurrentDataStructures)\r\n" +
            "4 = 0x000001D10DF5B480 (NetCoreConsoleApp.InstanceInConcurrentDataStructures)\r\n" +
            "\r\n";        //
        //
        private const string _dcqHelp =
            "-------------------------------------------------------------------------------\r\n" +
            "!DumpConcurrentQueue\r\n" +
            "\r\n" +
            "!dcq lists all items in the given concurrent queue. Show each item type with -t as parameter" +
            "\r\n" +
            "0:000> !dcq 000001d10df67420 -t\r\n" +
            "   1 - 0x000001D10DF5B420 | NetCoreConsoleApp.InstanceInConcurrentDataStructures\r\n" +
            "   2 - 0x000001D10DF5B438 | NetCoreConsoleApp.InstanceInConcurrentDataStructures\r\n" +
            "   3 - 0x000001D10DF5B450 | NetCoreConsoleApp.InstanceInConcurrentDataStructures\r\n" +
            "   4 - 0x000001D10DF5B468 | NetCoreConsoleApp.InstanceInConcurrentDataStructures\r\n" +
            "   5 - 0x000001D10DF5B480 | NetCoreConsoleApp.InstanceInConcurrentDataStructures\r\n" +
            "\r\n";
        //
        private const string _pstacksHelp =
            "-------------------------------------------------------------------------------\r\n" +
            "!pstacks\r\n" +
            "\r\n" +
            "!pstacks merges parallel stacks.\r\n" +
            "List 4 thread IDs at the end of each frame groups (-all to get the full list).\r\n" +
            "Click such a thread ID to change current thread.\r\n" +
            "\r\n" +
            "0:000> !pstacks (-all)\r\n" +
            "________________________________________________\r\n" +
            "~~~~ 8f8c\r\n" +
            "    1 (dynamicClass).IL_STUB_PInvoke(IntPtr, Byte*, Int32, Int32 ByRef, IntPtr)\r\n" +
            "    ...\r\n" +
            "    1 System.Console.ReadLine()\r\n" +
            "    1 NetCoreConsoleApp.Program.Main(String[])\r\n" +
            "\r\n" +
            "________________________________________________\r\n" +
            "           ~~~~ 7034\r\n" +
            "              1 System.Threading.Monitor.Wait(Object, Int32, Boolean)\r\n" +
            "              ...\r\n" +
            "              1 System.Threading.Tasks.Task.Wait()\r\n" +
            "              1 NetCoreConsoleApp.Program+c.b__1_4(Object)\r\n" +
            "           ~~~~ 9c6c,4020\r\n" +
            "              2 System.Threading.Monitor.Wait(Object, Int32, Boolean)\r\n" +
            "              ...\r\n" +
            "                   2 NetCoreConsoleApp.Program+c__DisplayClass1_0.b__7()\r\n" +
            "              3 System.Threading.Tasks.Task.InnerInvoke()\r\n" +
            "         4 System.Threading.Tasks.Task+c.cctor>b__278_1(Object)\r\n" +
            "         ...\r\n" +
            "         4 System.Threading.Tasks.Task.ExecuteEntryUnsafe()\r\n" +
            "         4 System.Threading.Tasks.Task.ExecuteWorkItem()\r\n" +
            "    7 System.Threading.ThreadPoolWorkQueue.Dispatch()\r\n" +
            "    7 System.Threading._ThreadPoolWaitCallback.PerformWaitCallback()\r\n" +
            "\r\n" +
            "==> 8 threads with 2 roots" +
            "\r\n";
        //
        private static void OnHelp(ClrRuntime runtime, string args)
        {
            string command = args;
            if (args != null)
                command = args.ToLower();

            switch (command)
            {
                case "pstacks":
                case "parallelstacks":
                    Console.WriteLine(_pstacksHelp);
                    break;

                case "tpr":
                case "tprunning":
                    Console.WriteLine(_tprHelp);
                    break;

                case "tpq":
                case "tpqueue":
                    Console.WriteLine(_tpqHelp);
                    break;

                case "ti":
                case "timerinfo":
                    Console.WriteLine(_tiHelp);
                    break;

                case "tks":
                case "taskstate":
                    Console.WriteLine(_tksHelp);
                    break;

                case "sd":
                case "stringduplicates":
                    Console.WriteLine(_sdHelp);
                    break;

                case "gmn":
                case "getmethodname":
                    Console.WriteLine(_gmnHelp);
                    break;

                case "po":
                case "pinnedobjects":
                    Console.WriteLine(_poHelp);
                    break;

                case "gci":
                case "gcinfo":
                    Console.WriteLine(_gciHelp);
                    break;

                case "dcd":
                case "dumpconcurrentdictionary":
                    Console.WriteLine(_dcdHelp);
                    break;

                case "dcq":
                case "dumpconcurrentqueue":
                    Console.WriteLine(_dcqHelp);
                    break;

                default:
                    Console.WriteLine(_help);
                    break;
            }
        }
    }
}