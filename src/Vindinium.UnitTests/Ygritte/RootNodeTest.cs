using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			var root = new RootNode(map, state);

			var act = root.GetMove(map, TimeSpan.FromMilliseconds(1900));
		}

		[Test]
		public void Process_20ply_()
		{
			var map = MapTest.Map18;
			var state = State.Create(1,
				Hero.Create(90, map[2, 3], 0, 0),
				Hero.Create(90, map[5, 6], 0, 0),
				Hero.Create(90, map[9, 9], 0, 0),
				Hero.Create(90, map[1, 9], 0, 0),
				MineOwnership.Create(map));

			var root = new RootNode(map, state);
			root.InitializeMoveMappings(map);

			var sw = new Stopwatch();
			sw.Start();
			for (int i = 1; i < 20; i++)
			{
				root.Process(map, i, PotentialScore.EmptyCollection);
			}
			//root.Process(map, 20, PontentialScore.EmptyCollection);
			sw.Stop();

			Console.WriteLine("Ellapsed: {0:0.0}", sw.Elapsed.TotalMilliseconds);

			Console.WriteLine(Node.Lookup.DebuggerDisplay);

			Console.WriteLine(Node.Lookup.LogNodeCounts());

			var act = root.BestMove;
			var exp = MoveDirection.S;
			Assert.AreEqual(exp, act);
		}

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

			var state = State.Create(1200 - 4,
				Hero.Create(44, map[3, 1], 0, 6),
				Hero.Create(90, map[2, 6], 2, 4),
				Hero.Create(90, map[7, 0], 0, 0, true),
				Hero.Create(90, map[7, 7], 0, 0, true),
				MineOwnership.Create(0, 0, 2, 2));

			var root = new RootNode(map, state);
			root.InitializeMoveMappings(map);

			var sw = new Stopwatch();
			sw.Start();
			for (var depth = state.Turn + 1; depth < 1220; depth++)
			{
				root.Process(map, depth, PotentialScore.EmptyCollection);
			}
			sw.Stop();

			Console.WriteLine("Ellapsed: {0:0.0}", sw.Elapsed.TotalMilliseconds);

			Console.WriteLine(Node.Lookup.DebuggerDisplay);

			Console.WriteLine(Node.Lookup.LogNodeCounts());

			var act = root.BestMove;
			var exp = MoveDirection.N;
			Assert.AreEqual(exp, act);
		}


		[Test]
		public void Process_MoveToFreeMineDistance3_GoNorth()
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

			var state = State.Create(1200 - 16,
				Hero.Create(44, map[3, 2], 0, 9),
				Hero.Create(90, map[2, 6], 2, 4, true),
				Hero.Create(90, map[7, 0], 0, 0, true),
				Hero.Create(90, map[7, 7], 0, 0, true),
				MineOwnership.Create(0, 0, 2, 2));

			var root = new RootNode(map, state);
			root.InitializeMoveMappings(map);

			var sw = new Stopwatch();
			sw.Start();
			for (var depth = state.Turn + 1; depth < 1220; depth++)
			{
				root.Process(map, depth, PotentialScore.EmptyCollection);
			}
			sw.Stop();

			foreach (var score in root.Scores)
			{
				Console.WriteLine("{0} {1}", score.Item1, score.Item2.ToConsoleDisplay(state.PlayerToMove));
			}

			Console.WriteLine("Ellapsed: {0:0.0}", sw.Elapsed.TotalMilliseconds);

			Console.WriteLine(Node.Lookup.DebuggerDisplay);

			Console.WriteLine(Node.Lookup.LogNodeCounts());

			var act = root.BestMove;
			var exp = MoveDirection.N;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Process_12ply_Profiling()
		{
			var map = MapTest.Map18;
			var state = State.Create(1,
				Hero.Create(90, map[2, 3], 0, 0),
				Hero.Create(90, map[5, 6], 0, 0),
				Hero.Create(90, map[9, 9], 0, 0),
				Hero.Create(90, map[1, 9], 0, 0),
				MineOwnership.Create(map));

			var root = new RootNode(map, state);
			root.InitializeMoveMappings(map);

			var sw = new Stopwatch();
			sw.Start();
			root.Process(map, 12, PotentialScore.EmptyCollection);
			sw.Stop();

			Console.WriteLine("Ellapsed: {0:0.0}", sw.Elapsed.TotalMilliseconds);
		}
	}
}
