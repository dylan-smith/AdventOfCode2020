using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 19)]
    public class Day19 : BaseDay
    {
        public override string PartOne(string input)
        {
            var rules = ParseRules(input.Paragraphs().First());
            var expanded = ExpandedRule.ExpandRule(0, rules);

            var messages = input.Paragraphs().Last().Lines().Select(line => line.Select(c => c == 'a' ? 2 : 118).ToList()).ToList();

            var result = messages.Count(m => expanded.MatchesMessage(m).Any(c => c == m.Count));

            return result.ToString();
        }

        private Dictionary<int, (int a, int b, int c, int d)> ParseRules(string input)
        {
            var result = new Dictionary<int, (int a, int b, int c, int d)>();

            foreach (var line in input.Lines())
            {
                if (!line.Contains("a") && !line.Contains("b"))
                {
                    var words = line.Words().ToList();

                    var number = int.Parse(words[0].ShaveRight(1));
                    var a = int.Parse(words[1]);
                    var b = -1;
                    var c = -1;
                    var d = -1;

                    if (!line.Contains("|"))
                    {
                        if (words.Count > 2)
                        {
                            b = int.Parse(words[2]);
                        }
                    }
                    else
                    {
                        var pipe = words.IndexOf("|");

                        if (pipe == 3)
                        {
                            b = int.Parse(words[2]);
                            c = int.Parse(words[4]);

                            if (words.Count > 5)
                            {
                                d = int.Parse(words[5]);
                            }
                        }
                        else
                        {
                            c = int.Parse(words[3]);

                            if (words.Count > 4)
                            {
                                d = int.Parse(words[4]);
                            }
                        }
                    }

                    result.Add(number, (a, b, c, d));
                }
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }

        // 2 and 118 are the terminal rules

        private class ExpandedRule
        {
            public List<ExpandedRule> Left { get; set; } = new List<ExpandedRule>();
            public List<ExpandedRule> Right { get; set; } = new List<ExpandedRule>();
            public int Value { get; set; } = -1;

            public static ExpandedRule ExpandRule(int ruleNumber, Dictionary<int, (int a, int b, int c, int d)> rules)
            {
                var result = new ExpandedRule();

                if (ruleNumber == 2 || ruleNumber == 118)
                {
                    result.Value = ruleNumber;
                    return result;
                }

                var rule = rules[ruleNumber];

                result.Left.Add(ExpandedRule.ExpandRule(rule.a, rules));

                if (rule.b != -1)
                {
                    result.Left.Add(ExpandedRule.ExpandRule(rule.b, rules));
                }

                if (rule.c != -1)
                {
                    result.Right.Add(ExpandedRule.ExpandRule(rule.c, rules));
                }

                if (rule.d != -1)
                {
                    result.Right.Add(ExpandedRule.ExpandRule(rule.d, rules));
                }

                return result;
            }

            public override string ToString()
            {
                if (Value != -1)
                {
                    return Value.ToString();
                }

                var result = "";

                foreach (var l in Left)
                {
                    result += $"({ l.ToString() }) ";
                }

                if (Right.Any())
                {
                    result += "| ";
                }

                foreach (var r in Right)
                {
                    result += $"({ r.ToString() }) ";
                }

                return result;
            }

            public List<int> MatchesMessage(List<int> message)
            {
                var leftResults = new List<int>();
                var results = new List<int>();
                var matchFound = true;

                if (Value != -1)
                {
                    if (message[0] == Value)
                    {
                        leftResults.Add(1);
                        return leftResults;
                    }

                    return leftResults;
                }

                foreach (var l in Left)
                {
                    if (!leftResults.Any())
                    {
                        var counts = l.MatchesMessage(message);

                        if (!counts.Any())
                        {
                            matchFound = false;
                            break;
                        }

                        leftResults = counts;
                    }
                    else
                    {
                        var newResults = new List<int>();

                        foreach (var r in leftResults)
                        {
                            var counts = l.MatchesMessage(message.Skip(r).ToList());

                            if (counts.Any())
                            {
                                foreach (var c in counts)
                                {
                                    newResults.Add(r + c);
                                }
                            }
                        }

                        if (!newResults.Any())
                        {
                            matchFound = false;
                            break;
                        }

                        leftResults = newResults;
                    }
                }

                if (matchFound)
                {
                    results.AddRange(leftResults);
                }

                var rightResults = new List<int>();
                matchFound = true;

                foreach (var r in Right)
                {
                    if (!rightResults.Any())
                    {
                        var counts = r.MatchesMessage(message);

                        if (!counts.Any())
                        {
                            matchFound = false;
                            break;
                        }

                        rightResults = counts;
                    }
                    else
                    {
                        var newResults = new List<int>();

                        foreach (var res in rightResults)
                        {
                            var counts = r.MatchesMessage(message.Skip(res).ToList());

                            if (counts.Any())
                            {
                                foreach (var c in counts)
                                {
                                    newResults.Add(res + c);
                                }
                            }
                        }

                        if (!newResults.Any())
                        {
                            matchFound = false;
                            break;
                        }

                        rightResults = newResults;
                    }
                }

                if (matchFound)
                {
                    results.AddRange(rightResults);
                }

                return results;
            }
        }
    }
}
