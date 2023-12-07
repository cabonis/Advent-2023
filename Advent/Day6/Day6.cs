using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Day6
{
	internal class Day6 : IDay
	{
		public string DoWork(string[] input)
		{
			string[] splitter = new string[] { " " };

			// Part 1
			//List<double> times = input[0].Split(splitter, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => double.Parse(x.Trim())).ToList();
			//List<double> distances = input[1].Split(splitter, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => double.Parse(x.Trim())).ToList();
			//List<Race> races = new List<Race>();
			//for (int i = 0; i < times.Count; i++)
			//{
			//	races.Add(new Race(times[i], distances[i]));
			//}
			//double product = races.Select(x => x.GetWinningButtonDurations).Aggregate((a, b) => a * b);

			StringBuilder timeBuilder = new StringBuilder();
			input[0].Split(splitter, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList().ForEach(x => timeBuilder.Append(x));
			double time = double.Parse(timeBuilder.ToString());

			StringBuilder distanceBuilder = new StringBuilder();
			input[1].Split(splitter, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList().ForEach(x => distanceBuilder.Append(x));
			double distance = double.Parse(distanceBuilder.ToString());

			Race race = new Race(time, distance);
			double numWays = race.GetWinningButtonDurations();
			return numWays.ToString();
		}

		public class Race
		{
			private readonly double _allotedTime;
			private readonly double _recordDistance;

			private double CalculateDistance(double buttonMs)
			{
				return (_allotedTime - buttonMs) * buttonMs;
			}

			public Race(double allotedTime, double recordDistance)
			{
				_allotedTime = allotedTime;
				_recordDistance = recordDistance;
			}

			public double GetWinningButtonDurations()
			{
				return EnumerableHelper.Range(0, _allotedTime).Select(x => CalculateDistance(x)).Where(x => x > _recordDistance).Count();
			}
		}

		public static class EnumerableHelper
		{
			public static IEnumerable<double> Range(double from, double to, double step = 1)
			{
				if (step <= 0.0) step = (step == 0.0) ? 1.0 : -step;

				if (from <= to)
				{
					for (double d = from; d <= to; d += step) yield return d;
				}
				else
				{
					for (double d = from; d >= to; d -= step) yield return d;
				}
			}
		}
	}
}
