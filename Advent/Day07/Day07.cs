using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Day7
{
	internal class Day07 : IDay
	{
		public string DoWork(string[] input)
		{
			List<Hand> hands = new List<Hand>();

			foreach (string line in input)
			{
				var parts = line.Split(' ');
				hands.Add(new Hand(parts[0].ToCharArray(), int.Parse(parts[1].Trim())));
			}

			hands.Sort();

			int totalValue = hands.Select((h, i) => h.Bet * (i + 1)).Sum();
			return totalValue.ToString();
		}

		public class Hand : IComparable<Hand>
		{
			private readonly char[] _cards;
			private readonly int _bet;
			private readonly HandValue _handValue;

			private int CardValue(char card)
			{
				return card switch
				{
					'A' => 14,
					'K' => 13,
					'Q' => 12,
					//'J' => 11,
					'J' => 1,
					'T' => 10,
					_ => int.Parse(card.ToString())
				};
			}

			public Hand(char[] cards, int bet)
			{
				_cards = cards;
				_bet = bet;

				int jokers = 0;
				Dictionary<int, int> distribution = new Dictionary<int, int>();
				foreach (char card in cards)
				{
					int value = CardValue(card);

					if (value == 1)
					{
						jokers++;
					}

					if (distribution.ContainsKey(value))
						distribution[value]++;
					else
						distribution.Add(value, 1);
				}

				var noJokers = distribution.Where(kvp => kvp.Key != 1);
				if (noJokers.Any())
				{
					var bestSet = noJokers.Where(kvp => kvp.Value == noJokers.Max(kvp => kvp.Value)).First().Key;
					distribution[bestSet] += jokers;
					distribution.Remove(1);

				}

				int maxCommon = distribution.Max(kvp => kvp.Value);

				switch (distribution.Count)
				{
					case 1:
						// Five of a kind
						_handValue = HandValue.FiveOfAKind;
						break;
					case 2:
						// Full house or four of a kind
						_handValue = maxCommon == 4 ? HandValue.FourOfAKind : HandValue.FullHouse;
						break;
					case 3:
						// Two pair or three of a kind
						_handValue = maxCommon == 3 ? HandValue.ThreeOfAKind : HandValue.TwoPair;
						break;
					case 4:
						// Pair
						_handValue = HandValue.Pair;
						break;
					case 5:
						// High card
						_handValue = HandValue.HighCard;
						break;
				}
			}

			public int GetCardValue(int index)
			{
				return CardValue(_cards[index]);
			}

			public int Bet => _bet;

			public override string ToString()
			{
				return _cards.ToString();
			}

			int IComparable<Hand>.CompareTo(Hand other)
			{
				int compared = _handValue.CompareTo(other._handValue);

				if (compared != 0)
					return compared;

				for (int i = 0; i < _cards.Length; i++)
				{
					compared = GetCardValue(i).CompareTo(other.GetCardValue(i));
					if (compared != 0)
						return compared;
				}

				return 0;
			}

		}

		public enum HandValue
		{
			HighCard,
			Pair,
			TwoPair,
			ThreeOfAKind,
			FullHouse,
			FourOfAKind,
			FiveOfAKind
		}
	}
}
