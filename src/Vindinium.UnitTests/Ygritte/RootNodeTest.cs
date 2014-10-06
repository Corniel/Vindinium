using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.Ygritte.Decisions;

namespace Vindinium.UnitTests.Ygritte
{
	[TestFixture]
	public class RootNodeTest
	{
		[Test]
		public void GetMove()
		{
			var map = MapTest.Map18;
			var state = State.Create(1,
				Hero.Create(90, map[2, 3], 0, 0),
				Hero.Create(90, map[5, 6], 0, 0),
				Hero.Create(90, map[9, 9], 0, 0),
				Hero.Create(90, map[1, 9], 0, 0),
				MineOwnership.Create(map));

			var root = new RootNode(state);

			var act = root.GetMove(map, TimeSpan.FromMilliseconds(1900));
		}
	}
}
