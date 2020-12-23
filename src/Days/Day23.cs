using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 23)]
    public class Day23 : BaseDay
    {
        public override string PartOne(string input)
        {
            var cups = new LinkedList<int>();

            foreach (var c in input.Trim())
            {
                cups.AddLast(int.Parse(c.ToString()));
            }

            var refs = BuildNodeRefs(cups);
            var currentCup = cups.First;

            for (var m = 0; m < 100; m++)
            {
                currentCup = MakeMove(currentCup, refs);
            }

            var cur = refs[1];
            var result = string.Empty;

            for (var i = 0; i < cups.Count - 1; i++)
            {
                cur = cur.NextCircular();
                result += cur.Value.ToString();
            }

            return result;
        }

        private LinkedListNode<int> MakeMove(LinkedListNode<int> cup, List<LinkedListNode<int>> refs)
        {
            var pick1 = cup.NextCircular();
            var pick2 = pick1.NextCircular();
            var pick3 = pick2.NextCircular();

            cup.List.Remove(pick1);
            cup.List.Remove(pick2);
            cup.List.Remove(pick3);

            var dest = cup.Value - 1;

            while (pick1.Value == dest || pick2.Value == dest || pick3.Value == dest || dest == 0)
            {
                dest--;

                if (dest < 1)
                {
                    dest = refs.Count - 1;
                }
            }

            cup.List.AddAfter(refs[dest], pick1);
            cup.List.AddAfter(pick1, pick2);
            cup.List.AddAfter(pick2, pick3);

            return cup.NextCircular();
        }

        public override string PartTwo(string input)
        {
            var cups = new LinkedList<int>();

            foreach (var c in input.Trim())
            {
                cups.AddLast(int.Parse(c.ToString()));
            }

            var maxCup = cups.Max();

            for (var i = maxCup + 1; i <= 1000000; i++)
            {
                cups.AddLast(i);
            }

            var refs = BuildNodeRefs(cups);
            var currentCup = cups.First;

            for (var m = 0; m < 10000000; m++)
            {
                currentCup = MakeMove(currentCup, refs);
            }

            var result = (long)refs[1].NextCircular().Value * (long)refs[1].NextCircular().NextCircular().Value;

            return result.ToString();
        }

        private List<LinkedListNode<int>> BuildNodeRefs(LinkedList<int> cups)
        {
            var result = new List<LinkedListNode<int>>(cups.Count + 1);

            for (var i = 0; i < cups.Count + 1; i++)
            {
                result.Add(null);
            }

            var cur = cups.First;

            for (var i = 0; i <= cups.Count; i++)
            {
                result[cur.Value] = cur;
                cur = cur.NextCircular();
            }

            return result;
        }
    }
}
