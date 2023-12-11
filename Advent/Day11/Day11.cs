using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Day11
{
	internal class Day11 : IDay
	{
		public string DoWork(string[] input)
		{
			var space = new Space(input);
			

			return space.Traverse().ToString();
		}
	}

	public class Space
	{
		private readonly List<List<char>> _matrix;
		private readonly List<int> _emptyRows = new List<int>();
		private readonly List<int> _emptyColumns = new List<int>();

		private List<List<char>> Transpose(List<List<char>> matrix)
		{
			return matrix.SelectMany(inner => inner.Select((item, index) => new { item, index }))
				.GroupBy(i => i.index, i => i.item)
				.Select(g => g.ToList())
				.ToList();
		}

		private List<int> GetEmptyRows(List<List<char>> matrix)
		{
			List<int> empties = new List<int>();
			foreach (var (row, index) in matrix.WithIndex())
			{
				if (row.All(x => x == '.'))
					empties.Add(index);
			}
			return empties;
		}

		public Space(string[] input)
		{
			var matrix = Enumerable.Range(0, input.Length)
				.Select(row => Enumerable.Range(0, input[0].Length)
					.Select(column => input[row][column])
					.ToList())
				.ToList();

			_matrix = matrix;
			_emptyRows = GetEmptyRows(matrix);
			_emptyColumns = GetEmptyRows(Transpose(matrix));
		}

		public double Traverse()
		{
			List<Galaxy> galaxies = new List<Galaxy>();

			foreach (var (row, x) in _matrix.WithIndex())
				foreach (var (col, y) in row.WithIndex())
				{
					if (col != '.')
					{
						galaxies.Add(new Galaxy(x, y));
					}
				}

			var combinations = galaxies.SelectMany((x, i) => galaxies.Skip(i + 1), (x, y) => Tuple.Create(x, y));
			return combinations.Select(c => c.Item1.GetDistance(c.Item2, _emptyRows, _emptyColumns)).Sum();
		}

	}

	public class Galaxy
	{
		private const int ExpansionFactor = 1000000;

		public int X { get; }
		public int Y { get; }

		public Galaxy(int x, int y)
		{
			X = x;
			Y = y;
		}

		public double GetDistance(Galaxy galaxy, List<int> emptyRows, List<int> emptyColumns)
		{
			int numRows = emptyRows.Where(r => r > Math.Min(galaxy.X, X) && r < Math.Max(galaxy.X, X)).Count();
			int numColumns = emptyColumns.Where(c => c > Math.Min(galaxy.Y, Y) && c < Math.Max(galaxy.Y, Y)).Count();

			int multiplier = ExpansionFactor - 1;
			double deltaX = Math.Abs(galaxy.X - X) + (numRows * multiplier);
			double deltaY = Math.Abs(galaxy.Y - Y) + (numColumns * multiplier);

			return deltaX + deltaY;
		}
	}

	public static class IEnumerableHelper
	{
		public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
		{
			return source.Select((item, index) => (item, index));
		}
	}
}
