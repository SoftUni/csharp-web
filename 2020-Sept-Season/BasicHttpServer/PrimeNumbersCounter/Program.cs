using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumbersCounter
{
    class Program
    {
        static int Count = 0;
        static object lockObj = new object();

        static void Main(string[] args)
        {
            /*
             * 664580
             * 00:00:11.2199496
             */
            Stopwatch sw = Stopwatch.StartNew();
            PrintPrimeCount(1, 10_000_000);
            Console.WriteLine(Count);
            Console.WriteLine(sw.Elapsed);

            /*
             * 664580
             * 00:00:10.5438104
             */

            // Task scheduler
            // 8 virtual CPUs
            // 8 threads
            //
            // download
            //  ^ write file
            //     ^ send email
            //       ^ create
            //       ^ send over the Inet
            // prime number
            // 
            //
            //
            // 
            //

            //// Stopwatch sw2 = Stopwatch.StartNew();
            //// List<Task> tasks = new List<Task>();
            //// for (int i = 1; i <= 100; i++)
            //// {
            ////     var task = Task.Run(() => DownloadAsync(i));
            ////     tasks.Add(task);
            //// }
            //// 
            //// Task.WaitAll(tasks.ToArray());
            //// 
            //// Console.WriteLine(sw2.Elapsed);
            //// Console.ReadLine();

            //return;

            //Stopwatch sw = Stopwatch.StartNew();
            //Thread thread = new Thread(() => PrintPrimeCount(1, 2_500_000));
            //thread.Name = "1";
            //thread.Start();
            //Thread thread2 = new Thread(() => PrintPrimeCount(2_500_001, 5_000_000));
            //thread2.Start();
            //thread2.Name = "2";
            //Thread thread3 = new Thread(() => PrintPrimeCount(5_000_001, 7_500_000));
            //thread3.Start();
            //thread3.Name = "3";
            //Thread thread4 = new Thread(() => PrintPrimeCount(7_500_001, 10_000_000));
            //thread4.Start();
            //thread4.Name = "4";

            //thread.Join();
            //thread2.Join();
            //thread3.Join();
            //thread4.Join();
            //Console.WriteLine(Count);
            //Console.WriteLine(sw.Elapsed);
            //while (true)
            //{
            //    var input = Console.ReadLine();
            //    Console.WriteLine(input.ToUpper());
            //}
        }

        static async Task DownloadAsync(int i)
        {
            // async / await
            HttpClient httpClient = new HttpClient();
            var url = $"https://vicove.com/vic-{i}";
            var httpResponse = await httpClient.GetAsync(url);
            var vic = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(vic.Length);
        }

        static async Task OldDownloadAsync(int i)
        {
            HttpClient httpClient = new HttpClient();
            var url = $"https://vicove.com/vic-{i}";
            httpClient.GetAsync(url).ContinueWith((httpResponse) =>
            {
                if (httpResponse.IsFaulted)
                {

                }

                httpResponse.Result.Content.ReadAsStringAsync().ContinueWith((vic) =>
                {
                    if (vic.IsFaulted)
                    {

                    }

                    Console.WriteLine(vic.Result.Length);
                });
            });
        }

        static void PrintPrimeCount(int min, int max)
        {
            // for (int i = min; i < max + 1; i++)
            Parallel.For(min, max + 1, i =>
            {
                bool isPrime = true;
                for (int j = 2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    // moitor
                    lock (lockObj)
                    {
                        Count++;
                    }
                }
            });
        }
    }
}
