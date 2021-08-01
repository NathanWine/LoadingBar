using System;
using System.Threading.Tasks;

namespace LoadingBar
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<bool> loading = AsyncTest.LoadAsync();
            loading.Wait();
        }
    }
    class AsyncTest
    {
        public async static Task<bool> LoadAsync()
        {
            Random rng = new Random();
            using (LoadingBar bar = new LoadingBar())
            {
                while (bar.Progress < 1.0)
                {
                    await Task.Delay(rng.Next(10, 25));         // Simulate work being done
                    bar.Progress += 0.002545;                   // Simulate progress being made towards final goal
                }
            }
            return true;
        }
    }
}
