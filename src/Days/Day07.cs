using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 7)]
    public class Day07 : BaseDay
    {
        public override string PartOne(string input)
        {
            var rules = input.ParseLines(ParseRule).ToList();
            var target = "shiny gold";

            var result = new List<string>();

            var bags = GetBagsThatContain(new List<string>() { target }, rules);

            while (bags.Any())
            {
                result.AddRange(bags);

                result = result.Distinct().ToList();

                bags = GetBagsThatContain(bags, rules).Except(result).ToList();
            }

            return result.Count().ToString();
        }

        private List<string> GetBagsThatContain(List<string> bags, List<(string Bag, Dictionary<string, int> Contents)> rules)
        {
            var result = new List<string>();

            foreach (var bag in bags)
            {
                result.AddRange(rules.Where(r => r.Contents.Any(c => c.Key == bag)).Select(r => r.Bag));
            }

            return result;
        }

        private (string Bag, Dictionary<string, int> Contents) ParseRule(string line)
        {
            var words = line.Words().ToList();

            var bag = $"{words[0]} {words[1]}";
            var contents = new Dictionary<string, int>();

            for (var i = 4; i < words.Count(); i += 4)
            {
                if (words[i] != "no")
                {
                    contents.Add($"{words[i + 1]} {words[i + 2]}", int.Parse(words[i]));
                }
            }

            return (bag, contents);
        }

        public override string PartTwo(string input)
        {
            var rules = input.ParseLines(ParseRule).ToList();
            var target = "shiny gold";

            var result = GetBagCount(target, rules) - 1;

            return result.ToString();
        }

        private int GetBagCount(string target, List<(string Bag, Dictionary<string, int> Contents)> rules)
        {
            var result = 1;

            foreach (var child in rules.Single(r => r.Bag == target).Contents)
            {
                result += child.Value * GetBagCount(child.Key, rules);
            }

            return result;
        }
    }
}
