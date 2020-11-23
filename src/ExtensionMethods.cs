using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public enum Direction
    {
        Down,
        Up,
        Right,
        Left
    }

    public enum Compass
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionExtensions
    {
        public static Direction TurnLeft(this Direction dir)
        {
            return dir switch
            {
                Direction.Up => Direction.Left,
                Direction.Left => Direction.Down,
                Direction.Down => Direction.Right,
                Direction.Right => Direction.Up,
                _ => throw new ArgumentException("Unexpected value for Direction")
            };
        }

        public static Direction TurnRight(this Direction dir)
        {
            return dir switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new ArgumentException("Unexpected value for Direction")
            };
        }
    }

    public static class PermutationExtensions
    {
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();

            var factorials = Enumerable.Range(0, array.Length + 1)
                .Select(Factorial)
                .ToArray();

            for (var i = 0L; i < factorials[array.Length]; i++)
            {
                var sequence = GenerateSequence(i, array.Length - 1, factorials);

                yield return GeneratePermutation(array, sequence);
            }
        }

        public static IEnumerable<string> GetPermutations<T>(this string source)
        {
            return source.ToCharArray().GetPermutations().Select(x => new string(x.ToArray()));
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var clone = (T[])array.Clone();

            for (int i = 0; i < clone.Length - 1; i++)
            {
                Swap(ref clone[i], ref clone[i + sequence[i]]);
            }

            return clone;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var facto = factorials[sequence.Length - j];

                sequence[j] = (int)(number / facto);
                number = (int)(number % facto);
            }

            return sequence;
        }

        public static IEnumerable<string> GetCombinations(this string source)
        {
            return source.ToCharArray().GetCombinations().Select(c => new string(c.ToArray()));
        }

        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> source)
        {
            var result = new List<IEnumerable<T>>();

            for (var i = 1; i <= source.Count(); i++)
            {
                result.AddRange(source.GetCombinations(i));
            }

            return result;
        }

        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> source, int length)
        {
            var result = new List<List<T>>();

            if (length == 1)
            {
                return source.Select(x => new List<T>(1) { x });
            }

            for (var i = 0; i < source.Count(); i++)
            {
                var subList = source.Take(i).Concat(source.Skip(i + 1));

                var subCombos = subList.GetCombinations(length - 1);

                foreach (var c in subCombos)
                {
                    var newCombo = new List<T>(length);
                    newCombo.Add(source.ElementAt(i));
                    newCombo.AddRange(c);
                    result.Add(newCombo);
                }
            }

            return result;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private static long Factorial(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }
    }

    public static class StringManipulationExtensions
    {
        public static StringBuilder SwapPositions(this StringBuilder source, int x, int y)
        {
            var xChar = source[x];
            var yChar = source[y];

            source = source.Remove(x, 1);
            source = source.Insert(x, yChar);

            source = source.Remove(y, 1);
            source = source.Insert(y, xChar);

            return source;
        }

        public static string SwapPositions(this string source, int x, int y)
        {
            return new StringBuilder(source).SwapPositions(x, y).ToString();
        }

        public static StringBuilder RotateLeft(this StringBuilder source)
        {
            var startChar = source[0];

            source.Remove(0, 1);
            source.Insert(source.Length, startChar);

            return source;
        }

        public static StringBuilder RotateRight(this StringBuilder source)
        {
            var endChar = source[source.Length - 1];

            source.Remove(source.Length - 1, 1);
            source.Insert(0, endChar);

            return source;
        }

        public static string RotateLeft(this string source)
        {
            return new StringBuilder(source).RotateLeft().ToString();
        }

        public static string RotateRight(this string source)
        {
            return new StringBuilder(source).RotateRight().ToString();
        }

        public static StringBuilder RotateRight(this StringBuilder source, int rotateCount)
        {
            for (var i = 0; i < rotateCount; i++)
            {
                source.RotateRight();
            }

            return source;
        }

        public static StringBuilder RotateLeft(this StringBuilder source, int rotateCount)
        {
            for (var i = 0; i < rotateCount; i++)
            {
                source.RotateLeft();
            }

            return source;
        }

        public static string RotateRight(this string source, int rotateCount)
        {
            for (var i = 0; i < rotateCount; i++)
            {
                source.RotateRight();
            }

            return source;
        }

        public static string RotateLeft(this string source, int rotateCount)
        {
            for (var i = 0; i < rotateCount; i++)
            {
                source.RotateLeft();
            }

            return source;
        }

        public static string ReverseString(this string source)
        {
            return new string(source.Reverse().ToArray());
        }

        public static IEnumerable<string> Lines(this string input)
        {
            return input.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> Words(this string input)
        {
            return input.Split(new string[] { " ", "\t", Environment.NewLine, ",", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<int> Integers(this string input)
        {
            return input.Words().Select(x => int.Parse(x)).ToList();
        }

        public static IEnumerable<long> Longs(this string input)
        {
            return input.Words().Select(x => long.Parse(x)).ToList();
        }

        public static IEnumerable<double> Doubles(this string input)
        {
            return input.Words().Select(x => double.Parse(x)).ToList();
        }

        public static bool IsAnagram(this string a, string b)
        {
            return a.ToCharArray().UnorderedEquals(b.ToCharArray());
        }

        public static string ShaveLeft(this string a, int characters)
        {
            return a.Substring(characters);
        }

        public static string ShaveLeft(this string a, string shave)
        {
            var result = a;

            while (result.StartsWith(shave))
            {
                result = result.Substring(shave.Length);
            }

            return result;
        }

        public static string ShaveRight(this string a, int characters)
        {
            return a.Substring(0, a.Length - characters);
        }

        public static string ShaveRight(this string a, string shave)
        {
            var result = a;

            while (result.EndsWith(shave))
            {
                result = result.Substring(0, result.Length - shave.Length);
            }

            return result;
        }

        public static string Shave(this string a, int characters)
        {
            return a.Substring(characters, a.Length - (characters * 2));
        }

        public static string Shave(this string a, string shave)
        {
            var result = a;

            while (result.StartsWith(shave))
            {
                result = result.Substring(shave.Length);
            }

            while (result.EndsWith(shave))
            {
                result = result.Substring(0, result.Length - shave.Length);
            }

            return result;
        }

        public static string Strip(this string a, params string[] remove)
        {
            var result = a;

            while (remove.Any(x => result.Contains(x)))
            {
                var r = remove.First(x => result.Contains(x));

                result = result.Remove(result.IndexOf(r), r.Length);
            }

            return result;
        }

        public static string HexToBinary(this string hex)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in hex.ToCharArray())
            {
                var intValue = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
                sb.Append(Convert.ToString(intValue, 2).PadLeft(4, '0'));
            }

            return sb.ToString();
        }

        public static string Overlap(this string a, string b)
        {
            return new string(Overlap<char>(a, b).ToArray());
        }

        public static IEnumerable<T> Overlap<T>(this IEnumerable<T> a, IEnumerable<T> b) where T : IEquatable<T>
        {
            var result = new List<T>();
            var c = a.ToList();
            var d = b.ToList();

            for (var x = 0; x < Math.Min(c.Count, d.Count); x++)
            {
                if (c[x].Equals(d[x]))
                {
                    result.Add(c[x]);
                }
            }

            return result;
        }

        public static void Deconstruct<T>(this IEnumerable<T> list, out T first)
        {
            first = list.Count() > 0 ? list.ElementAt(0) : default(T); // or throw
        }

        public static void Deconstruct<T>(this IEnumerable<T> list, out T first, out T second)
        {
            first = list.Count() > 0 ? list.ElementAt(0) : default(T); // or throw
            second = list.Count() > 1 ? list.ElementAt(1) : default(T); // or throw
        }

        public static void Deconstruct<T>(this IEnumerable<T> list, out T first, out T second, out T third)
        {
            first = list.Count() > 0 ? list.ElementAt(0) : default(T); // or throw
            second = list.Count() > 1 ? list.ElementAt(1) : default(T); // or throw
            third = list.Count() > 2 ? list.ElementAt(2) : default(T); // or throw
        }

        public static void Deconstruct<T>(this IEnumerable<T> list, out T first, out T second, out T third, out T fourth)
        {
            first = list.Count() > 0 ? list.ElementAt(0) : default(T); // or throw
            second = list.Count() > 1 ? list.ElementAt(1) : default(T); // or throw
            third = list.Count() > 2 ? list.ElementAt(2) : default(T); // or throw
            fourth = list.Count() > 3 ? list.ElementAt(3) : default(T); // or throw
        }

        public static void Deconstruct<T>(this IEnumerable<T> list, out T first, out T second, out T third, out T fourth, out T fifth)
        {
            first = list.Count() > 0 ? list.ElementAt(0) : default(T); // or throw
            second = list.Count() > 1 ? list.ElementAt(1) : default(T); // or throw
            third = list.Count() > 2 ? list.ElementAt(2) : default(T); // or throw
            fourth = list.Count() > 3 ? list.ElementAt(3) : default(T); // or throw
            fifth = list.Count() > 4 ? list.ElementAt(4) : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth, out T fifth)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
            fifth = list.Count > 4 ? list[4] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
            fifth = list.Count > 4 ? list[4] : default(T); // or throw
            sixth = list.Count > 5 ? list[5] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth, out T seventh)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
            fifth = list.Count > 4 ? list[4] : default(T); // or throw
            sixth = list.Count > 5 ? list[5] : default(T); // or throw
            seventh = list.Count > 6 ? list[6] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth, out T seventh, out T eigth)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
            fifth = list.Count > 4 ? list[4] : default(T); // or throw
            sixth = list.Count > 5 ? list[5] : default(T); // or throw
            seventh = list.Count > 6 ? list[6] : default(T); // or throw
            eigth = list.Count > 7 ? list[7] : default(T); // or throw
        }

        public static Direction ToDirection(this char c)
        {
            return c switch
            {
                'R' => Direction.Right,
                'D' => Direction.Down,
                'U' => Direction.Up,
                'L' => Direction.Left,
                _ => throw new ArgumentException($"Unrecognized character [{c}]")
            };
        }

        public static Compass ToCompass(this char c)
        {
            return c switch
            {
                'N' => Compass.North,
                'S' => Compass.South,
                'E' => Compass.East,
                'W' => Compass.West,
                _ => throw new ArgumentException($"Unrecognized character [{c}]")
            };
        }
    }

    public static class ImageHelper
    {
        public static void CreateBitmap(int width, int height, string filePath, Func<int, int, Color> getPixel)
        {
            using var img = new Bitmap(width, height);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    img.SetPixel(x, y, getPixel(x, y));
                }
            }

            img.Save(filePath);
        }

        public static void CreateBitmap(Color[,] pixels, string filePath)
        {
            var width = pixels.GetUpperBound(0);
            var height = pixels.GetUpperBound(1);

            using var img = new Bitmap(width, height);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    img.SetPixel(x, y, pixels[x, y]);
                }
            }

            img.Save(filePath);
        }
    }

    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                @this.Add(element);
            }
        }

        public static bool UnorderedEquals<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a.Count() != b.Count())
            {
                return false;
            }

            var sortedA = a.OrderBy(x => x);
            var sortedB = b.OrderBy(x => x);

            return sortedA.SequenceEqual(sortedB);
        }

        public static void ForEach<T>(this T[,] a, Action<int, int> action)
        {
            for (var x = a.GetLowerBound(0); x <= a.GetUpperBound(0); x++)
            {
                for (var y = a.GetLowerBound(1); y <= a.GetUpperBound(1); y++)
                {
                    action(x, y);
                }
            }
        }

        public static void ForEach<T>(this T[,] a, Action<T> action)
        {
            for (var x = a.GetLowerBound(0); x <= a.GetUpperBound(0); x++)
            {
                for (var y = a.GetLowerBound(1); y <= a.GetUpperBound(1); y++)
                {
                    action(a[x, y]);
                }
            }
        }

        public static IEnumerable<(int index, T item)> SelectWithIndex<T>(this IEnumerable<T> a)
        {
            var list = a.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                yield return (i, list[i]);
            }
        }

        public static IEnumerable<T> ToList<T>(this T[,] a)
        {
            for (var x = a.GetLowerBound(0); x <= a.GetUpperBound(0); x++)
            {
                for (var y = a.GetLowerBound(1); y <= a.GetUpperBound(1); y++)
                {
                    yield return a[x, y];
                }
            }
        }

        public static void ForEach<T>(this T[,,] a, Action<int, int, int> action)
        {
            for (var x = a.GetLowerBound(0); x <= a.GetUpperBound(0); x++)
            {
                for (var y = a.GetLowerBound(1); y <= a.GetUpperBound(1); y++)
                {
                    for (var z = a.GetLowerBound(2); z <= a.GetUpperBound(2); z++)
                    {
                        action(x, y, z);
                    }
                }
            }
        }

        public static void ForEach<T>(this T[,,] a, Action<T> action)
        {
            for (var x = a.GetLowerBound(0); x <= a.GetUpperBound(0); x++)
            {
                for (var y = a.GetLowerBound(1); y <= a.GetUpperBound(1); y++)
                {
                    for (var z = a.GetLowerBound(2); z <= a.GetUpperBound(2); z++)
                    {
                        action(a[x, y, z]);
                    }
                }
            }
        }

        public static IEnumerable<T> ToList<T>(this T[,,] a)
        {
            for (var x = a.GetLowerBound(0); x <= a.GetUpperBound(0); x++)
            {
                for (var y = a.GetLowerBound(1); y <= a.GetUpperBound(1); y++)
                {
                    for (var z = a.GetLowerBound(2); z <= a.GetUpperBound(2); z++)
                    {
                        yield return a[x, y, z];
                    }
                }
            }
        }

        public static int IndexOf<T>(this T[] a, T b)
        {
            for (var i = a.GetLowerBound(0); i <= a.GetUpperBound(0); i++)
            {
                if (a[i].Equals(b))
                {
                    return i;
                }
            }

            return -1;
        }

        public static T WithMin<T>(this IEnumerable<T> a, Func<T, int> selector)
        {
            var min = a.Min(selector);
            return a.First(x => selector(x) == min);
        }

        public static T WithMin<T>(this IEnumerable<T> a, Func<T, long> selector)
        {
            var min = a.Min(selector);
            return a.First(x => selector(x) == min);
        }

        public static T WithMin<T>(this IEnumerable<T> a, Func<T, double> selector)
        {
            var min = a.Min(selector);
            return a.First(x => selector(x) == min);
        }

        public static T WithMax<T>(this IEnumerable<T> a, Func<T, int> selector)
        {
            var max = a.Max(selector);
            return a.First(x => selector(x) == max);
        }

        public static T WithMax<T>(this IEnumerable<T> a, Func<T, long> selector)
        {
            var max = a.Max(selector);
            return a.First(x => selector(x) == max);
        }

        public static T WithMax<T>(this IEnumerable<T> a, Func<T, double> selector)
        {
            var max = a.Max(selector);
            return a.First(x => selector(x) == max);
        }

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> a)
        {
            while (true)
            {
                foreach (var x in a)
                {
                    yield return x;
                }
            }
        }

        public static void SafeIncrement<TKey>(this Dictionary<TKey, int> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict.Add(key, 1);
            }
        }

        public static void SafeIncrement<TKey>(this Dictionary<TKey, int> dict, TKey key, int amount)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += amount;
            }
            else
            {
                dict.Add(key, amount);
            }
        }

        public static void SafeDecrement<TKey>(this Dictionary<TKey, int> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict.Add(key, -1);
            }
        }

        public static void SafeSet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static bool SafeCompare<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key].Equals(value);
            }

            return false;
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            list.ToList().ForEach(action);
        }

        public static LinkedListNode<T> PreviousCircular<T>(this LinkedListNode<T> node)
        {
            return node.Previous ?? node.List.Last;
        }

        public static LinkedListNode<T> PreviousCircular<T>(this LinkedListNode<T> node, int hops)
        {
            var result = node;

            Enumerable.Range(0, hops).ForEach(x => result = result.PreviousCircular());

            return result;
        }

        public static LinkedListNode<T> NextCircular<T>(this LinkedListNode<T> node)
        {
            return node.Next ?? node.List.First;
        }

        public static LinkedListNode<T> NextCircular<T>(this LinkedListNode<T> node, int hops)
        {
            var result = node;

            Enumerable.Range(0, hops).ForEach(x => result = result.NextCircular());

            return result;
        }

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> collection)
        {
            collection.ForEach(x => set.Add(x));
        }

        public static void AddMany<T>(this IList<T> list, T item, int count)
        {
            for (var i = 0; i < count; i++)
            {
                list.Add(item);
            }
        }

        public static long LeastCommonMultiple(this IEnumerable<long> numbers)
        {
            return numbers.Aggregate(LeastCommonMultiple);
        }

        public static long LeastCommonMultiple(long a, long b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }

        public static long GreatestCommonDivisor(long a, long b)
        {
            return b == 0 ? a : GreatestCommonDivisor(b, a % b);
        }

        public static int LeastCommonMultiple(this IEnumerable<int> numbers)
        {
            return numbers.Aggregate(LeastCommonMultiple);
        }

        public static int LeastCommonMultiple(int a, int b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }

        public static int GreatestCommonDivisor(int a, int b)
        {
            return b == 0 ? a : GreatestCommonDivisor(b, a % b);
        }

        public static void Initialize<T>(this IList<T> list, T value, int count)
        {
            for (var i = 0; i < count; i++)
            {
                list.Add(value);
            }
        }

        public static void RotateLeft<T>(this LinkedList<T> list)
        {
            var temp = list.First.Value;
            list.RemoveFirst();
            list.AddLast(temp);
        }

        public static void RotateRight<T>(this LinkedList<T> list)
        {
            var temp = list.Last.Value;
            list.RemoveLast();
            list.AddFirst(temp);
        }

        public static void RotateLeft<T>(this LinkedList<T> list, int n)
        {
            if (n >= 0)
            {
                for (var i = 0; i < n; i++)
                {
                    list.RotateLeft();
                }
            }
            else
            {
                for (var i = 0; i < Math.Abs(n); i++)
                {
                    list.RotateRight();
                }
            }
        }
    }

    public static class PointExtensions
    {
        public static IEnumerable<Point> GetNeighbors(this Point point)
        {
            return point.GetNeighbors(true);
        }

        public static IEnumerable<Point> GetNeighbors(this Point point, bool includeDiagonals)
        {
            var adjacentPoints = new List<Point>(8);

            adjacentPoints.Add(new Point(point.X - 1, point.Y));
            adjacentPoints.Add(new Point(point.X + 1, point.Y));
            adjacentPoints.Add(new Point(point.X, point.Y + 1));
            adjacentPoints.Add(new Point(point.X, point.Y - 1));

            if (includeDiagonals)
            {
                adjacentPoints.Add(new Point(point.X - 1, point.Y - 1));
                adjacentPoints.Add(new Point(point.X + 1, point.Y - 1));
                adjacentPoints.Add(new Point(point.X + 1, point.Y + 1));
                adjacentPoints.Add(new Point(point.X - 1, point.Y + 1));
            }

            return adjacentPoints;
        }

        public static int ManhattanDistance(this Point point)
        {
            return point.ManhattanDistance(new Point(0, 0));
        }

        public static int ManhattanDistance(this Point point, Point target)
        {
            return Math.Abs(point.X - target.X) + Math.Abs(point.Y - target.Y);
        }

        public static Point MoveDown(this Point point, int distance)
        {
            return new Point(point.X, point.Y - distance);
        }

        public static Point MoveUp(this Point point, int distance)
        {
            return new Point(point.X, point.Y + distance);
        }

        public static Point MoveRight(this Point point, int distance)
        {
            return new Point(point.X + distance, point.Y);
        }

        public static Point MoveLeft(this Point point, int distance)
        {
            return new Point(point.X - distance, point.Y);
        }

        public static Point MoveDown(this Point point)
        {
            return point.MoveDown(1);
        }

        public static Point MoveUp(this Point point)
        {
            return point.MoveUp(1);
        }

        public static Point MoveRight(this Point point)
        {
            return point.MoveRight(1);
        }

        public static Point MoveLeft(this Point point)
        {
            return point.MoveLeft(1);
        }

        public static Point Move(this Point point, Direction direction, int distance)
        {
            return direction switch
            {
                Direction.Down => point.MoveDown(distance),
                Direction.Up => point.MoveUp(distance),
                Direction.Right => point.MoveRight(distance),
                Direction.Left => point.MoveLeft(distance),
                _ => throw new ArgumentException(),
            };
        }

        public static Point Move(this Point point, Direction direction)
        {
            return point.Move(direction, 1);
        }

        public static double CalcDistance(this Point p, Point to) => Math.Sqrt(Math.Pow(p.X - to.X, 2) + Math.Pow(p.Y - to.Y, 2));

        public static double CalcSlope(this Point p, Point to) => (double)(p.Y - to.Y) / (double)(p.X - to.X);
    }

    public static class RectangleExtensions
    {
        public static IEnumerable<Point> GetPoints(this Rectangle rect)
        {
            for (var x = rect.Left; x < rect.Left + rect.Width; x++)
            {
                for (var y = rect.Top; y < rect.Top + rect.Height; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }

    public static class CharGridExtensions
    {
        public static IEnumerable<Point> GetPoints(this char[,] grid)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public static char[,] CreateCharGrid(this string input)
        {
            var lines = input.Lines().ToList();
            var result = new char[lines[0].Length, lines.Count];

            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[0].Length; x++)
                {
                    result[x, y] = lines[y][x];
                }
            }

            return result;
        }

        public static string GetString(this char[,] grid)
        {
            var sb = new StringBuilder(grid.GetLength(0) * grid.GetLength(1) + (Environment.NewLine.Length * grid.GetLength(1)));

            for (var y = 0; y <= grid.GetUpperBound(1); y++)
            {
                for (var x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    sb.Append(grid[x, y]);
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static IEnumerable<Point> GetPoints(this char[,] grid, char match)
        {
            return GetPoints(grid).Where(p => grid[p.X, p.Y] == match);
        }

        public static IEnumerable<Point> GetPoints(this char[,] grid, Func<char, bool> match)
        {
            return GetPoints(grid).Where(p => match(grid[p.X, p.Y]));
        }

        public static IEnumerable<Point> GetPoints(this char[,] grid, Func<Point, bool> match)
        {
            return GetPoints(grid).Where(match);
        }

        public static void Replace(this char[,] grid, char match, char replace)
        {
            foreach (var p in grid.GetPoints(match))
            {
                grid[p.X, p.Y] = replace;
            }
        }

        public static int Count(this char[,] grid, char match)
        {
            var result = 0;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == match)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public static char[,] Clone(this char[,] grid, Func<int, int, char, char> transform)
        {
            var result = new char[grid.GetLength(0), grid.GetLength(1)];

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    result[x, y] = transform(x, y, grid[x, y]);
                }
            }

            return result;
        }

        public static char[,] Clone(this char[,] grid, Func<char, char> transform)
        {
            var result = new char[grid.GetLength(0), grid.GetLength(1)];

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    result[x, y] = transform(grid[x, y]);
                }
            }

            return result;
        }

        public static char[,] Clone(this char[,] grid)
        {
            return grid.Clone(c => c);
        }

        public static IEnumerable<char> GetNeighbors(this char[,] map, int x, int y, bool includeDiagonals)
        {
            var neighbors = new Point(x, y).GetNeighbors(includeDiagonals);

            foreach (var n in neighbors)
            {
                if (n.X >= 0 && n.X <= map.GetUpperBound(0) && n.Y >= 0 && n.Y <= map.GetUpperBound(1))
                {
                    yield return map[n.X, n.Y];
                }
            }
        }

        public static IEnumerable<char> GetNeighbors(this char[,] map, int x, int y)
        {
            return map.GetNeighbors(x, y, true);
        }

        public static IEnumerable<(Point point, char c)> GetNeighborPoints(this char[,] map, int x, int y)
        {
            return map.GetNeighborPoints(new Point(x, y));
        }

        public static IEnumerable<(Point point, char c)> GetNeighborPoints(this char[,] map, Point p)
        {
            var neighbors = p.GetNeighbors(false);

            foreach (var n in neighbors)
            {
                if (n.X >= 0 && n.X <= map.GetUpperBound(0) && n.Y >= 0 && n.Y <= map.GetUpperBound(1))
                {
                    yield return (n, map[n.X, n.Y]);
                }
            }
        }

        public static Dictionary<Point, int> FindShortestPaths(this char[,] grid, Func<char, bool> validMove, Point start)
        {
            var steps = 0;
            var result = new Dictionary<Point, int>();

            result.Add(start, 0);

            var reachable = start.GetNeighbors(false).Where(p => validMove(grid[p.X, p.Y]) && !result.ContainsKey(p)).ToList();

            while (reachable.Any())
            {
                steps++;

                reachable.ForEach(r => result.Add(r, steps));

                var newReachable = new List<Point>();
                reachable.ForEach(p => newReachable.AddRange(p.GetNeighbors(false).ToList()));
                reachable = newReachable.Where(p => validMove(grid[p.X, p.Y]) && !result.ContainsKey(p)).Distinct().ToList();
            }

            return result;
        }

        public static int FindShortestPath(this char[,] grid, Func<char, bool> validMove, Point start, Point end)
        {
            var seen = new HashSet<Point>();
            var steps = 0;

            if (start == end)
            {
                return 0;
            }

            var reachable = start.GetNeighbors(false).Where(p => validMove(grid[p.X, p.Y]) && !seen.Contains(p)).ToList();

            while (reachable.Any())
            {
                steps++;

                if (reachable.Any(p => p == end))
                {
                    return steps;
                }

                reachable.ForEach(r => seen.Add(r));

                var newReachable = new List<Point>();
                reachable.ForEach(p => newReachable.AddRange(p.GetNeighbors(false).ToList()));
                reachable = newReachable.Where(p => validMove(grid[p.X, p.Y]) && !seen.Contains(p)).Distinct().ToList();
            }

            return -1;
        }

        public static bool IsValidPoint(this char[,] grid, Point point)
        {
            if (point.X >= 0 && point.X < grid.GetLength(0))
            {
                if (point.Y >= 0 && point.Y < grid.GetLength(1))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetRow(this char[,] grid, int row)
        {
            var sb = new StringBuilder();

            for (var x = 0; x <= grid.GetUpperBound(0); x++)
            {
                sb.Append(grid[x, row]);
            }

            return sb.ToString();
        }

        public static IEnumerable<string> GetRows(this char[,] grid)
        {
            for (var y = 0; y <= grid.GetUpperBound(1); y++)
            {
                yield return grid.GetRow(y);
            }
        }
    }

    public static class NumericExtensions
    {
        public static bool IsPrime(this int number)
        {
            var sqrt = Math.Floor(Math.Sqrt((double)number));

            for (var x = 2; x <= sqrt; x++)
            {
                if (number % x == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Point3D : IEquatable<Point3D>
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }

        public Point3D(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D()
        { }

        public Point3D(string coordinates) :
            this(long.Parse(coordinates.Words().ToList()[0]),
                 long.Parse(coordinates.Words().ToList()[1]),
                 long.Parse(coordinates.Words().ToList()[2]))
        {
        }

        public long GetManhattanDistance()
        {
            return Math.Abs(X - 0) + Math.Abs(Y - 0) + Math.Abs(Z - 0);
        }

        public long GetManhattanDistance(Point3D point)
        {
            return Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z);
        }

        public static bool operator ==(Point3D a, Point3D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Point3D a, Point3D b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point3D);
        }

        public bool Equals(Point3D other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public IEnumerable<Point3D> GetNeighbors()
        {
            var adjacentPoints = new List<Point3D>(6);

            adjacentPoints.Add(new Point3D(X - 1, Y, Z));
            adjacentPoints.Add(new Point3D(X + 1, Y, Z));
            adjacentPoints.Add(new Point3D(X, Y + 1, Z));
            adjacentPoints.Add(new Point3D(X, Y - 1, Z));
            adjacentPoints.Add(new Point3D(X, Y, Z + 1));
            adjacentPoints.Add(new Point3D(X, Y, Z - 1));

            return adjacentPoints;
        }
    }

    public class Point4D : IEquatable<Point4D>
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }
        public long T { get; set; }

        public Point4D(long x, long y, long z, long t)
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }

        public Point4D(string coordinates) :
            this(long.Parse(coordinates.Words().ToList()[0]),
                 long.Parse(coordinates.Words().ToList()[1]),
                 long.Parse(coordinates.Words().ToList()[2]),
                 long.Parse(coordinates.Words().ToList()[3]))
        {
        }

        public long GetManhattanDistance()
        {
            return Math.Abs(X - 0) + Math.Abs(Y - 0) + Math.Abs(Z - 0) + Math.Abs(T - 0);
        }

        public long GetManhattanDistance(Point4D point)
        {
            return Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z) + Math.Abs(T - point.T);
        }

        public static bool operator ==(Point4D a, Point4D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.T == b.T;
        }

        public static bool operator !=(Point4D a, Point4D b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.T != b.T;
        }

        public static Point4D operator +(Point4D a, Point4D b)
        {
            return new Point4D(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.T + b.T);
        }

        public static Point4D operator -(Point4D a, Point4D b)
        {
            return new Point4D(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.T - b.T);
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z},{T}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point4D);
        }

        public bool Equals(Point4D other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 486187739 + X.GetHashCode();
            hashCode = hashCode * 486187739 + Y.GetHashCode();
            hashCode = hashCode * 486187739 + Z.GetHashCode();
            hashCode = hashCode * 486187739 * T.GetHashCode();

            return hashCode;
        }
    }

    public class Tree<T> : IEnumerable<Tree<T>>
    {
        public Tree<T> Parent { get; set; }
        public LinkedList<Tree<T>> Children { get; set; } = new LinkedList<Tree<T>>();
        public T Data { get; set; }

        public Tree(T data)
        {
            Data = data;
        }

        public int CalcDistance(Tree<T> target)
        {
            var distance = 0;

            var visited = new HashSet<Tree<T>>();
            var reachable = new List<Tree<T>>() { this };

            while (!reachable.Any(x => x == target))
            {
                var newReachable = new List<Tree<T>>();

                foreach (var t in reachable.Except(visited))
                {
                    if (t.Parent != null)
                    {
                        newReachable.Add(t.Parent);
                    }

                    newReachable.AddRange(t.Children);
                }

                visited.AddRange(reachable);
                reachable = newReachable;
                distance++;
            }

            return distance;
        }

        public IEnumerator<Tree<T>> GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
        }

        private IEnumerable<Tree<T>> AsEnumerable()
        {
            yield return this;

            foreach (var child in Children)
            {
                foreach (var t in child.AsEnumerable())
                {
                    yield return t;
                }
            }
        }
    }

    public static class TreeExtensions
    {
        public static IEnumerable<Tree<T>> GetAllChildren<T>(this IEnumerable<Tree<T>> trees)
        {
            foreach (var tree in trees)
            {
                foreach (var child in tree.Children)
                {
                    yield return child;
                }
            }
        }
    }

    public static class QueueExtensions
    {
        public static void Enqueue<T>(this Queue<T> q, IEnumerable<T> items)
        {
            items.ForEach(i => q.Enqueue(i));
        }
    }
}