using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Decisions
{
	public abstract class Node
	{
		private static readonly NodeLookup lookup = new NodeLookup();

		public static Node Get(int turn, State state)
		{
			return lookup.Get(turn, state);
		}

		public State State { get; protected set; }
		public int Turn { get { return this.State.Turn; } }
		public PlayerType PlayerToMove { get { return this.State.PlayerToMove; } }

		public abstract void GeneratePlans(Map map);

		public abstract int GetScore(PlayerType player);
	}
}
