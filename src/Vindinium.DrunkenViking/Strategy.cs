using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking
{
	public abstract class Strategy
	{
		protected Strategy(Map map)
		{
			this.Map = map;
		}
		public Map Map { get; protected set; }

		public virtual void Initializes() { }

		public abstract bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove);

		public abstract MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove);

	}
}
