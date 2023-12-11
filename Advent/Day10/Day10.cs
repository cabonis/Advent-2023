using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.Day10
{
	internal class Day10 : IDay
	{
		public string DoWork(string[] input)
		{
			Map map = new Map(input);

			// Part 1
			//int hops = map.Traverse().Count - 1;
			//return Math.Ceiling((decimal)hops / 2).ToString();

			return map.Traverse2().ToString();
		}


		public class Map
		{
			private static HashSet<NodeType> LeftNodes = new HashSet<NodeType> { NodeType.EastWest, NodeType.NorthWest, NodeType.SouthWest, NodeType.Start };
			private static HashSet<NodeType> RightNodes = new HashSet<NodeType> { NodeType.EastWest, NodeType.NorthEast, NodeType.SouthEast, NodeType.Start };
			private static HashSet<NodeType> UpNodes = new HashSet<NodeType> { NodeType.NorthSouth, NodeType.NorthEast, NodeType.NorthWest, NodeType.Start };
			private static HashSet<NodeType> DownNodes = new HashSet<NodeType> { NodeType.NorthSouth, NodeType.SouthEast, NodeType.SouthWest, NodeType.Start };

			private readonly NodeType[,] _nodes;
			private readonly Node _start;

			private List<Node> GetAdjacent(Node current)
			{
				List<Node> adjacent = new List<Node>();

				void TryAddNode(int x, int y, HashSet<NodeType> nodeTypes)
				{
					Node n = new Node(x, y, _nodes[x, y]);
					if (nodeTypes.Contains(n.NodeType))
						adjacent.Add(n);
				}

				// Left
				if (LeftNodes.Contains(current.NodeType) && current.X > 0)
					TryAddNode(current.X - 1, current.Y, RightNodes);

				// Right
				if (RightNodes.Contains(current.NodeType) && current.X < _nodes.GetLength(0) - 1)
					TryAddNode(current.X + 1, current.Y, LeftNodes);

				// Up
				if (UpNodes.Contains(current.NodeType) && current.Y > 0)
					TryAddNode(current.X, current.Y - 1, DownNodes);

				// Down
				if (DownNodes.Contains(current.NodeType) && current.Y < _nodes.GetLength(1) - 1)
					TryAddNode(current.X, current.Y + 1, UpNodes);

				return adjacent;
			}

			private void UpdateStartPipe()
			{
				List<Node> adjacent = GetAdjacent(_start);

				if (adjacent.Count != 2)
					throw new Exception();

				foreach (NodeType nodeType in Enum.GetValues(typeof(NodeType)))
				{
					if (nodeType == NodeType.Start || nodeType == NodeType.Ground)
						continue;

					_nodes[_start.X, _start.Y] = nodeType;

					if (GetAdjacent(adjacent[0]).Contains(_start) && GetAdjacent(adjacent[1]).Contains(_start))
					{
						return;
					}
				}

				throw new Exception();
			}
		

			public Map(string[] input)
			{
				_nodes = new NodeType[input.Length, input[0].Length];

				for (int i = 0; i < input.Length; i++)
				{
					for (int j = 0; j < input[i].Length; j++)
					{
						NodeType nodeType = (NodeType)input[i][j];
						_nodes[j, i] = nodeType;

						if (nodeType == NodeType.Start)
						{
							_start = new Node(j, i, NodeType.Start);
						}
					}
				}
			}

			public List<Node> Traverse()
			{
				HashSet<Node> visited = new HashSet<Node>();
				Node current = _start;

				while (true)
				{
					visited.Add(current);
					List<Node> adjacent = GetAdjacent(current);
					current = adjacent.Where(a => !visited.Contains(a)).FirstOrDefault();

					if (current == null)
						break;
				}

				return visited.ToList();
			}

			public int Traverse2()
			{
				List<Node> loop = Traverse();

				UpdateStartPipe();

				char GetNode(int x, int y)
				{
					Node node = new Node(x, y, _nodes[x, y]);
					return loop.Contains(node) ? (char)node.NodeType : (char)NodeType.Ground;
				}

				int inside = 0;

				Regex regex = new Regex("\\||F-*J|L-*7", RegexOptions.Compiled);

				for (int i = 0; i < _nodes.GetLength(1); i++)
				{
					var row = string.Join("", Enumerable.Range(0, _nodes.GetLength(0)).Select(x => GetNode(x, i)).ToArray());

					string[] sections = regex.Split(row);
					for (int j = 0; j < sections.Length; j++)
					{
						if (j % 2 == 0)
							continue;

						inside += sections[j].Count(c => c == '.');
					}
				}

				return inside;
			}

		}

		public class Node
		{
			public int X { get; }
			public int Y { get; }
			public NodeType NodeType { get; }

			public Node(int x, int y, NodeType nodeType)
			{
				X = x;
				Y = y;
				NodeType = nodeType;
			}

			public override int GetHashCode()
			{
				return X.GetHashCode() + Y.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				Node other = obj as Node;
				return other != null && other.X == X && other.Y == Y;
			}
		}


		public enum NodeType
		{
			NorthSouth = '|',
			EastWest = '-',
			NorthEast = 'L',
			NorthWest = 'J',
			SouthWest = '7',
			SouthEast = 'F',
			Ground = '.',
			Start = 'S'
		}
	}
}
