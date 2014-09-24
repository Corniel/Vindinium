using System;
using NUnit.Framework;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class HeroTest
	{
		[Test]
		public void Initial_XAndY_AreEqual()
		{
			var hero = Hero.Initial(23, 24);
			var act = hero.DebugToString();
			var exp = "Hero[23,24] Health: 100, Mines: 0, Gold: 0";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebugToString_None_DescribingDebugString()
		{
			var hero = new Hero(51, 12, 13, 1, 1025);
			var act = hero.DebugToString();
			var exp = "Hero[12,13] Health: 51, Mines: 1, Gold: 1,025";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void LoseMine_1_0MinesLeft()
		{
			var hero = new Hero(51, 12, 13, 1, 1025);
			var act = hero.LoseMine(1);
			var exp = "Hero[12,13] Health: 51, Mines: 0, Gold: 1,025";

			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void LoseMine_17_16MinesLeft()
		{
			var hero = new Hero(51, 12, 13, 17, 1025);
			var act = hero.LoseMine(17);
			var exp = "Hero[12,13] Health: 51, Mines: 16, Gold: 1,025";

			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Slapped_51_31HealthLeft()
		{
			var hero = new Hero(51, 12, 13, 1, 1025);
			var act = hero.Slapped(51);
			var exp = "Hero[12,13] Health: 31, Mines: 1, Gold: 1,025";

			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Slapped_21_1HealthLeft()
		{
			var hero = new Hero(21, 12, 13, 17, 1025);
			var act = hero.Slapped(21);
			var exp = "Hero[12,13] Health: 1, Mines: 17, Gold: 1,025";

			Assert.AreEqual(exp, act.DebugToString());
		}
	}
}
