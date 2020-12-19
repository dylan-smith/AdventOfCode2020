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
            var expanded = new ExpandedRule(0, rules);

            var messages = input.Paragraphs()
                                .Last()
                                .ParseLines(line => line.Select(c => c == 'a' ? 2 : 118)
                                                        .ToList());

            var result = messages.Count(m => expanded.MatchesMessage(m).Any(c => c == m.Count));

            return result.ToString();
        }

        private Dictionary<int, (int a, int b, int c, int d, int e)> ParseRules(string input)
        {
            var result = new Dictionary<int, (int a, int b, int c, int d, int e)>();

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
                    var e = -1;

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

                            if (words.Count > 6)
                            {
                                e = int.Parse(words[6]);
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

                    result.Add(number, (a, b, c, d, e));
                }
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            var rules = ParseRules(input.Paragraphs().First());

            var eight = (42, -1, 42, 8, -1);
            var eleven = (42, 31, 42, 11, 31);

            rules[8] = eight;
            rules[11] = eleven;

            var expanded = new ExpandedRule(0, rules);

            var messages = input.Paragraphs()
                                .Last()
                                .ParseLines(line => line.Select(c => c == 'a' ? 2 : 118)
                                                        .ToList());

            var result = messages.Count(m => expanded.MatchesMessage(m).Any(c => c == m.Count));

            return result.ToString();
        }

        private class ExpandedRule
        {
            public int Value { get; set; } = -1;
            private (int a, int b, int c, int d, int e) _rule;
            private Dictionary<int, (int a, int b, int c, int d, int e)> _rules;

            public ExpandedRule(int ruleNumber, Dictionary<int, (int a, int b, int c, int d, int e)> rules)
            {
                if (ruleNumber == 2 || ruleNumber == 118)
                {
                    Value = ruleNumber;
                }
                else
                {
                    _rule = rules[ruleNumber];
                    _rules = rules;
                }
            }

            public IEnumerable<ExpandedRule> GetLeft()
            {
                yield return new ExpandedRule(_rule.a, _rules);

                if (_rule.b != -1)
                {
                    yield return new ExpandedRule(_rule.b, _rules);
                }
            }

            public IEnumerable<ExpandedRule> GetRight()
            {
                if (_rule.c != -1)
                {
                    yield return new ExpandedRule(_rule.c, _rules);
                }

                if (_rule.d != -1)
                {
                    yield return new ExpandedRule(_rule.d, _rules);
                }

                if (_rule.e != -1)
                {
                    yield return new ExpandedRule(_rule.e, _rules);
                }
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

                foreach (var l in GetLeft())
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
                            if (message.Count > r)
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

                foreach (var r in GetRight())
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
                            if (message.Count > res)
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
