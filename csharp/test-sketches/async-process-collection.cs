using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asynctest
{
    class X
    {
        Random r = new Random(DateTime.Now.Millisecond);
        async Task<bool> S(int i)
        {
            Console.WriteLine($"pre:  {i}");
            await Task.Delay(r.Next(0, 10) * 500);
            Console.WriteLine($"post: {i}");
            return i%2 == 0;
        }

        public IReadOnlyCollection<int> Run(IEnumerable<int> l)
        {
            return l.Select(x => new { Item = x, Oper = S(x) }).Where(x => x.Oper.Result).Select(x=>x.Item).ToArray();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var x = new X();
            Console.WriteLine("Run #1");
            x.Run(Enumerable.Range(1, 10)).ToList().ForEach(x=>Console.WriteLine(x));
            Console.WriteLine("Run #2");
            x.Run(Enumerable.Range(50, 10)).ToList().ForEach(x => Console.WriteLine(x));
        }
    }
}
