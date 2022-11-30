using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleThreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ManualResetEvent mre = new ManualResetEvent(false);//线程信号标记

                Console.WriteLine("1、子线程执行耗时操作");
                Task.Run(() =>
                {
                    Console.WriteLine("Sub" + ":" + "子线程耗时操作");
                    Thread.Sleep(1000 * 5);
                    Console.WriteLine("Sub" + ":" + "子线程结束");
                    mre.Set();//标记继续
                });
                Console.WriteLine("Main" + ":" + "主线程等待");
                mre.WaitOne();//等待信号
                Console.WriteLine("Main" + ":" + "主线程继续");

                mre.Reset();//重置信号标记
                Console.WriteLine();
                Console.WriteLine("2、子线程执行异常操作");

                Exception ex = null;
                Task.Run(() =>
                {
                    Console.WriteLine("Sub" + ":" + "子线程异常操作");
                    try
                    {
                        Thread.Sleep(1000 * 2);
                        int i = 0;
                        i = i / i;//异常
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Sub" + ":" + "子线程抛错");
                        ex = e;
                        mre.Set();//标记继续
                    }
                });
                Console.WriteLine("Main" + ":" + "主线程等待");
                mre.WaitOne();//等待信号
                Console.WriteLine("Main" + ":" + "主线程异常处理");
                //异常处理
                if (ex != null)
                {
                    //throw ex;//包括这种写法也一样
                    Console.WriteLine("Main" + ":" + ex);
                    Console.WriteLine();
                }
                if (ex != null)//推荐
                {
                    throw new Exception("自定义异常信息，这种写法好处可以知道在主线程哪里抛错", ex);
                }

                Console.WriteLine("Main" + ":" + "主线程结束...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Main" + ":" + e);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
