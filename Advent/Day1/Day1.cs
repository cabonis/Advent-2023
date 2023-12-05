using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
	internal class Day1 : IDay
	{
		public int DoWork(string[] input)
		{
			int sum = 0;

			foreach (string line in input)
			{
				int num = GetNumber2(line);
				sum += num;
			}

			return sum;
		}

		private static int GetNumber(string line)
		{
			List<int> ints = new List<int>();
			foreach (char c in line)
			{
				if (int.TryParse(c.ToString(), out int num))
				{
					ints.Add(num);
				}
			}

			if (ints.Count == 0)
				throw new Exception("Oh crap!");

			string numString = $"{ints[0]}{ints[ints.Count - 1]}";
			return int.Parse(numString);
		}

		private static int GetNumber2(string line)
		{
			const string pattern = @"one|two|three|four|five|six|seven|eight|nine|zero|\d";
			Regex regex = new Regex(pattern, RegexOptions.Compiled);

			List<int> ints = new List<int>();
			foreach (var match in regex.Matches(line).Cast<Match>())
			{
				ints.Add(match.Value switch
				{
					"one" => 1,
					"two" => 2,
					"three" => 3,
					"four" => 4,
					"five" => 5,
					"six" => 6,
					"seven" => 7,
					"eight" => 8,
					"nine" => 9,
					"zero" => 0,
					_ => int.Parse(match.Value)
				});
			}

			if (ints.Count == 0)
				throw new Exception("Oh crap!");

			string numString = $"{ints[0]}{ints[ints.Count - 1]}";
			return int.Parse(numString);
		}
	}
}
