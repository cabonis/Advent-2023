using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent;

namespace Advent.Day5
{
	internal class Day05 : IDay
	{
		public string DoWork(string[] input)
		{
			var seeds = input[0].Split(':')[1].Trim().Split(' ').Select(s => double.Parse(s)).ToList();

			MapLayer baseLayer = new MapLayer(input, 3);

			//// Part 1
			////var min = seeds.Select(s => baseLayer.Lookup(s)).Min();

			// Part 2
			List<Range> seedRanges = new List<Range>();
			for (int i = 0; i < seeds.Count; i++)
			{
				if (i % 2 != 0)
					continue;

				seedRanges.Add(new Range(seeds[i], seeds[i] + seeds[i + 1] - 1));
			}

			var results = baseLayer.Lookup(seedRanges);
			var min = results.Select(x => x.Start).Min();

			return min.ToString();
		}
	}

	public class MapLayer
	{
		private readonly MapLayer _innerLayer;
		private readonly List<MapEntry> _mapEntries = new List<MapEntry>();

		public MapLayer(string[] input, int lineIndex)
		{
			while (input.Length > lineIndex && !string.IsNullOrEmpty(input[lineIndex]))
			{
				var line = input[lineIndex].Split(' ').Select(x => double.Parse(x)).ToList();
				Range range = new Range(line[1], line[1] + line[2] - 1);
				_mapEntries.Add(new MapEntry(range, line[0]));

				lineIndex++;
			}			

			if (input.Length > lineIndex + 2)
			{
				_innerLayer = new MapLayer(input, lineIndex + 2);
			}
		}

		public double Lookup(double value)
		{
			double localLookup = value;

			foreach (var entry in _mapEntries)
			{
				if (entry.TryGetValue(value, out double found))
				{
					localLookup = found;
					break;
				}
			}

			return _innerLayer?.Lookup(localLookup) ?? localLookup;
		}

		public List<Range> Lookup(List<Range> inputs)
		{
			List<Range> result = new List<Range>();

			foreach (var input in inputs)
			{
				List<Range> overlaps = new List<Range>();

				foreach (var mapEntry in _mapEntries)
				{
					if (mapEntry.TryMapRange(input, out Range overlap, out Range mapping))
					{
						result.Add(mapping);
						overlaps.Add(overlap);
					}	
				}

				overlaps = overlaps.OrderBy(o => o.Start).ToList();

				if (overlaps.Count == 0)
				{
					result.Add(input);
					continue;
				}

				if (input.Start < overlaps[0].Start)
				{
					result.Add(new Range(input.Start, overlaps[0].Start - 1));
				}

				if (input.End > overlaps[overlaps.Count - 1].End)
				{
					result.Add(new Range(overlaps[overlaps.Count - 1].End + 1, input.End));
				}

				for (int i = 0; i < overlaps.Count - 1; i++)
				{
					if (overlaps[i + 1].Start > overlaps[i].End + 1)
					{
						result.Add(new Range(overlaps[i].End + 1, overlaps[i + 1].Start - 1));
					}
				}
			}

			return _innerLayer?.Lookup(result) ?? result;
		}
	}

	public class MapEntry
	{
		private readonly Range _sourceRange;
		private readonly double _targetStartValue;

		public MapEntry(Range sourceRange, double targetStartValue)
		{
			_sourceRange = sourceRange;
			_targetStartValue = targetStartValue;
		}

		public bool TryGetValue(double inputValue, out double value)
		{
			if (inputValue < _sourceRange.Start || inputValue > _sourceRange.End)
			{
				value = 0;
				return false;
			}

			value = _targetStartValue + (inputValue - _sourceRange.Start);
			return true;
		}

		public bool TryMapRange(Range inputRange, out Range overlap, out Range mapping)
		{
			double maxStart = Math.Max(inputRange.Start, _sourceRange.Start);
			double minEnd = Math.Min(inputRange.End, _sourceRange.End);

			if (maxStart < minEnd)
			{
				double delta = Math.Abs(_sourceRange.Start - maxStart);
				double overlapNum = minEnd - maxStart;
				double targetStart = _targetStartValue + delta;
				overlap = new Range(maxStart, maxStart + overlapNum);
				mapping = new Range(targetStart, targetStart + overlapNum);
				return true;
			}

			overlap = null;
			mapping = null;
			return false;
		}
	}

	public class Range
	{
		private readonly double _startValue;
		private readonly double _endValue;

		public Range(double startValue, double endValue)
		{
			_startValue = startValue;
			_endValue = endValue;
		}

		public double Start => _startValue;
		public double End => _endValue;
	}
}
