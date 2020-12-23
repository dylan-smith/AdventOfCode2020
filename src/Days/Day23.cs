using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 23)]
    public class Day23 : BaseDay
    {
        // 215694783
        //public override string PartOne(string input)
        //{
        //    var cups = new LinkedList<int>();

        //    foreach (var c in input.Trim())
        //    {
        //        cups.AddLast(int.Parse(c.ToString()));
        //    }

        //    var currentCup = cups.First;

        //    for (var m = 0; m < 100; m++)
        //    {
        //        currentCup = MakeMove(currentCup, cups);
        //    }

        //    var cur = cups.Find(1);
        //    var result = string.Empty;

        //    for (var i = 0; i < cups.Count - 1; i++)
        //    {
        //        cur = cur.NextCircular();
        //        result += cur.Value.ToString();
        //    }

        //    return result;
        //}

        public override string PartOne(string input)
        {
            var start = new CupNode();
            var cur = start;

            foreach (var c in input.Trim())
            {
                cur.Value = int.Parse(c.ToString());
                var newCup = new CupNode();
                newCup.Previous = cur;
                cur.Next = newCup;
                cur = newCup;
            }

            cur.Previous.Next = start;
            start.Previous = cur.Previous;

            //var maxCup = 9;

            //for (var i = maxCup + 1; i <= 1000000; i++)
            //{
            //    var newCup = new CupNode();
            //    newCup.Value = i;
            //    start.Previous.Next = newCup;
            //    newCup.Previous = start.Previous;
            //    start.Previous = newCup;
            //    newCup.Next = start;
            //}

            var currentCup = start;

            for (var m = 0; m < 10; m++)
            {
                if (m % 100000 == 0)
                {
                    Log($"{m}");
                }

                //currentCup = MakeMove2(currentCup);
            }

            //var resultCup = FindDestination2(1, start).Next;
            var resultCup = start;
            var result = string.Empty;

            while (resultCup.Value != 1)
            {
                result += resultCup.Value.ToString();
                resultCup = resultCup.Next;
            }

            return result;
        }

        private LinkedListNode<int> MakeMove(LinkedListNode<int> cup, LinkedList<int> list)
        {
            var pickedUp = new List<int>();

            var pick1 = cup.NextCircular();
            var pick2 = pick1.NextCircular();
            var pick3 = pick2.NextCircular();

            pickedUp.Add(pick1.Value);
            pickedUp.Add(pick2.Value);
            pickedUp.Add(pick3.Value);

            list.Remove(pick1);
            list.Remove(pick2);
            list.Remove(pick3);

            var destination = FindDestination(cup.Value, cup.List);

            destination = list.AddAfter(destination, pickedUp[0]);
            destination = list.AddAfter(destination, pickedUp[1]);
            destination = list.AddAfter(destination, pickedUp[2]);

            return cup.NextCircular();
        }

        private CupNode MakeMove2(CupNode cup, List<CupNode> refs)
        {
            var pick1 = cup.Next;
            var pick2 = pick1.Next;
            var pick3 = pick2.Next;

            cup.Next = pick3.Next;
            cup.Next.Previous = cup;

            var dest = cup.Value - 1;

            while (pick1.Value == dest || pick2.Value == dest || pick3.Value == dest || dest == 0)
            {
                dest--;

                if (dest < 1)
                {
                    dest = 1000000;
                }
            }

            var destination = refs[dest];

            var after = destination.Next;
            destination.Next = pick1;
            pick1.Previous = destination;
            pick3.Next = after;
            after.Previous = pick3;

            return cup.Next;
        }

        private LinkedListNode<int> FindDestination(int value, LinkedList<int> list)
        {
            var dest = value - 1;

            while (!list.Contains(dest))
            {
                dest--;

                if (dest < 1)
                {
                    dest = list.Max();
                }
            }

            return list.Find(dest);
        }

        //private CupNode FindDestination2(int value, CupNode cup)
        //{
        //    var cur = cup;

        //    while (cur.Value != value)
        //    {
        //        cur = cur.Next;
        //    }

        //    return cur;
        //}

        public override string PartTwo(string input)
        {
            var start = new CupNode();
            var cur = start;

            foreach (var c in input.Trim())
            {
                cur.Value = int.Parse(c.ToString());
                var newCup = new CupNode();
                newCup.Previous = cur;
                cur.Next = newCup;
                cur = newCup;
            }

            cur.Previous.Next = start;
            start.Previous = cur.Previous;

            var maxCup = 9;

            for (var i = maxCup + 1; i <= 1000000; i++)
            {
                var newCup = new CupNode();
                newCup.Value = i;
                start.Previous.Next = newCup;
                newCup.Previous = start.Previous;
                start.Previous = newCup;
                newCup.Next = start;
            }

            var refs = new List<CupNode>(1000001);
            cur = start;

            for (var i = 0; i < 1000001; i++)
            {
                refs.Add(null);
            }

            for (var i = 0; i < 1000000; i++)
            {
                refs[cur.Value] = cur;
                cur = cur.Next;
            }

            var currentCup = start;

            for (var m = 0; m < 10000000; m++)
            {
                if (m % 100000 == 0)
                {
                    Log($"{m}");
                }

                currentCup = MakeMove2(currentCup, refs);
            }

            var result = (long)refs[1].Next.Value * (long)refs[1].Next.Next.Value;

            return result.ToString();
        }

        //public override string PartTwo(string input)
        //{
        //    var cups = new LinkedList<int>();
        //    var cups = new CupNode();

        //    foreach (var c in input.Trim())
        //    {
        //        cups.AddLast(int.Parse(c.ToString()));
        //    }

        //    var maxCup = cups.Max();

        //    for (var i = maxCup + 1; i <= 1000000; i++)
        //    {
        //        cups.AddLast(i);
        //    }

        //    var currentCup = cups.First;

        //    for (var m = 0; m < 10000000; m++)
        //    {
        //        if (m % 100000 == 0)
        //        {
        //            Log($"{m}");
        //        }

        //        currentCup = MakeMove(currentCup, cups);
        //    }

        //    var cur = cups.Find(1);
        //    var result = (long)cur.NextCircular().Value * (long)cur.NextCircular().NextCircular().Value;

        //    return result.ToString();
        //}
    }

    public class CupNode
    {
        public int Value { get; set; }
        public CupNode Next { get; set; }
        public CupNode Previous { get; set; }
    }
}
