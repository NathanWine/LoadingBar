using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadingBar
{
    class Program
    {
        static void Main(string[] args)
        {
            const string loadingMsg = "Now loading: ";
            Random rng = new Random();
            Console.Write(loadingMsg);

            using (LoadingBar bar = new LoadingBar(loadingMsg.Length))
            {
                while (bar.Progress < 1.0)
                {
                    Thread.Sleep(rng.Next(10, 25));                 // Simulate work being done
                    bar.Progress += 0.001;                          // Simulate progress being made towards final goal
                }
            }

            Thread t = new Thread(AsyncTest.DoWork);
            t.Start();
        }
    }
    class AsyncTest
    {
        public static void DoWork()
        {
            const string loadingMsg = "Now loading: ";
            Random rng = new Random();
            Console.Write(loadingMsg);
            using (LoadingBar bar = new LoadingBar(loadingMsg.Length))
            {
                while (bar.Progress < 1.0)
                {
                    Thread.Sleep(rng.Next(10, 25));                 // Simulate work being done
                    bar.Progress += 0.001;                          // Simulate progress being made towards final goal
                }
            }
        }
    }
}
