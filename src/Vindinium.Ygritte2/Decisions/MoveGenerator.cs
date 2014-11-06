using System.Collections.Generic;

namespace Vindinium.Ygritte2.Decisions
{
	public class MoveGenerator
	{
		public IEnumerable<MoveDirection> Generate(Map map, State state)
		{
			var source = map[state.GetActiveHero()];
			foreach (var dir in source.Directions)
			{
				if (dir == MoveDirection.x) { yield return dir; }
				var target = source[dir];

				if (target.GetOccupied(state) == PlayerType.None)
				{
					yield return dir;
				}
			}
		}
	}
}
