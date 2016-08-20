using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFlushQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoFlushQueue q = new AutoFlushQueue(10, 10);
            q.OnQueueIsFull = list => PrintList(list);
            Task.Run(() => FillQueue(q));
            Console.ReadLine();
        }

        private static void FillQueue(AutoFlushQueue q)
        {
            for (int i = 0; i < 100; i++)
                q.Enqueue(i);
        }

        private static void PrintList(List<int> list)
        {
            foreach (var l in list)
            {
                Thread.Sleep(1000);
                Console.WriteLine(l);
            }
        }
    }
}
