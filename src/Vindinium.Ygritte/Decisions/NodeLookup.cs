using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	public class NodeLookup
	{
		private Dictionary<int, Dictionary<State, Node>> lookup = new Dictionary<int, Dictionary<State, Node>>();

		public Node Get(int turn, Map map, State state)
		{
			Node node;
			Dictionary<State, Node> dict;

			if (lookup.TryGetValue(turn, out dict))
			{
				if (dict.TryGetValue(state, out node))
				{
					return node;
				}
			}
			else
			{
				dict = new Dictionary<State, Node>();
				lookup[turn] = dict;
			}
			node = ChildNode.Create(map, state);
			dict[state] = node;

			return node;
		}
	}
}
