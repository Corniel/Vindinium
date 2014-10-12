using System.Diagnostics;

namespace Vindinium.Ygritte.Decisions
{
	[DebuggerDisplay("Move: crashed")]
	public class MoveCrashed : Move
	{
		public override Tile GetTarget(Tile source, Map map, State state) { return source; }
	}
}
