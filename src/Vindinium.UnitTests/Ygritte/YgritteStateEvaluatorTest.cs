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
	public class YgritteStateEvaluatorTest
	{
		[Test]
		public void Evaluate_SomeValues_AreEqual()
		{
			var map = MapTest.Map20;
			var state = State.Create(
				101,
				new Hero(100, 0, 0, 1, 10),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 11),
				new Hero(100, 0, 0, 1, 13),
				MineOwnership20.Empty);

			var score = YgritteStateEvaluator.Instance.Evaluate(map, state);

			var act = score.DebuggerDisplay;
			var exp = "h1: 393.60, h2: 394.60, h3: 394.60, h4: 395.20";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Evaluate_Performance_()
		{
			var sw = new Stopwatch();
			var runs = 1000000;
			var map = MapTest.Map20;
			var rnd = new System.Random(17);
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
				var score = YgritteStateEvaluator.Instance.Evaluate(map, state);
				sw.Stop();
			}
			Console.WriteLine("{0:#,##0.00}M/s", runs / sw.Elapsed.TotalMilliseconds/1000.0);
		}
	}
}
