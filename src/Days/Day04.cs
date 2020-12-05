using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 4)]
    public class Day04 : BaseDay
    {
        public override string PartOne(string input)
        {
            var passports = input.Paragraphs().Select(ParsePassport);

            return passports.Count(HasAllFields).ToString();
        }

        private Dictionary<string, string> ParsePassport(string input)
        {
            var passport = new Dictionary<string, string>();

            foreach (var word in input.Words())
            {
                var fields = word.Split(':');

                passport.Add(fields[0], fields[1]);
            }

            return passport;
        }

        private bool HasAllFields(Dictionary<string, string> passport)
        {
            var requiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            return requiredFields.All(f => passport.ContainsKey(f));
        }

        public override string PartTwo(string input)
        {
            var passports = input.Paragraphs().Select(ParsePassport);

            return passports.Count(IsValidPassport).ToString();
        }

        private bool IsValidPassport(Dictionary<string, string> passport)
        {
            if (!HasAllFields(passport))
            {
                return false;
            }

            return IsByrValid(passport["byr"]) &&
                   IsIyrValid(passport["iyr"]) &&
                   IsEyrValid(passport["eyr"]) &&
                   IsHgtValid(passport["hgt"]) &&
                   IsHclValid(passport["hcl"]) &&
                   IsEclValid(passport["ecl"]) &&
                   IsPidValid(passport["pid"]);
        }

        private bool IsByrValid(string byr)
        {
            return IsValidYear(byr, 1920, 2002);
        }

        private bool IsIyrValid(string iyr)
        {
            return IsValidYear(iyr, 2010, 2020);
        }

        private bool IsEyrValid(string eyr)
        {
            return IsValidYear(eyr, 2020, 2030);
        }

        private bool IsValidYear(string input, int min, int max)
        {
            if (input.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(input, out var value))
            {
                return false;
            }

            if (value < min || value > max)
            {
                return false;
            }

            return true;
        }

        private bool IsHgtValid(string hgt)
        {
            var units = hgt.Substring(hgt.Length - 2);
            hgt = hgt.ShaveRight(2);

            if (!int.TryParse(hgt, out var value))
            {
                return false;
            }

            if (units == "cm")
            {
                if (value < 150 || value > 193)
                {
                    return false;
                }
            }
            else if (units == "in")
            {
                if (value < 59 || value > 76)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool IsHclValid(string hcl)
        {
            if (!hcl.StartsWith("#"))
            {
                return false;
            }

            if (hcl.Length != 7)
            {
                return false;
            }

            return hcl.ShaveLeft("#").IsHex();
        }

        private bool IsEclValid(string ecl)
        {
            var valid = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            return valid.Any(v => v == ecl);
        }

        private bool IsPidValid(string pid)
        {
            if (pid.Length != 9)
            {
                return false;
            }

            if (!int.TryParse(pid, out int value))
            {
                return false;
            }

            return true;
        }
    }
}
