using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 21)]
    public class Day21 : BaseDay
    {
        public override string PartOne(string input)
        {
            var foods = input.ParseLines(ParseFood).ToList();

            var allIngredients = foods.SelectMany(f => f.ingredients).Distinct().ToList();
            var allAllergens = foods.SelectMany(f => f.allergens).Distinct().ToList();

            var possible = new Dictionary<string, HashSet<string>>();

            foreach (var a in allAllergens)
            {
                possible.Add(a, new HashSet<string>(allIngredients));
            }

            foreach (var food in foods)
            {
                foreach (var allergen in food.allergens)
                {
                    var toRemove = new List<string>();

                    foreach (var ingredient in possible[allergen])
                    {
                        if (!food.ingredients.Contains(ingredient))
                        {
                            toRemove.Add(ingredient);
                        }
                    }

                    foreach (var r in toRemove)
                    {
                        possible[allergen].Remove(r);
                    }
                }
            }

            foreach (var a in possible)
            {
                foreach (var i in a.Value)
                {
                    allIngredients.Remove(i);
                }
            }

            var result = 0;

            foreach (var food in foods)
            {
                foreach (var i in food.ingredients)
                {
                    if (allIngredients.Contains(i))
                    {
                        result++;
                    }
                }
            }

            return result.ToString();
        }

        private (List<string> ingredients, List<string> allergens) ParseFood(string line)
        {
            var paren = line.IndexOf('(');

            var ingredients = new string(line.Take(paren).ToArray()).Words().ToList();
            var allergensText = new string(line.Skip(paren + 1).ToArray());

            var allergens = allergensText.Split(new string[] { "(", "contains ", " ", ",", ")" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return (ingredients, allergens);
        }

        public override string PartTwo(string input)
        {
            var foods = input.ParseLines(ParseFood).ToList();

            var allIngredients = foods.SelectMany(f => f.ingredients).Distinct().ToList();
            var allAllergens = foods.SelectMany(f => f.allergens).Distinct().ToList();

            var possible = new Dictionary<string, HashSet<string>>();

            foreach (var a in allAllergens)
            {
                possible.Add(a, new HashSet<string>(allIngredients));
            }

            foreach (var food in foods)
            {
                foreach (var allergen in food.allergens)
                {
                    var toRemove = new List<string>();

                    foreach (var ingredient in possible[allergen])
                    {
                        if (!food.ingredients.Contains(ingredient))
                        {
                            toRemove.Add(ingredient);
                        }
                    }

                    foreach (var r in toRemove)
                    {
                        possible[allergen].Remove(r);
                    }
                }
            }

            var result = new List<(string allergen, string ingredient)>();

            while (possible.Any())
            {
                var minAllergen = possible.First(p => p.Value.Count == 1);
                var ingredient = minAllergen.Value.First();

                result.Add((minAllergen.Key, ingredient));
                possible.Remove(minAllergen.Key);

                foreach (var p in possible)
                {
                    p.Value.Remove(ingredient);
                }
            }

            result = result.OrderBy(x => x.allergen).ToList();

            var canonical = string.Join(',', result.Select(r => r.ingredient));

            return canonical;
        }
    }
}
