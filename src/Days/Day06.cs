using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 6)]
    public class Day06 : BaseDay
    {
        public override string PartOne(string input)
        {
            var groups = input.Paragraphs().Select(p => CountYesAnswers(p)).Sum();

            return groups.ToString();
        }

        private int CountYesAnswers(string group)
        {
            var questions = new HashSet<char>();
            foreach (var c in group)
            {
                if (c != '\n' && c != '\r')
                {
                    questions.Add(c);
                }
            }

            return questions.Count();
        }

        private int CountYesAnswers2(string group)
        {
            var questions = new HashSet<char>();

            foreach (var c in group)
            {
                if (c != '\n' && c != '\r')
                {
                    questions.Add(c);
                }
            }

            foreach (var line in group.Lines())
            {
                var goodQuestions = new HashSet<char>();

                foreach (var q in questions)
                {
                    if (line.Contains(q))
                    {
                        goodQuestions.Add(q);
                    }
                }

                questions = goodQuestions;
            }

            return questions.Count();
        }

        public override string PartTwo(string input)
        {
            var groups = input.Paragraphs().Select(p => CountYesAnswers2(p)).Sum();

            return groups.ToString();
        }
    }
}
