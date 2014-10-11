using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.Ygritte.Decisions;

namespace Vindinium.UnitTests.Ygritte
{
	[TestFixture]
	public class ScoreTest
	{
		[Test]
		public void Create_SomeValues_AreEqual()
		{
			var state = State.Create(
				101,
				new Hero(100, 0, 0, 1, 10),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 13),
				MineOwnership20.Empty);

			var score = Score.Create(state);

			var act = score.DebuggerDisplay;
			var exp = "Score: [1]0, 290, [2]1, 291, [3]1, 291, [4]3, 292";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Create_Performance_()
		{
			var sw = new Stopwatch();
			var runs = 1000000;
			var rnd = new Random(17);
			for (int i = 0; i < runs; i++)
			{
				var state = State.Create(
					rnd.Next(1, 1201),
					new Hero(100, 0, 0, rnd.Next(12), rnd.Next(4000)),
					new Hero(100, 0, 0, rnd.Next(12), rnd.Next(4000)),
					new Hero(100, 0, 0, rnd.Next(12), rnd.Next(4000)),
					new Hero(100, 0, 0, rnd.Next(12), rnd.Next(4000)),
					MineOwnership20.Empty);

				sw.Start();
				var score = Score.Create(state);
				sw.Stop();
			}
			Console.WriteLine("{0:#,##0.00}k/s", runs / sw.Elapsed.TotalMilliseconds);
		}
	}
}
