using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Day9
{
	internal class Day09 : IDay
	{
		public string DoWork(string[] input)
		{
			List<Sequence> sequences = new List<Sequence>();

			foreach (string line in input)
			{
				sequences.Add(new Sequence(line));
			}

			//Part 1
			//return sequences.Select(x => x.GetNext()).Sum().ToString();

			return sequences.Select(x => x.GetPrevious()).Sum().ToString();
		}

		public class Sequence
		{
			private readonly List<int> _values;

			public Sequence(string input)
			{
				_values = new List<int>(input.Split(' ').Select(x => int.Parse(x.Trim())));
			}

			public Sequence(List<int> values)
			{
				_values = values;
			}

			public int GetNext()
			{
				if(_values.All(x => x == 0))
					return 0;

				Sequence subSequence = new Sequence(_values.Skip(1).Select((x, i) => x - _values[i]).ToList());

				int next = subSequence.GetNext();
				return _values.Last() + next;
			}

			public int GetPrevious()
			{
				if (_values.All(x => x == 0))
					return 0;

				Sequence subSequence = new Sequence(_values.Skip(1).Select((x, i) => x - _values[i]).ToList());

				int previous = subSequence.GetPrevious();
				return _values.First() - previous;
			}
		}
	}
}
