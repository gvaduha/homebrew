using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotsProblem
{
    enum Containment
    {
        Robot,
        Parachute
    }

    class Robot
    {
        int step;
        IDictionary<int,Containment> v;
        int pos;
        bool moving;

        public Robot(IDictionary<int, Containment> v, int pos)
        {
            this.v = v;
            this.pos = pos;
            this.step = 1;
            this.moving = true;
        }

        public IEnumerable<int> run()
        {
            if (moving)
            {
                Containment c;
                if (v.ContainsKey(pos + step))
                {
                    v[pos + step] = Containment.Robot;
                    moving = false;
                    yield break;
                }
                if (v.TryGetValue(pos, out c) && c == Containment.Robot)
                {
                    yield return 88;
                }
                step++;
                yield break;
            }
        }
    }

    class RobotsProblem
    {
        static void Main(string[] args)
        {
            // if pos1==pos2 return boom;

            var v = new Dictionary<int, Containment>(2) { {2, Containment.Parachute}, {7, Containment.Parachute} };

            Robot r1 = new Robot(v,2);
            Robot r2 = new Robot(v,7);

            for (;;)
            {
                r1.run().Any();
                if (r2.run().Any()) break;
            }

            Console.WriteLine("BOOM!");
            //Console.WriteLine(String.Join("",v));
        }
    }
}
