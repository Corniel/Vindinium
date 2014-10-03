using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	public abstract class Node
	{
		private static readonly NodeLookup lookup = new NodeLookup();

		public static Node Get(int turn, Map map, State state)
		{
			return lookup.Get(turn, map, state);
		}

		public abstract void Process(int turn);

		public int Score { get; protected set; }
		public int Depth { get; protected set; }

		public State State { get; protected set; }
		public int Turn { get { return this.State.Turn; } }
		public PlayerType PlayerToMove { get { return this.State.PlayerToMove; } }

		public abstract void GeneratePlans(Map map);
	}
}
