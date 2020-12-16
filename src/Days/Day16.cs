using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 16)]
    public class Day16 : BaseDay
    {
        public override string PartOne(string input)
        {
            var rules = ParseRules(input).ToList();
            var tickets = ParseTickets(input).ToList();
            var result = 0;

            foreach (var ticket in tickets)
            {
                var invalid = GetInvalidValues(ticket, rules);
                result += invalid.Sum();
            }

            return result.ToString();
        }

        private IEnumerable<List<(int min, int max)>> ParseRules(string input)
        {
            var rulesText = input.Paragraphs().First();

            foreach (var ruleText in rulesText.Lines().ToList())
            {
                var colonPos = ruleText.IndexOf(":");

                var valueText = ruleText.Substring(colonPos + 2);

                var parts = valueText.Split(new string[] { "-", " or " }, StringSplitOptions.RemoveEmptyEntries);

                var ranges = new List<(int min, int max)>();
                ranges.Add((int.Parse(parts[0]), int.Parse(parts[1])));
                ranges.Add((int.Parse(parts[2]), int.Parse(parts[3])));

                yield return ranges;
            }
        }

        private Dictionary<string, List<(int min, int max)>> ParseRules2(string input)
        {
            var rulesText = input.Paragraphs().First();
            var result = new Dictionary<string, List<(int min, int max)>>();

            foreach (var ruleText in rulesText.Lines().ToList())
            {
                var colonPos = ruleText.IndexOf(":");
                var ruleName = ruleText.Substring(0, colonPos);

                var valueText = ruleText.Substring(colonPos + 2);

                var parts = valueText.Split(new string[] { "-", " or " }, StringSplitOptions.RemoveEmptyEntries);

                var ranges = new List<(int min, int max)>();
                ranges.Add((int.Parse(parts[0]), int.Parse(parts[1])));
                ranges.Add((int.Parse(parts[2]), int.Parse(parts[3])));

                result.Add(ruleName, ranges);
            }

            return result;
        }

        private IEnumerable<List<int>> ParseTickets(string input)
        {
            var lines = input.Lines().ToList();

            var yourTicket = lines.IndexOf("your ticket:");

            for (var i = yourTicket + 4; i < lines.Count; i++)
            {
                yield return lines[i].Integers().ToList();
            }
        }

        private List<int> GetInvalidValues(List<int> ticket, List<List<(int min, int max)>> rules)
        {
            var result = new List<int>();

            foreach (var field in ticket)
            {
                var valid = false;

                foreach (var rule in rules)
                {
                    if (MatchesRule(rule, field))
                    {
                        valid = true;
                    }
                }

                if (!valid)
                {
                    result.Add(field);
                }
            }

            return result;
        }

        private List<int> GetInvalidValues2(List<int> ticket, Dictionary<string, List<(int min, int max)>> rules)
        {
            var result = new List<int>();

            foreach (var field in ticket)
            {
                var valid = false;

                foreach (var rule in rules)
                {
                    if (MatchesRule(rule.Value, field))
                    {
                        valid = true;
                    }
                }

                if (!valid)
                {
                    result.Add(field);
                }
            }

            return result;
        }

        private bool MatchesRule(IEnumerable<(int min, int max)> rule, int field)
        {
            foreach (var range in rule)
            {
                if (field >= range.min && field <= range.max)
                {
                    return true;
                }
            }

            return false;
        }

        public override string PartTwo(string input)
        {
            var rules = ParseRules2(input);
            var tickets = ParseTickets(input).ToList();
            var yourTicket = GetYourTicket(input);
            var validTickets = GetValidTickets(tickets, rules).ToList();
            var possibleFields = GetPossibleFields(rules, validTickets);

            var ruleFields = GetRuleFields(rules, possibleFields);

            return ((long)yourTicket[ruleFields["departure location"]] * (long)yourTicket[ruleFields["departure station"]] * (long)yourTicket[ruleFields["departure platform"]] * (long)yourTicket[ruleFields["departure track"]] * (long)yourTicket[ruleFields["departure date"]] * (long)yourTicket[ruleFields["departure time"]]).ToString();
        }

        private Dictionary<string, int> GetRuleFields(Dictionary<string, List<(int min, int max)>> rules, Dictionary<string, List<int>> possibleFields)
        {
            var result = new Dictionary<string, int>();

            if (possibleFields.Any(x => x.Value.Count == 0))
            {
                return null;
            }

            if (rules.Count == 1)
            {
                result.Add(rules.First().Key, possibleFields[rules.First().Key].First());
                return result;
            }

            var smallestRule = rules.WithMin(r => possibleFields[r.Key].Count).Key;

            foreach (var field in possibleFields[smallestRule])
            {
                var removedRule = rules[smallestRule];
                rules.Remove(smallestRule);

                var child = GetRuleFields(rules, UpdatePossibleFields(possibleFields, smallestRule, field));

                if (child != null)
                {
                    result.Add(smallestRule, field);

                    foreach (var x in child)
                    {
                        result.Add(x.Key, x.Value);
                    }

                    return result;
                }

                rules.Add(smallestRule, removedRule);
            }

            return null;
        }

        private Dictionary<string, List<int>> UpdatePossibleFields(Dictionary<string, List<int>> possibleFields, string rule, int field)
        {
            var result = new Dictionary<string, List<int>>();

            foreach (var possible in possibleFields)
            {
                if (possible.Key != rule)
                {
                    var fields = new List<int>(possible.Value);
                    
                    if (fields.Contains(field))
                    {
                        fields.Remove(field);
                    }

                    result.Add(possible.Key, fields);
                }
            }

            return result;
        }

        private Dictionary<string, List<int>> GetPossibleFields(Dictionary<string, List<(int min, int max)>> rules, List<List<int>> tickets)
        {
            var result = new Dictionary<string, List<int>>();

            foreach (var rule in rules)
            {
                var possible = new List<int>();

                for (var i = 0; i < tickets[0].Count(); i++)
                {
                    possible.Add(i);
                }

                foreach (var ticket in tickets)
                {
                    for (var i = 0; i < ticket.Count; i++)
                    {
                        if (!MatchesRule(rule.Value, ticket.ElementAt(i)))
                        {
                            possible.Remove(i);
                        }
                    }
                }

                result.Add(rule.Key, possible);
            }

            return result;
        }

        private List<int> FindFieldPos(List<(int min, int max)> rule, List<List<int>> tickets)
        {
            var possible = new List<int>();

            for (var i = 0; i < tickets[0].Count(); i++)
            {
                possible.Add(i);
            }

            foreach (var ticket in tickets)
            {
                for (var i = 0; i < ticket.Count(); i++)
                {
                    if (!MatchesRule(rule, ticket.ElementAt(i)))
                    {
                        possible.Remove(i);
                    }
                }
            }

            return possible;
        }

        private IEnumerable<List<int>> GetValidTickets(List<List<int>> tickets, Dictionary<string, List<(int min, int max)>> rules)
        {
            foreach (var ticket in tickets)
            {
                var invalid = GetInvalidValues2(ticket, rules);

                if (!invalid.Any())
                {
                    yield return ticket;
                }
            }
        }

        private string PrintList(List<int> items)
        {
            var result = "";

            foreach (var i in items)
            {
                result += i.ToString() + ", ";
            }

            return result;
        }

        

        private List<int> GetYourTicket(string input)
        {
            var lines = input.Lines().ToList();

            var yourTicket = lines.IndexOf("your ticket:");

            return lines[yourTicket + 1].Integers().ToList();
        }
    }
}
