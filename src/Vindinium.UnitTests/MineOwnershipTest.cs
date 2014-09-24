using NUnit.Framework;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class MineOwnershipTest
	{
		[Test]
		public void Set_0Hero1_AreEqual()
		{
			var act = MineOwnership.Empty.Set(0, PlayerType.Hero1);
			var exp = "1...............................";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Set_3Hero1_AreEqual()
		{
			var act = MineOwnership.Empty.Set(3, PlayerType.Hero1);
			var exp = "...1............................";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Set_4Hero2_AreEqual()
		{
			var act = MineOwnership.Empty.Set(4, PlayerType.Hero2);
			var exp = "....2...........................";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Set_15Hero3_AreEqual()
		{
			var act = MineOwnership.Empty.Set(15, PlayerType.Hero3);
			var exp = "...............3................";
			Assert.AreEqual(exp, act.DebugToString());
		}
		[Test]
		public void Set_31Hero4_AreEqual()
		{
			var act = MineOwnership.Empty.Set(31, PlayerType.Hero4);
			var exp = "...............................4";
			Assert.AreEqual(exp, act.DebugToString());
		}

		[Test]
		public void Create_Int32Array_AreEqual()
		{
			var act = MineOwnership.Create(0, 0, 1, 1, 1, 4, 2, 0, 3, 0, 4, 1, 0, 0, 0, 0, 1);
			var exp = "..11142.3.41....1...............";
			Assert.AreEqual(exp, act.DebugToString());
		}
		
	}
}
