﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 6)]
    public class Day06 : BaseDay
    {
        private string _questions = "abcdefghijklmnopqrstuvwxyz";

        public override string PartOne(string input)
        {
            var result = input.Paragraphs().Sum(CountQuestionsWithAnyYesAnswers);

            return result.ToString();
        }

        private int CountQuestionsWithAnyYesAnswers(string group)
        {
            return _questions.Count(c => group.Contains(c));
        }

        private int CountQuestionsWithAllYesAnswers(string group)
        {
            var people = group.Lines().ToList();

            return _questions.Count(q => people.All(p => p.Contains(q)));
        }

        public override string PartTwo(string input)
        {
            var result = input.Paragraphs().Sum(CountQuestionsWithAllYesAnswers);

            return result.ToString();
        }
    }
}
