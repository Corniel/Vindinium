using NUnit.Framework;
using Vindinium.DrunkenViking.Strategies;

namespace Vindinium.UnitTests.DrunkenViking.Stragties
{
	[TestFixture]
	public class SafePathCollectionTest
	{
		[Test]
		public void Process_()
		{
			var map = MapTest.Map06;
			var state = State.Create(0,
				new Hero(80, 2, 2, 0, 0),
				new Hero(80, 0, 4, 0, 0),
				new Hero(80, 5, 2, 0, 0),
				new Hero(80, 5, 3, 0, 0),
				MineOwnership.Create(map));

			var act = SafePathCollection.Create(map, state);
			act.Procces();

			Assert.AreEqual(-1, act.Count);
		}
	}
}
