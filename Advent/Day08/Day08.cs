using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Day8
{
	internal class Day08 : IDay
	{
		public string DoWork(string[] input)
		{
			MapSteps steps = new MapSteps(input[0]);
			Map map = new Map(input.Skip(2));

			// Part 1
			//int moves = map.Traverse(steps);

			double moves = map.TraverseMulti(steps);

			return moves.ToString();
		}

		public class Map
		{
			private const string Start = "AAA";
			private const string End = "ZZZ";

			private readonly List<string> _startingPositions = new List<string>();

			private readonly Dictionary<string, Tuple<string, string>> _map = new();

			private string GetNextPosition(string current, Direction direction)
			{
				if (_map.TryGetValue(current, out var directions))
				{
					return direction == Direction.Left ? directions.Item1 : directions.Item2;
				}

				throw new Exception("Location not found");
			}

			public Map(IEnumerable<string> input)
			{
				foreach (var line in input)
				{
					var location = line.Substring(0, 3);
					var left = line.Substring(7, 3);
					var right = line.Substring(12, 3);
					_map[location] = Tuple.Create(left, right);

					if (location[2] == 'A')
					{
						_startingPositions.Add(location);
					}
				}
			}

			public int Traverse(MapSteps steps)
			{
				string position = Start;
				int stepIndex = 0;

				while (true)
				{
					Direction nextStep = steps.GetStep(stepIndex);
					position = GetNextPosition(position, nextStep);

					if (string.Equals(position, End))
					{
						break;
					}

					stepIndex++;
				}

				return stepIndex + 1;
			}

			public double TraverseMulti(MapSteps steps)
			{
				double gcf(double a, double b)
				{
					while (b != 0)
					{
						double temp = b;
						b = a % b;
						a = temp;
					}
					return a;
				}

				double lcm(List<double> args)
				{
					double lcminternal(double a, double b)
					{
						double gcfVal = gcf(a, b);
						return (a * b) / gcfVal;
					}

					if (args.Count == 2)
					{
						return lcminternal(args[0], args[1]);
					}
					else
					{
						var a = args[0];
						var b = lcm(args.Skip(1).ToList());
						return lcminternal(a, b);
					}
				}

				double result = 0;
				Dictionary<int, double> possibleEndSteps = new Dictionary<int, double>();
				List<string> positions = new List<string>(_startingPositions);

				int stepIndex = 0;
				while(true)
				{
					List<string> nextPositions = new List<string>();

					for (int i = 0; i < positions.Count; i++)
					{
						Direction nextStep = steps.GetStep(stepIndex);
						string nextPosition = GetNextPosition(positions[i], nextStep);
						nextPositions.Add(nextPosition);

						if (nextPosition[2] == 'Z')
						{
							possibleEndSteps[i] = stepIndex + 1;
						}
					}

					if (possibleEndSteps.Count == _startingPositions.Count)
					{
						result = lcm(possibleEndSteps.Values.ToList());
						break;
					}

					positions = nextPositions;
					stepIndex++;
				}

				return result;
			}

		}

		public class MapSteps
		{
			private readonly string _steps;

			private Direction ToStep(char c)
			{
				return c switch
				{
					'R' => Direction.Right,
					'L' => Direction.Left,
					_ => throw new Exception("Unexpected direction"),
				};
			}

			public MapSteps(string input)
			{
				_steps = input;
			}

			public Direction GetStep(int index)
			{
				return ToStep(_steps[index % _steps.Length]);
			}
		}

		public enum Direction
		{
			Left,
			Right
		}
	}
}
