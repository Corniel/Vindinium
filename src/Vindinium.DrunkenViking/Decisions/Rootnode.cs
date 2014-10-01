﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Decisions
{
	public class RootNode : Node
	{
		private Dictionary<MoveDirection, Node> states = new Dictionary<MoveDirection, Node>();

		public override void GeneratePlans(Map map) { }

		public override int GetScore(PlayerType player)
		{
			throw new NotImplementedException();
		}

		public static RootNode Create(Map map, State state, Hero hero, PlayerType player, Tile source)
		{
			var root = new RootNode()
			{
				State = state,
			};

			foreach (var dir in source.Directions)
			{
				var target = source[dir];
				var nw_sta = state.Move(map, hero, player, source, target);
				root.states[dir] = Node.Get(state.Turn + 1, nw_sta);
			}
			return root;
		}
	}
}
