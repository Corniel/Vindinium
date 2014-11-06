using NUnit.Framework;
using System;
using Vindinium.Ygritte2.Decisions;
using Vindinium.Ygritte2.Evaluation;

namespace Vindinium.UnitTests.Ygritte2
{
	[TestFixture]
	public class RootNodeTest
	{
		[Test]
		public void Process_MoveToFreeMineDistance1_GoNorth()
		{
			var map = Map.Parse(
				"@1[]  $-$-  []@2\r\n" +
				"                \r\n" +
				"                \r\n" +
				"                \r\n" +
				"                \r\n" +
				"                \r\n" +
				"                \r\n" +
				"@3[]  $-$-  []@4\r\n");

			var state = State.Create(0,
				Hero.Create(44, map[3, 1], 0, 6),
				Hero.Create(90, map[2, 6], 2, 4),
				Hero.Create(90, map[7, 0], 0, 0, true),
				Hero.Create(90, map[7, 7], 0, 0, true),
				MineOwnership.Create(0, 0, 2, 2));

			var root = new RootNode(state);
			var evaluator =Evaluator.Get(map);
			var data = new ProcessData(root.Lookup, map, evaluator);

			var act = root.GetMove(data, TimeSpan.FromSeconds(2));

			Console.WriteLine(root.Lookup.LogNodeCounts());
			var exp = MoveDirection.N;
			Assert.AreEqual(exp, act.Direction);
		}
	}
}
