using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
	internal class Day3 : IDay
	{
		public int DoWork(string[] input)
		{
			Schematic schematic = new Schematic(input);

			// Part 1
			//List<Part> validParts = schematic.GetValidParts();
			//int sum = validParts.Select(x => x.Number).Sum();

			int sum = schematic.GetGearsTotal();
			
			return sum;
		}

		public class Schematic
		{
			private string[] _data;
			private Regex _regex;

			private List<Part> GetParts()
			{
				List<Part> parts = new List<Part>();

				for (int i = 0; i < _data.Length; i++)
				{
					foreach (var match in _regex.Matches(_data[i]).Cast<Match>())
					{
						Part partNum = new Part { Number = int.Parse(match.Value) };
						for (int l = 0; l < match.Length; l++)
						{
							partNum.Locations.Add(new Location { Line = i, Index = match.Index + l });
						}

						parts.Add(partNum);
					}
				}

				return parts;
			}

			private List<Location> GetAdjacent(Part part)
			{
				HashSet<Location> partLocations = new HashSet<Location>(part.Locations);
				HashSet<Location> adjacentLocations = new HashSet<Location>();

				foreach (Location partLocation in part.Locations)
				{
					for (int newLine = partLocation.Line - 1; newLine < partLocation.Line + 2; newLine++)
					{
						// Out of bounds
						if (newLine < 0 || newLine == _data.Length)
							continue;

						for (int newIndex = partLocation.Index - 1; newIndex < partLocation.Index + 2; newIndex++)
						{
							// Out of bounds
							if (newIndex < 0 || newIndex == _data[partLocation.Line].Length)
								continue;

							// Same part
							if (newLine == partLocation.Line && newIndex == partLocation.Index)
								continue;

							Location adjacent = new Location { Line = newLine, Index = newIndex };

							if (!partLocations.Contains(adjacent))
							{
								adjacentLocations.Add(adjacent);
							}
						}
					}
				}

				return adjacentLocations.ToList();
			}

			public List<Part> GetValidParts()
			{
				List<Part> validParts = new List<Part>();

				foreach (Part part in GetParts())
				{
					foreach (Location adjacent in GetAdjacent(part))
					{
						var character = _data[adjacent.Line][adjacent.Index];

						if (!int.TryParse(character.ToString(), out _) && !char.Equals(character, '.'))
						{
							validParts.Add(part);
							break;
						}
					}
				}

				return validParts;
			}

			public int GetGearsTotal()
			{
				Dictionary<Location, List<Part>> potentialGears = new Dictionary<Location, List<Part>>();

				foreach (Part part in GetParts())
				{
					foreach (Location adjacent in GetAdjacent(part))
					{
						var character = _data[adjacent.Line][adjacent.Index];

						if (char.Equals(character, '*'))
						{
							if (!potentialGears.TryGetValue(adjacent, out List<Part> gearParts))
							{
								potentialGears[adjacent] = new List<Part>();
							}
							potentialGears[adjacent].Add(part);
						}

					}
				}

				var gearTotal = potentialGears.Where(pair => pair.Value.Count == 2)
					.Select(pair => pair.Value[0].Number * pair.Value[1].Number)
					.Sum();

				return gearTotal;
			}


			public Schematic(string[] data)
			{
				_data = data;
				_regex = new Regex(@"\d+", RegexOptions.Compiled);
			}


		}

		public class Part
		{
			public int Number { get; set; }
			public List<Location> Locations { get; } = new List<Location>();

			public override string ToString()
			{
				return Number.ToString();
			}
		}

		public class Location
		{
			public int Line { get; set; }
			public int Index { get; set; }

			public override int GetHashCode()
			{
				return Line.GetHashCode() + Index.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (obj is Location location)
				{
					return location.Line == Line && location.Index == Index;
				}

				return false;
			}
		}
	}
}
