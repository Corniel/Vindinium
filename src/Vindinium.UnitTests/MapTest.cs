using System;
using NUnit.Framework;
using System.Diagnostics;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class MapTest
	{
		#region Maps

		public static readonly Map Map06 = Map.Parse(
			"@1$-    $-@2\r\n" +
			"  []    []  \r\n" +
			"            \r\n" +
			"            \r\n" +
			"  []    []  \r\n" +
			"@3$-    $-@4\r\n");

		public static readonly Map Map10 = Map.Parse(
			"      ########      \r\n" +
			"$-  @1[]####[]@4  $-\r\n" +
			"    ############    \r\n" +
			"                    \r\n" +
			"##  ##  ####  ##  ##\r\n" +
			"##  ##  ####  ##  ##\r\n" +
			"                    \r\n" +
			"    ############    \r\n" +
			"$-  @2[]####[]@3  $-\r\n" +
			"      ########      \r\n");

		public static readonly Map Map10Mines8 = Map.Parse(
			"##  []########[]  ##\r\n" +
			"    @1########@4    \r\n" +
			"$-      ####      $-\r\n" +
			"####            ####\r\n" +
			"######  $-$-  ######\r\n" +
			"######  $-$-  ######\r\n" +
			"####            ####\r\n" +
			"$-      ####      $-\r\n" +
			"    @2########@3    \r\n" +
			"##  []########[]  ##\r\n");


		public static readonly Map Map18 = Map.Parse(
			"$-                                $-\r\n" +
			"    ##  $-                $-  ##    \r\n" +
			"        @1                  @4      \r\n" +
			"####        ##  $-$-  ##        ####\r\n" +
			"##    ##  ##  ##    ##  ##  ##    ##\r\n" +
			"$-                                $-\r\n" +
			"        ##      [][]      ##        \r\n" +
			"  ##      $-  ##    ##  $-      ##  \r\n" +
			"                                    \r\n" +
			"                                    \r\n" +
			"  ##      $-  ##    ##  $-      ##  \r\n" +
			"        ##      [][]      ##        \r\n" +
			"$-                                $-\r\n" +
			"##    ##  ##  ##    ##  ##  ##    ##\r\n" +
			"####        ##  $-$-  ##        ####\r\n" +
			"                                    \r\n" +
			"    ##@2$-                $-@3##    \r\n" +
			"$-                                $-\r\n");

		public static readonly Map Map20 = Map.Parse(
			"######$-    $-############$-    $-######\r\n" +
			"######        ##        ##        ######\r\n" +
			"####[]    ####            ####    []####\r\n" +
			"##      ####  ##        ##  ####      ##\r\n" +
			"####            $-    $-            ####\r\n" +
			"##########  @1            @4  ##########\r\n" +
			"############  ####    ####  ############\r\n" +
			"$-##$-        ############        $-##$-\r\n" +
			"  $-      $-################$-      $-  \r\n" +
			"        ########################        \r\n" +
			"        ########################        \r\n" +
			"  $-      $-################$-      $-  \r\n" +
			"$-##$-        ############        $-##$-\r\n" +
			"############  ####    ####  ############\r\n" +
			"##########  @2            @3  ##########\r\n" +
			"####            $-    $-            ####\r\n" +
			"##      ####  ##        ##  ####      ##\r\n" +
			"####[]    ####            ####    []####\r\n" +
			"######        ##        ##        ######\r\n" +
			"######$-    $-############$-    $-######\r\n");

		#endregion

		[Test, ExpectedException(typeof(ArgumentException))]
		public void Parse_InvalidString_ThrowsException()
		{
			Map.Parse("InvalidString");
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Parse_NullArray_ThrowsException()
		{
			string[] lines = null;
			Map.Parse(lines);
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void Parse_EmptyArray_ThrowsException()
		{
			string[] lines = new string[0];
			Map.Parse(lines);
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void Parse_InvalidLineLenghts_ThrowsException()
		{
			string[] lines = new string[]{"##  ##", "$1  "};
			Map.Parse(lines);
		}

		[Test]
		public void Parse_Map06_Successful()
		{
			var act = Map06;

			Assert.AreEqual(4, act.Mines.Length, "Mines");
			Assert.AreEqual(4, act.Taverns.Length, "Taverns");
			Assert.AreEqual(36, act.Count, "act.Count");
		}

		[Test]
		public void Parse_Map16_Successful()
		{
			var act = Map20;

			Assert.AreEqual(28, act.Mines.Length, "Mines");
			Assert.AreEqual(4, act.Taverns.Length, "Taverns");
			Assert.AreEqual(212, act.Count, "act.Count");

			Assert.AreEqual("Tile[3,0] GoldMine, Neighbors: 2", act.Mines[0].DebugToString());
			Assert.AreEqual("Tile[6,5] Hero1, Neighbors: 4", act.GetSpawn(PlayerType.Hero1).DebugToString());
		}

		[Test]
		public void GetSpwan_Map16_Successful()
		{
			var act = Map20;

			Assert.AreEqual(null, act.GetSpawn(PlayerType.None), "None");
			Assert.AreEqual(act[06, 05], act.GetSpawn(PlayerType.Hero1), "Hero1");
			Assert.AreEqual(act[06, 14], act.GetSpawn(PlayerType.Hero2), "Hero2");
			Assert.AreEqual(act[13, 14], act.GetSpawn(PlayerType.Hero3), "Hero3");
			Assert.AreEqual(act[13, 05], act.GetSpawn(PlayerType.Hero4), "Hero4");
		}

		[Test]
		public void GetDistances_Tile_DistancesArray()
		{
			Distance[,] distances = MapTest.Map20.GetDistances(MapTest.Map20[13, 12]);
			var act = distances.ToUnitTestString();
			Console.WriteLine(act);
			var exp = @"
-------------------------------------------------------------
|  |  |  |  |29|30|  |  |  |  |  |  |  |  |23|22|  |  |  |  |
-------------------------------------------------------------
|  |  |  |27|28|29|30|  |22|21|20|21|  |23|22|21|20|  |  |  |
-------------------------------------------------------------
|  |  |  |26|27|  |  |22|21|20|19|20|21|  |  |20|19|  |  |  |
-------------------------------------------------------------
|  |27|26|25|  |  |22|  |20|19|18|19|  |15|  |  |18|19|20|  |
-------------------------------------------------------------
|  |  |25|24|23|22|21|20|  |18|17|  |15|14|15|16|17|18|  |  |
-------------------------------------------------------------
|  |  |  |  |  |21|20|19|18|17|16|15|14|13|14|  |  |  |  |  |
-------------------------------------------------------------
|  |  |  |  |  |  |21|  |  |18|17|  |  |12|  |  |  |  |  |  |
-------------------------------------------------------------
|  |  |  |19|20|21|22|  |  |  |  |  |  |11|10| 9| 8|  |  |  |
-------------------------------------------------------------
|21|  |19|18|19|  |  |  |  |  |  |  |  |  |  | 8| 7| 8|  |10|
-------------------------------------------------------------
|20|19|18|17|  |  |  |  |  |  |  |  |  |  |  |  | 6| 7| 8| 9|
-------------------------------------------------------------
|19|18|17|16|  |  |  |  |  |  |  |  |  |  |  |  | 5| 6| 7| 8|
-------------------------------------------------------------
|20|  |16|15|14|  |  |  |  |  |  |  |  |  |  | 3| 4| 5|  | 9|
-------------------------------------------------------------
|  |  |  |14|13|12|11|  |  |  |  |  |  | 0| 1| 2| 3|  |  |  |
-------------------------------------------------------------
|  |  |  |  |  |  |10|  |  | 7| 6|  |  | 1|  |  |  |  |  |  |
-------------------------------------------------------------
|  |  |  |  |  |10| 9| 8| 7| 6| 5| 4| 3| 2| 3|  |  |  |  |  |
-------------------------------------------------------------
|  |  |14|13|12|11|10| 9|  | 7| 6|  | 4| 3| 4| 5| 6| 7|  |  |
-------------------------------------------------------------
|  |16|15|14|  |  |11|  | 9| 8| 7| 8|  | 4|  |  | 7| 8| 9|  |
-------------------------------------------------------------
|  |  |  |15|16|  |  |11|10| 9| 8| 9|10|  |  | 9| 8|  |  |  |
-------------------------------------------------------------
|  |  |  |16|17|18|19|  |11|10| 9|10|  |12|11|10| 9|  |  |  |
-------------------------------------------------------------
|  |  |  |  |18|19|  |  |  |  |  |  |  |  |12|11|  |  |  |  |
-------------------------------------------------------------
";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetDistances_TileTwice_CalculatedOnces()
		{
			var map = MapTest.Map06;
			var target = map.Taverns[0];

			map.ClearDistances();
			Assert.AreEqual(0, map.DistancesCount, "Distances Count before.");

			var act0 = map.GetDistances(target);
			Assert.AreEqual(1, map.DistancesCount, "Distances Count after first call.");

			var act1 = map.GetDistances(target);
			Assert.AreEqual(1, map.DistancesCount, "Distances Count after second call.");

			Assert.AreSame(act0, act1);
		}

		[Test]
		public void GetDistances_TilesWithEnermies_DistancesArray()
		{
			var map = MapTest.Map20;
			var targets = map.Taverns;
			var oppos = new Tile[]{ map[4,0], map[7, 17]};

			Distance[,] distances = MapTest.Map20.GetDistances(targets, oppos);
			var act = distances.ToUnitTestString();
			Console.WriteLine(act);
			var exp = @"
-------------------------------------------------------------
|  |  |  |  | X| X|  |  |  |  |  |  |  |  | 5| 4|  |  |  |  |
-------------------------------------------------------------
|  |  |  | X| X| X| X|  |15|14|14|15|  | 5| 4| 3| 2|  |  |  |
-------------------------------------------------------------
|  |  | 0| 1| X|  |  |15|14|13|13|14|15|  |  | 2| 1| 0|  |  |
-------------------------------------------------------------
|  | 2| 1| 2|  |  | 7|  |13|12|12|13|  | 7|  |  | 2| 1| 2|  |
-------------------------------------------------------------
|  |  | 2| 3| 4| 5| 6| 7|  |11|11|  | 7| 6| 5| 4| 3| 2|  |  |
-------------------------------------------------------------
|  |  |  |  |  | 6| 7| 8| 9|10|10| 9| 8| 7| 6|  |  |  |  |  |
-------------------------------------------------------------
|  |  |  |  |  |  | 8|  |  |11|11|  |  | 8|  |  |  |  |  |  |
-------------------------------------------------------------
|  |  |  |12|11|10| 9|  |  |  |  |  |  | 9|10|11|12|  |  |  |
-------------------------------------------------------------
|18|  |14|13|12|  |  |  |  |  |  |  |  |  |  |12|13|14|  |18|
-------------------------------------------------------------
|17|16|15|14|  |  |  |  |  |  |  |  |  |  |  |  |14|15|16|17|
-------------------------------------------------------------
| X| X| X| X|  |  |  |  |  |  |  |  |  |  |  |  |14|15|16|17|
-------------------------------------------------------------
| X|  | X| X| X|  |  |  |  |  |  |  |  |  |  |12|13|14|  |18|
-------------------------------------------------------------
|  |  |  | X| X| X| X|  |  |  |  |  |  | 9|10|11|12|  |  |  |
-------------------------------------------------------------
|  |  |  |  |  |  | X|  |  | X| X|  |  | 8|  |  |  |  |  |  |
-------------------------------------------------------------
|  |  |  |  |  | 6| X| X| X| X| X| X| X| 7| 6|  |  |  |  |  |
-------------------------------------------------------------
|  |  | 2| 3| 4| 5| 6| X|  | X| X|  | 7| 6| 5| 4| 3| 2|  |  |
-------------------------------------------------------------
|  | 2| 1| 2|  |  | 7|  | X| X| X| X|  | 7|  |  | 2| 1| 2|  |
-------------------------------------------------------------
|  |  | 0| 1| 2|  |  | X| X| X| X| X| X|  |  | 2| 1| 0|  |  |
-------------------------------------------------------------
|  |  |  | 2| 3| 4| 5|  | X| X| X| X|  | 5| 4| 3| 2|  |  |  |
-------------------------------------------------------------
|  |  |  |  | 4| 5|  |  |  |  |  |  |  |  | 5| 4|  |  |  |  |
-------------------------------------------------------------
";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetDistances_Performance_DistancesArray()
		{
			var sw = new Stopwatch();
			var map = MapTest.Map20;
			var runs = 100;

			sw.Start();
			for (var run = 0; run < runs; run++)
			{
				foreach (var tile in map)
				{
					var distances = map.GetDistances(tile);
				}
			}
			sw.Stop();
			Console.WriteLine("{0:#,##0.00}k/s", runs * map.Count / sw.Elapsed.TotalMilliseconds);
		}

		[Test]
		public void GetManhattanDistance_x3y0Tox3y0_0()
		{
			var map = MapTest.Map20;
			var hero1 = Hero.Create(100, map[03, 00], 0, 0);
			var hero2 = Hero.Create(100, map[03, 00], 0, 0);

			var act = Map.GetManhattanDistance(hero1, hero2);
			var exp = 0;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void GetManhattanDistance_x3y0Tox16y19_32()
		{
			var map = MapTest.Map20;
			var hero1 = Hero.Create(100, map[03, 00], 0, 0);
			var hero2 = Hero.Create(100, map[16, 19], 0, 0);

			var act = Map.GetManhattanDistance(hero1, hero2);
			var exp = 32;

			Assert.AreEqual(exp, act);
		}
	}
}
