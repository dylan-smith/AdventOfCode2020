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
            var result = input.Paragraphs().Sum(CountQuestionsWithAnyYesAnswers);

            return result.ToString();
        }

        private int CountQuestionsWithAnyYesAnswers(string group)
        {
            return group.RemoveWhitespace().Distinct().Count();
        }

        private int CountQuestionsWithAllYesAnswers(string group)
        {
            var people = group.Lines().ToList();
            var questions = group.RemoveWhitespace().Distinct().ToList();

            return questions.Count(q => people.All(p => p.Contains(q)));
        }

        public override string PartTwo(string input)
        {
            var result = input.Paragraphs().Sum(CountQuestionsWithAllYesAnswers);

            return result.ToString();
        }
    }
}
