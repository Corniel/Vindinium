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
			Assert.AreEqual(4, act.Tavernes.Length, "Tavernes");
			Assert.AreEqual(36, act.Count, "act.Count");
		}

		[Test]
		public void Parse_Map16_Successful()
		{
			var act = Map16;

			Assert.AreEqual(28, act.Mines.Length, "Mines");
			Assert.AreEqual(4, act.Tavernes.Length, "Tavernes");
			Assert.AreEqual(212, act.Count, "act.Count");

			Assert.AreEqual("Tile[3,0] GoldMine, Neighbors: 2", act.Mines[0].DebugToString());
			Assert.AreEqual("Tile[6,5] Hero1, Neighbors: 4", act.GetSpawn(PlayerType.Hero1).DebugToString());
		}

		[Test]
		public void GetSpwan_Map16_Successful()
		{
			var act = Map16;

			Assert.AreEqual(null, act.GetSpawn(PlayerType.None), "None");
			Assert.AreEqual(act[06, 05], act.GetSpawn(PlayerType.Hero1), "Hero1");
			Assert.AreEqual(act[06, 14], act.GetSpawn(PlayerType.Hero2), "Hero2");
			Assert.AreEqual(act[13, 14], act.GetSpawn(PlayerType.Hero3), "Hero3");
			Assert.AreEqual(act[13, 05], act.GetSpawn(PlayerType.Hero4), "Hero4");
		}
	}
}
