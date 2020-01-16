using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace asynctest
{
    class X
    {
        Random r = new Random(DateTime.Now.Millisecond);
        async Task<bool> S(int i)
        {
            Console.WriteLine($"pre:  {i} [thread:{Thread.CurrentThread.ManagedThreadId}]");
            await Task.Delay(r.Next(0, 10) * 500);
            Console.WriteLine($"post: {i} [thread:{Thread.CurrentThread.ManagedThreadId}]");
            return i%2 == 0;
        }

        public IReadOnlyCollection<int> Run(IEnumerable<int> l)
        {
            return l.Select(x => new { Item = x, Oper = S(x) }).Where(x => x.Oper.Result).Select(x=>x.Item).ToArray();
        }

        public IReadOnlyCollection<int> RunAsync(IEnumerable<int> l)
        {
            var tasks = l.Select(x => new { Item = x, Oper = S(x) });
            Console.WriteLine("Tasks prepared");
            Task.WhenAll(tasks.Select(x => x.Oper));
            return tasks.Where(x => x.Oper.Result).Select(x => x.Item).ToArray();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var x = new X();
            Console.WriteLine("Run #1");
            x.Run(Enumerable.Range(1, 10)).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("Run #2");
            x.Run(Enumerable.Range(50, 10)).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("RunAsync #1");
            x.RunAsync(Enumerable.Range(1, 10)).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("RunAsync #2");
            x.RunAsync(Enumerable.Range(50, 10)).ToList().ForEach(x => Console.WriteLine(x));
        }
    }
}
