using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent;

namespace Advent.Day4
{
	internal class Day04 : IDay
	{
		public string DoWork(string[] input)
		{
			List<Card> cards = new List<Card>();
			input.ToList().ForEach(l => cards.Add(new Card(l)));

			// Part 1
			//int score = cards.Select(c => c.GetWinnerScore()).Sum();

			return cards.Select(c => c.Play(cards)).Sum().ToString();
		}
	}

	public class Card
	{
		private readonly int _id;
		private HashSet<int> winningNumbers = new HashSet<int>();
		private List<int> numbers = new List<int>();

		private int NumberOfWinners()
		{
			int numWinners = 0;

			foreach (int number in numbers)
			{
				if (winningNumbers.Contains(number))
				{
					numWinners++;
				}
			}

			return numWinners;
		}

		public Card(string input)
		{
			var cardParts = input.Split(':');
			_id = int.Parse(cardParts[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]);

			var allNumParts = cardParts[1].Split('|');

			string[] numSplit = new string[] { " " };
			var winningParts = allNumParts[0].Trim().Split(numSplit, StringSplitOptions.RemoveEmptyEntries).ToList();
			winningParts.ForEach(p => winningNumbers.Add(int.Parse(p)));

			var numParts = allNumParts[1].Trim().Split(numSplit, StringSplitOptions.RemoveEmptyEntries).ToList();
			numParts.ForEach(p => numbers.Add(int.Parse(p)));
		}

		public int GetWinnerScore()
		{
			int numWinners = NumberOfWinners();

			if (numWinners < 2)
				return numWinners;

			return (int)Math.Pow(2, (double)numWinners - 1);
		}

		public int Play(List<Card> allCards)
		{
			int numWinners = NumberOfWinners();

			List<Card> newCards = new List<Card>();
			for (int i = 1; i < numWinners + 1; i++)
			{
				int newCardIndex = _id - 1 + i;

				if (newCardIndex < allCards.Count)
				{
					newCards.Add(allCards[newCardIndex]);
				}
			}

			return 1 + newCards.Select(c => c.Play(allCards)).Sum();
		}
	}
}
