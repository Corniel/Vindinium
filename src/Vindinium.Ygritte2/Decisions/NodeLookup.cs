﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Vindinium.Ygritte2.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class NodeLookup
	{
		private Dictionary<int, Dictionary<State, Node>> lookup = new Dictionary<int, Dictionary<State, Node>>();
		/// <summary>Gets the depth of the current search.</summary>
		public int Depth { get { return lookup.Max(kvp => kvp.Key) - lookup.Min(kvp => kvp.Key); } }

		/// <summary>Gets the number of nodes.</summary>
		public int Nodes { get { return lookup.Sum(kvp => kvp.Value.Count) + Transpositions; } }

		public int Transpositions { get; protected set; }

		public Node Get(int turn, State state, MoveDirection dir)
		{
			Node node;
			Dictionary<State, Node> dict;

			if (lookup.TryGetValue(turn, out dict))
			{
				if (dict.TryGetValue(state, out node))
				{
					this.Transpositions++;
					return node;
				}
			}
			else
			{
				dict = new Dictionary<State, Node>();
				lookup[turn] = dict;
			}
			node = new Node(state, dir);
			dict[state] = node;
			return node;
		}

		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Node lookup: Depth: {0}, Nodes: {1:#,##0}, Transpositions: {2:#,##0}", this.Depth, this.Nodes, this.Transpositions);
			}
		}

		public string LogNodeCounts()
		{
			var sb = new StringBuilder();

			int depth = 1;

			foreach (var kvp in lookup)
			{
				sb.AppendFormat("[{0,2}] {1:#,##0} ({2})", depth++, kvp.Value.Count, kvp.Key);
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
