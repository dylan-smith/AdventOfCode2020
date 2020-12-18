using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 18)]
    public class Day18 : BaseDay
    {
        public override string PartOne(string input)
        {
            var result = input.Lines().Sum(EvaluateExpression1);

            return result.ToString();
        }

        private long EvaluateExpression1(string expression)
        {
            var parts = expression.Strip(" ").Select(c => c.ToString()).ToList();

            while (parts.Any(p => p == ")"))
            {
                var closing = parts.IndexOf(")");
                var subExpression = parts.Take(closing).ToList();
                var opening = subExpression.LastIndexOf("(");
                subExpression = subExpression.Skip(opening + 1).ToList();

                var result = EvaluateArithmetic1(subExpression);

                parts = parts.Take(opening).Concat(new List<string>() { result.ToString() }).Concat(parts.Skip(closing + 1)).ToList();
            }

            return EvaluateArithmetic1(parts);
        }

        private long EvaluateArithmetic1(List<string> expression)
        {
            var result = long.Parse(expression[0]);

            for (var i = 0; i < expression.Count; i++)
            {
                if (expression[i] == "+")
                {
                    result += long.Parse(expression[i + 1]);
                }
                else if (expression[i] == "*")
                {
                    result *= long.Parse(expression[i + 1]);
                }
            }

            return result;
        }

        private long EvaluateExpression2(string expression)
        {
            var parts = expression.Strip(" ").Select(c => c.ToString()).ToList();

            while (parts.Any(p => p == ")"))
            {
                var closing = parts.IndexOf(")");
                var subExpression = parts.Take(closing).ToList();
                var opening = subExpression.LastIndexOf("(");
                subExpression = subExpression.Skip(opening + 1).ToList();

                var result = EvaluateArithmetic2(subExpression);

                parts = parts.Take(opening).Concat(new List<string>() { result.ToString() }).Concat(parts.Skip(closing + 1)).ToList();
            }

            return EvaluateArithmetic2(parts);
        }

        private long EvaluateArithmetic2(List<string> expression)
        {
            while (expression.Any(x => x == "+"))
            {
                var opIdx = expression.IndexOf("+");

                expression[opIdx - 1] = (long.Parse(expression[opIdx - 1]) + long.Parse(expression[opIdx + 1])).ToString();
                expression.RemoveAt(opIdx);
                expression.RemoveAt(opIdx);
            }

            while (expression.Any(x => x == "*"))
            {
                var opIdx = expression.IndexOf("*");

                expression[opIdx - 1] = (long.Parse(expression[opIdx - 1]) * long.Parse(expression[opIdx + 1])).ToString();
                expression.RemoveAt(opIdx);
                expression.RemoveAt(opIdx);
            }

            return long.Parse(expression[0]);
        }

        public override string PartTwo(string input)
        {
            var result = input.Lines().Sum(EvaluateExpression2);

            return result.ToString();
        }
    }
}
