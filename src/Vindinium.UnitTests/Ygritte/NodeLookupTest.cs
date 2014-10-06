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
	public class NodeLookupTest
	{
		[Test]
		public void Get_SameStateTwice_NodesAreEqual1NodeInLookup()
		{
			var map = MapTest.Map06;
			var state = State.Create(map);

			var lookup = new NodeLookup();

			var act0 = lookup.Get(1, map, state);
			var act1 = lookup.Get(1, map, state);

			Assert.AreEqual(act0, act1);
			Assert.AreEqual(1, lookup.Nodes);
		}

		[Test]
		public void Get_TwoStates_NodesAreEqual1NodeInLookup()
		{
			var map = MapTest.Map06;
			var state0 = State.Create(map);
			var state1 = state0.Move(map, MoveDirection.x, state0.PlayerToMove);

			var lookup = new NodeLookup();

			var act0 = lookup.Get(1, map, state0);
			var act1 = lookup.Get(1, map, state1);

			Assert.AreNotEqual(act0, act1);
			Assert.AreEqual(2, lookup.Nodes);
		}
	}
}
