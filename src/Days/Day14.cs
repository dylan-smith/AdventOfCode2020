using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    [Day(2020, 14)]
    public class Day14 : BaseDay
    {
        public override string PartOne(string input)
        {
            var memory = new Dictionary<long, long>();
            var program = ParseInput(input);

            foreach (var chunk in program)
            {
                foreach (var memset in chunk.memsets)
                {
                    memory.SafeSet(memset.address, ApplyMask(memset.value, chunk.mask));
                }
            }

            return memory.Sum(x => x.Value).ToString();
        }

        private List<(string mask, List<(long address, long value)> memsets)> ParseInput(string input)
        {
            var lines = input.Lines().ToList();
            var curMask = "";
            var memsets = new List<(long address, long value)>();

            var result = new List<(string mask, List<(long address, long value)> memsets)>();

            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    if (memsets.Any())
                    {
                        result.Add((curMask, memsets));
                    }

                    curMask = line.ShaveLeft("mask = ");
                    memsets = new List<(long address, long value)>();
                }

                if (line.StartsWith("mem["))
                {
                    var parts = line.Split(new string[] { "mem[", "]", " = " }, StringSplitOptions.RemoveEmptyEntries);
                    memsets.Add((long.Parse(parts[0]), long.Parse(parts[1])));
                }
            }

            if (memsets.Any())
            {
                result.Add((curMask, memsets));
            }

            return result;
        }

        private long ApplyMask(long value, string mask)
        {
            var orMask = Convert.ToInt64(mask.Replace('X', '0'), 2);
            value |= orMask;

            var andMask = Convert.ToInt64(mask.Replace('X', '1'), 2);
            value &= andMask;

            return value;
        }

        public override string PartTwo(string input)
        {
            var memory = new Dictionary<long, long>();
            var program = ParseInput(input);

            foreach (var chunk in program)
            {
                foreach (var memset in chunk.memsets)
                {
                    UpdateMemory(memory, memset.value, memset.address, chunk.mask);
                    Log($"Done Line! {chunk.mask}");
                }
            }

            //var memoryInstructions = new List<(string address, long value)>();

            //foreach (var chunk in program)
            //{
            //    foreach (var memset in chunk.memsets)
            //    {
            //        memoryInstructions.Add((ApplyFloatingMask(memset.address, chunk.mask), memset.value));
            //    }
            //}



            //Log($"{floatingAddresses.Count}");
            //Log($"{floatingAddresses.Distinct().Count()}");

            //var newAddresses = PruneAddresses(floatingAddresses);

            //floatingAddresses.ForEach(x => Log($"{x.Count(c => c == 'X')}"));

            //var count = floatingAddresses.Sum(x => Math.Pow(2, x.Count(c => c == 'X')));

            //Log($"{count}");

            return memory.Sum(x => x.Value).ToString();
        }

        private List<string> PruneAddresses(List<string> floatingAddresses)
        {
            var result = new List<string>();

            for (var i = 0; i < floatingAddresses.Count; i++)
            {
                var good = true;

                for (var k = i + 1; k < floatingAddresses.Count && good; k++)
                {
                    var match = true;

                    for (var c = 0; c < floatingAddresses[i].Length; c++)
                    {
                        if (floatingAddresses[k][c] != floatingAddresses[i][c])
                        {
                            if (floatingAddresses[k][c] != 'X')
                            {
                                match = false;
                            }
                        }
                    }

                    if (match)
                    {
                        good = false;
                    }
                }

                if (good)
                {
                    result.Add(floatingAddresses[i]);
                }
            }

            return result;
        }

        private void UpdateMemory(Dictionary<long, long> memory, long value, long address, string mask)
        {
            var floatingAddress = ApplyFloatingMask(address, mask);
            //var allAddresses = GetAllAddresses(floatingAddress.ToCharArray(), memory, value).ToList();
            GetAllAddresses(floatingAddress.ToCharArray(), memory, value);

            //foreach (var a in allAddresses)
            //{
            //    memory.SafeSet(a, value);
            //}
        }

        //private IEnumerable<long> GetAllAddresses(string address)
        //{
        //    for (var i = 0; i < address.Count(); i++)
        //    {
        //        if (address[i] == 'X')
        //        {
        //            var newAddress = address.ToCharArray();

        //            newAddress[i] = '1';
        //            var ones = GetAllAddresses(new string(newAddress));

        //            newAddress[i] = '0';
        //            var zeroes = GetAllAddresses(new string(newAddress));

        //            foreach (var one in ones)
        //            {
        //                yield return one;
        //            }

        //            foreach (var zero in zeroes)
        //            {
        //                yield return zero;
        //            }
        //        }
        //    }

        //    if (!address.Any(c => c == 'X'))
        //    {
        //        yield return Convert.ToInt64(address, 2);
        //    }
        //}

        private void GetAllAddresses(char[] address, Dictionary<long, long> memory, long value)
        {
            for (var i = 0; i < address.Length; i++)
            {
                if (address[i] == 'X')
                {
                    address[i] = '1';
                    GetAllAddresses(address, memory, value);

                    address[i] = '0';
                    GetAllAddresses(address, memory, value);
                    address[i] = 'X';
                }
            }

            if (!address.Any(c => c == 'X'))
            {
                //yield return Convert.ToInt64(new string(address), 2);
                memory.SafeSet(Convert.ToInt64(new string(address), 2), value);
            }
        }

        private string ApplyFloatingMask(long address, string mask)
        {
            var orMask = Convert.ToInt64(mask.Replace('X', '0'), 2);
            address |= orMask;

            var result = Convert.ToString(address, 2).PadLeft(36, '0').ToCharArray();

            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                {
                    result[i] = 'X';
                }
            }

            return new string(result);
        }
    }
}
