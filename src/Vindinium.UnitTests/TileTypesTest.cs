using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.UnitTests
{
	[TestFixture]
	public class TileTypesTest
	{
		[Test]
		public void TryParse_Dollar1_GoldMine1()
		{
			var act = TileTypes.TryParse("$1");
			var exp = TileType.GoldMine1;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryParse_Dollar2_GoldMine2()
		{
			var act = TileTypes.TryParse("$2");
			var exp = TileType.GoldMine2;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryParse_Dollar3_GoldMine3()
		{
			var act = TileTypes.TryParse("$3");
			var exp = TileType.GoldMine3;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryParse_Dollar4_GoldMine4()
		{
			var act = TileTypes.TryParse("$4");
			var exp = TileType.GoldMine4;
			Assert.AreEqual(exp, act);
		}
	}
}
