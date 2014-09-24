using System;
using NUnit.Framework;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class MapTest
	{
		public static readonly Map Map06 = Map.Parse(
			"@1$-    $-@2\r\n" +
			"  []    []  \r\n" +
			"            \r\n" +
			"            \r\n" +
			"  []    []  \r\n" +
			"@3$-    $-@4\r\n");

		public static readonly Map Map16 = Map.Parse(
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

		[Test]
		public void Parse_Map06_Successful()
		{
			var act = Map06;

			Assert.AreEqual(4, act.Mines.Length, "Mines");
			Assert.AreEqual(4, act.Tavernes.Length, "Tavernes");
		}

		[Test]
		public void Parse_Map16_Successful()
		{
			var act = Map16;

			Assert.AreEqual(28, act.Mines.Length, "Mines");
			Assert.AreEqual(4, act.Tavernes.Length, "Tavernes");

			Assert.AreEqual("Tile[3,0] GoldMine, Neighbors: 2", act.Mines[0].DebugToString());
			Assert.AreEqual("Tile[6,5] Hero1, Neighbors: 4", act.GetSpawn(PlayerType.Hero1).DebugToString());
		}
	}
}
