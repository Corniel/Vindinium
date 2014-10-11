using NUnit.Framework;
using System;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class MineOwnership128Test
	{
		public static readonly IMineOwnership TestValue = MineOwnership.Parse(".1....................2...22...............333..........2.............................4.................2...........33.........1");
		public static readonly IMineOwnership TestValue1 = MineOwnership.Parse(new String('1', 128));
		public static readonly IMineOwnership TestValue2 = MineOwnership.Parse(new String('2', 128));
		public static readonly IMineOwnership TestValue3 = MineOwnership.Parse(new String('3', 128));
		public static readonly IMineOwnership TestValue4 = MineOwnership.Parse(new String('4', 128));

		[Test]
		public void Min64EqualsBitwiseAnd63_64To128_AreEqual()
		{
			for (var i = 64; i < 128; i++)
			{
				Assert.AreEqual(i - 64, i & 63, i.ToString());
			}
		}

		[Test]
		public void Set_0Hero1_AreEqual()
		{
			var act = MineOwnership128.Empty.Set(0, PlayerType.Hero1);
			var exp = "1...............................................................................................................................";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void Set_3Hero1_AreEqual()
		{
			var act = MineOwnership128.Empty.Set(3, PlayerType.Hero1);
			var exp = "...1............................................................................................................................";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void Set_4Hero2_AreEqual()
		{
			var act = MineOwnership128.Empty.Set(4, PlayerType.Hero2);
			var exp = "....2...........................................................................................................................";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void Set_15Hero3_AreEqual()
		{
			var act = MineOwnership128.Empty.Set(15, PlayerType.Hero3);
			var exp = "...............3................................................................................................................";
			Assert.AreEqual(exp, act.ToString());
		}
		[Test]
		public void Set_63Hero4_AreEqual()
		{
			var act = MineOwnership128.Empty.Set(63, PlayerType.Hero4);
			var exp = "...............................................................4................................................................";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void Create_Int32Array_AreEqual()
		{
			var act = MineOwnership128.Create(0, 0, 1, 1, 1, 4, 2, 0, 3, 0, 4, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
			var exp = "..11142.3.41....1....1....1....1....1....1......................................................................................";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void Count_None_115()
		{
			Assert.AreEqual(115, TestValue.Count(PlayerType.None));
		}
		[Test]
		public void Count_Hero1_2()
		{
			Assert.AreEqual(2, TestValue.Count(PlayerType.Hero1));
		}
		[Test]
		public void Count_Hero2_5()
		{
			Assert.AreEqual(5, TestValue.Count(PlayerType.Hero2));
		}
		[Test]
		public void Count_Hero3_5()
		{
			Assert.AreEqual(5, TestValue.Count(PlayerType.Hero3));
		}
		[Test]
		public void Count_Hero4_1()
		{
			Assert.AreEqual(1, TestValue.Count(PlayerType.Hero4));
		}
#if DEBUG
		[Test, ExpectedException(typeof(ArgumentException))]
		public void ChangeOwnership_NoneToHero1_ThrowsNewArgumentException()
		{
			TestValue.ChangeOwnership(PlayerType.None, PlayerType.Hero1,128);
		}
#endif
		[Test]
		public void ChangeOwnership_Hero1ToNone_AreEqual()
		{
			var act = TestValue1.ChangeOwnership(PlayerType.Hero1, PlayerType.None, 128);
			var exp = MineOwnership128.Empty;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ChangeOwnership_Hero3ToNone_AreEqual()
		{
			var act = TestValue3.ChangeOwnership(PlayerType.Hero3, PlayerType.None, 128);
			var exp = MineOwnership128.Empty;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ChangeOwnership_Hero1ToHero2_AreEqual()
		{
			var act = TestValue1.ChangeOwnership(PlayerType.Hero1, PlayerType.Hero2,128);
			var exp = TestValue2;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ChangeOwnership_Hero3ToHero1_AreEqual()
		{
			var act = TestValue3.ChangeOwnership(PlayerType.Hero3, PlayerType.Hero1,128);
			var exp = TestValue1;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ChangeOwnership_Hero1ToHero3_AreEqual()
		{
			var act = TestValue1.ChangeOwnership(PlayerType.Hero1, PlayerType.Hero3,128);
			var exp = TestValue3;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ChangeOwnership_Hero2ToHero4_AreEqual()
		{
			var act = TestValue2.ChangeOwnership(PlayerType.Hero2, PlayerType.Hero4,128);
			var exp = TestValue4;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void CreateFromTiles_String_EqualsTestStruct()
		{
			var act = MineOwnership128.Empty.UpdateFromTiles("$-  $-  $1  $1  $1$4##$2  []  $-  $3  $-  $4  $1  $-  $-  $-  $-  $1");
			var exp = "..11142.3.41....1...............................................................................................................";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void GetHashCode_Empty_0()
		{
			var act =  MineOwnership128.Empty.GetHashCode();
			var exp = 0;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_TestValue_Min2015358206()
		{
			var act = TestValue.GetHashCode();
			var exp = -2015358206;
			Assert.AreEqual(exp, act);
		}
	}
}
