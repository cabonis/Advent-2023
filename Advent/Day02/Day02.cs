using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent;

namespace Advent.Day2
{
	internal class Day02 : IDay
	{
		public string DoWork(string[] input)
		{
			List<Game> games = new List<Game>();
			foreach (string line in input)
			{
				games.Add(new Game(line));
			}

			// Part 1
			//CubeSelection actual = new CubeSelection { NumRed = 12, NumGreen = 13, NumBlue = 14 };
			//List<Game> validGames = games.Where(g => g.IsGameValid(actual)).ToList();
			//int validSum = validGames.Select(g => g.Id).Sum();
			//Console.WriteLine(validSum);

			int sumPowers = games.Select(g => g.PowMinSet()).Sum();
			return sumPowers.ToString();
		}
	}

	public class Game
	{
		public int Id { get; }

		public List<CubeSelection> Rounds = new List<CubeSelection>();

		public Game(string gameLine)
		{
			var parts = gameLine.Split(':');

			var firstPart = parts[0].Split(' ');
			Id = int.Parse(firstPart[1]);

			var rounds = parts[1].Trim().Split(';');

			foreach (var round in rounds)
			{
				CubeSelection gameRound = new CubeSelection();

				var roundParts = round.Trim().Split(',');

				foreach (var part in roundParts)
				{
					var partSplit = part.Trim().Split(' ');
					var number = int.Parse(partSplit[0]);
					var color = partSplit[1];

					switch (color)
					{
						case "red":
							gameRound.NumRed = number;
							break;
						case "green":
							gameRound.NumGreen = number;
							break;
						case "blue":
							gameRound.NumBlue = number;
							break;
						default:
							throw new Exception("Oh crap!");
					}
				}

				Rounds.Add(gameRound);
			}
		}

		public bool IsGameValid(CubeSelection selection)
		{
			foreach (var round in Rounds)
			{
				if (round.NumRed > selection.NumRed || round.NumGreen > selection.NumGreen || round.NumBlue > selection.NumBlue)
				{
					return false;
				}
			}

			return true;
		}

		public int PowMinSet()
		{
			int maxRed = Rounds.Select(round => round.NumRed).Max();
			int maxGreen = Rounds.Select(round => round.NumGreen).Max();
			int maxBlue = Rounds.Select(round => round.NumBlue).Max();

			return maxRed * maxGreen * maxBlue;
		}

	}

	public class CubeSelection
	{
		public int NumRed { get; set; } = 0;
		public int NumGreen { get; set; } = 0;
		public int NumBlue { get; set; } = 0;
	}
}
