using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HXW.CodeHelpLi;
using System.Threading;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Decimal a = 10.9999M;
            AsyncHelper helper = new AsyncHelper();
            Console.WriteLine("start Thread ID:"+Thread.CurrentThread.ManagedThreadId);
            helper.BeginAsyncOperation();
           // Console.WriteLine("Thread ID:"+Thread.CurrentThread.ManagedThreadId);
            helper.WaitAsyncResult(10000);

            Console.WriteLine("end Thread ID:"+Thread.CurrentThread.ManagedThreadId);

        }
    }
}
