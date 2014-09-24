using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class StateTest
	{
		[Test]
		public void Move_EAndS_Hero1HasMine()
		{
			var map = MapTest.Map06;
			var init = State.Create(map);

			var act1 = init.Move(map, MoveDirection.E, PlayerType.Hero1);
			Console.WriteLine(act1.ToUnitTestString());

			var exp1 = @"Hero[0,0] Health: 79, Mines: 1, Gold: 1
Hero[5,0] Health: 100, Mines: 0, Gold: 0
Hero[0,5] Health: 100, Mines: 0, Gold: 0
Hero[5,5] Health: 100, Mines: 0, Gold: 0
Turn: 1
Mines: 1...............................................................";

			AreEqual(exp1, act1);

			var act2 = act1.Move(map, MoveDirection.S, PlayerType.Hero2);
			var exp2 = @"Hero[0,0] Health: 79, Mines: 1, Gold: 1
Hero[5,1] Health: 99, Mines: 0, Gold: 0
Hero[0,5] Health: 100, Mines: 0, Gold: 0
Hero[5,5] Health: 100, Mines: 0, Gold: 0
Turn: 2
Mines: 1...............................................................";

			AreEqual(exp2, act2);

			var act3 = act2.Move(map, MoveDirection.E, PlayerType.Hero3);
			var exp3 = @"Hero[0,0] Health: 79, Mines: 1, Gold: 1
Hero[5,1] Health: 99, Mines: 0, Gold: 0
Hero[0,5] Health: 79, Mines: 1, Gold: 1
Hero[5,5] Health: 100, Mines: 0, Gold: 0
Turn: 3
Mines: 1.3.............................................................";

			AreEqual(exp3, act3);
		}

		[Test]
		public void Move_KillHeroAndGetMines_Hero1HasMine()
		{
			var map = MapTest.Map06;

			var mines = MineOwnership.Create( 1, 1, 1, 4 );
			var hero1 = new Hero(18, 3, 3, 3, 100);
			var hero2 = new Hero(80, 2, 3, 0, 101);
			var hero3 = new Hero(81, 4, 4, 0, 102);
			var hero4 = new Hero(82, 1, 5, 1, 103);

			var act0 = State.Create(16, hero1, hero2, hero3, hero4, mines);
			var exp0 = @"Hero[3,3] Health: 18, Mines: 3, Gold: 100
Hero[2,3] Health: 80, Mines: 0, Gold: 101
Hero[4,4] Health: 81, Mines: 0, Gold: 102
Hero[1,5] Health: 82, Mines: 1, Gold: 103
Turn: 16
Mines: 1114............................................................";

			AreEqual(exp0, act0);

			var act1 = act0.Move(map, MoveDirection.E, PlayerType.Hero2);
			var exp1 = @"Hero[0,0] Health: 100, Mines: 0, Gold: 100
Hero[2,3] Health: 79, Mines: 3, Gold: 104
Hero[4,4] Health: 81, Mines: 0, Gold: 102
Hero[1,5] Health: 82, Mines: 1, Gold: 103
Turn: 17
Mines: 2224............................................................";

			AreEqual(exp1, act1);
		}

		[Test]
		public void Move_DrinkBeer_Hero1HasMine()
		{
			var map = MapTest.Map06;

			var mines = MineOwnership.Create(1, 1, 1, 4);
			var hero1 = new Hero(18, 1, 2, 3, 100);
			var hero2 = new Hero(80, 2, 3, 0, 101);
			var hero3 = new Hero(81, 4, 4, 0, 102);
			var hero4 = new Hero(82, 1, 5, 1, 103);

			var act0 = State.Create(16, hero1, hero2, hero3, hero4, mines);
			var exp0 = @"Hero[1,2] Health: 18, Mines: 3, Gold: 100
Hero[2,3] Health: 80, Mines: 0, Gold: 101
Hero[4,4] Health: 81, Mines: 0, Gold: 102
Hero[1,5] Health: 82, Mines: 1, Gold: 103
Turn: 16
Mines: 1114............................................................";

			AreEqual(exp0, act0);

			var act1 = act0.Move(map, MoveDirection.N, PlayerType.Hero1);
			var exp1 = @"Hero[1,2] Health: 67, Mines: 3, Gold: 101
Hero[2,3] Health: 80, Mines: 0, Gold: 101
Hero[4,4] Health: 81, Mines: 0, Gold: 102
Hero[1,5] Health: 82, Mines: 1, Gold: 103
Turn: 17
Mines: 1114............................................................";

			AreEqual(exp1, act1);
		}

		private static void AreEqual(string exp, State act)
		{
			Console.WriteLine();
			Console.WriteLine(act.ToUnitTestString());
			Assert.AreEqual(exp, act.ToUnitTestString());
		}
	}
}
