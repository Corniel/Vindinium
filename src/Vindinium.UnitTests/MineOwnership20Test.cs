using NUnit.Framework;
using System;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class MineOwnership20Test
	{
		public static readonly IMineOwnership TestValue = MineOwnership20.Create(0, 0, 1, 1, 1, 4, 2, 0, 3, 0, 4, 1, 0, 0, 0, 0, 1);

		[Test]
		public void Set_0Hero1_AreEqual()
		{
			var act = MineOwnership20.Empty.Set(0, PlayerType.Hero1);
			var exp = "1...................";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Set_3Hero1_AreEqual()
		{
			var act = MineOwnership20.Empty.Set(3, PlayerType.Hero1);
			var exp = "...1................";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Set_4Hero2_AreEqual()
		{
			var act = MineOwnership20.Empty.Set(4, PlayerType.Hero2);
			var exp = "....2...............";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Set_15Hero3_AreEqual()
		{
			var act = MineOwnership20.Empty.Set(15, PlayerType.Hero3);
			var exp = "...............3....";
			Assert.AreEqual(exp, act.DebugToString());
		}
		[Test]
		public void Set_63Hero4_AreEqual()
		{
			var act = MineOwnership20.Empty.Set(19, PlayerType.Hero4);
			var exp = "...................4";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Create_Int32Array_AreEqual()
		{
			var act = MineOwnership20.Create(0, 0, 1, 1, 1, 4, 2, 0, 3, 0, 4, 1, 0, 0, 0, 0, 1);
			var exp = "..11142.3.41....1...";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Count_None_11()
		{
			Assert.AreEqual(11, TestValue.Count(PlayerType.None));
		}
		[Test]
		public void Count_Hero1_23()
		{
			Assert.AreEqual(05, TestValue.Count(PlayerType.Hero1));
		}
		[Test]
		public void Count_Hero2_1()
		{
			Assert.AreEqual(01, TestValue.Count(PlayerType.Hero2));
		}
		[Test]
		public void Count_Hero3_1()
		{
			Assert.AreEqual(01, TestValue.Count(PlayerType.Hero3));
		}
		[Test]
		public void Count_Hero4_2()
		{
			Assert.AreEqual(02, TestValue.Count(PlayerType.Hero4));
		}
#if DEBUG
		[Test, ExpectedException(typeof(ArgumentException))]
		public void ChangeOwnership_NoneToHero1_ThrowsNewArgumentException()
		{
			TestValue.ChangeOwnership(PlayerType.None, PlayerType.Hero1, 20);
		}
#endif
		[Test]
		public void ChangeOwnership_Hero1ToNone_AreEqual()
		{
			var act = TestValue.ChangeOwnership(PlayerType.Hero1, PlayerType.None, 20);
			var exp = ".....42.3.4.........";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void ChangeOwnership_Hero3ToNone_AreEqual()
		{
			var act = TestValue.ChangeOwnership(PlayerType.Hero3, PlayerType.None, 20);
			var exp = "..11142...41....1...";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void ChangeOwnership_Hero1ToHero2_AreEqual()
		{
			var act = TestValue.ChangeOwnership(PlayerType.Hero1, PlayerType.Hero2, 20);
			var exp = "..22242.3.42....2...";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void ChangeOwnership_Hero3ToHero1_AreEqual()
		{
			var act = TestValue.ChangeOwnership(PlayerType.Hero3, PlayerType.Hero1, 20);
			var exp = "..11142.1.41....1...";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void ChangeOwnership_Hero1ToHero3_AreEqual()
		{
			var act = TestValue.ChangeOwnership(PlayerType.Hero1, PlayerType.Hero3, 20);
			var exp = "..33342.3.43....3...";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void ChangeOwnership_Hero2ToHero4_AreEqual()
		{
			var act = TestValue.ChangeOwnership(PlayerType.Hero2, PlayerType.Hero4, 20);
			var exp = "..11144.3.41....1...";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void CreateFromTiles_String_EqualsTestStruct()
		{
			var act = MineOwnership20.Empty.UpdateFromTiles("$-  $-  $1  $1  $1$4##$2  []  $-  $3  $-  $4  $1  $-  $-  $-  $-  $1");
			var exp = TestValue;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_Empty_0()
		{
			var act =  MineOwnership20.Empty.GetHashCode();
			var exp = 0;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_TestValue_51057219()
		{
			var act = TestValue.GetHashCode();
			var exp = 51057219;
			Assert.AreEqual(exp, act);
		}
	}
}
