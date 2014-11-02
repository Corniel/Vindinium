using System;
using NUnit.Framework;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class HeroTest
	{
		[Test]
		public void Create_Serialization_AreEqual()
		{
			var serialization = new Vindinium.Serialization.Hero()
			{
				pos = new Vindinium.Serialization.Pos() {  x = 12, y = 13 },
				gold = 14,
				mineCount = 7,
				life = 99,
				id = 4,
			};

			var act = Hero.Create(serialization);
			var exp = "Hero[13,12] Health: 99, Mines: 7, Gold: 14";
			Assert.AreEqual(exp, act.DebuggerDisplay);
		}

		[Test]
		public void Parse_NonCrashed_AreEqual()
		{
			var act = Hero.Parse("Hero[17,12] H:13, M:3, G:1024");
			var exp = "Hero[17,12] Health: 13, Mines: 3, Gold: 1,024";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}
		[Test]
		public void Parse_Crashed_AreEqual()
		{
			var act = Hero.Parse("Hero[17,12] H:13, M:3, G:1024, Crashed");
			var exp = "Hero[17,12] Health: 13, Mines: 3, Gold: 1,024, Crashed";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}
		[Test]
		public void Parse_NoMinesOrGold_AreEqual()
		{
			var act = Hero.Parse("Hero[1,1] H:80, M:0, G:0");
			var exp = "Hero[1,1] Health: 80, Mines: 0, Gold: 0";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}

		[Test]
		public void Initial_XAndY_AreEqual()
		{
			var hero = Hero.Initial(23, 24);
			var act = hero.DebuggerDisplay;
			var exp = "Hero[23,24] Health: 100, Mines: 0, Gold: 0";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebugToString_None_DescribingDebugString()
		{
			var hero = new Hero(51, 12, 13, 1, 1025);
			var act = hero.DebuggerDisplay;
			var exp = "Hero[12,13] Health: 51, Mines: 1, Gold: 1,025";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void LoseMine_1_0MinesLeft()
		{
			var hero = new Hero(51, 12, 13, 1, 1025);
			var act = hero.LoseMine(1);
			var exp = "Hero[12,13] Health: 51, Mines: 0, Gold: 1,025";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}

		[Test]
		public void LoseMine_17_16MinesLeft()
		{
			var hero = new Hero(51, 12, 13, 17, 1025);
			var act = hero.LoseMine(17);
			var exp = "Hero[12,13] Health: 51, Mines: 16, Gold: 1,025";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}

		[Test]
		public void SetHealth_31_31HealthLeft()
		{
			var hero = new Hero(51, 12, 13, 1, 1025);
			var act = hero.SetHealth(31);
			var exp = "Hero[12,13] Health: 31, Mines: 1, Gold: 1,025";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}

		[Test]
		public void SetHealth_1_1HealthLeft()
		{
			var hero = new Hero(21, 12, 13, 17, 1025);
			var act = hero.SetHealth(1);
			var exp = "Hero[12,13] Health: 1, Mines: 17, Gold: 1,025";

			Assert.AreEqual(exp, act.DebuggerDisplay);
		}
	}
}
