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
	public class NodeComparerTest
	{
		[Test]
		public void Compare_EqualPositionMoreGold_RightBest()
		{
			var map = MapTest.Map18;
			var left = State.Create(
				101,
				new Hero(100, 0, 0, 1, 10),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 13),
				MineOwnership20.Empty);

			var right = State.Create(
				101,
				new Hero(100, 0, 0, 1, 20),
				new Hero(100, 0, 0, 1, 21),
				new Hero(100, 0, 0, 1, 21),
				new Hero(100, 0, 0, 1, 23),
				MineOwnership20.Empty);

			var l = new Node(map, left);
			var r = new Node(map, right);

			var list = new List<Node>() { l, r };
			list.Sort(NodeComparer.Get(PlayerType.Hero1));

			var act = list[0];
			var exp = r;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Compare_DifferentPosition_LeftBest()
		{
			var map = MapTest.Map18;
			var left = State.Create(
				101,
				new Hero(100, 0, 0, 2, 10),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 13),
				MineOwnership20.Empty);

			var right = State.Create(
				101,
				new Hero(100, 0, 0, 1, 20),
				new Hero(100, 0, 0, 1, 21),
				new Hero(100, 0, 0, 1, 21),
				new Hero(100, 0, 0, 1, 23),
				MineOwnership20.Empty);

			var l = new Node(map, left);
			var r = new Node(map, right);

			var list = new List<Node>() { l, r };
			list.Sort(NodeComparer.Get(PlayerType.Hero1));

			var act = list[0];
			var exp = l;

			Assert.AreEqual(exp, act);
		}
	}
}
