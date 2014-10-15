using NUnit.Framework;
using Vindinium.Decisions;

namespace Vindinium.UnitTests.Decisions
{
	[TestFixture]
	public class ScoreCollectionTest
	{
		[Test]
		public void Compare_TwoScoresForPlayer1_Min1()
		{
			var scores0 = new ScoreCollection(new GoldScore(1024), new GoldScore(0), new GoldScore(1), new GoldScore(2));
			var scores1 = new ScoreCollection(new GoldScore(1000), new GoldScore(3), new GoldScore(5), new GoldScore(7));

			var act = scores0.Compare(scores1, PlayerType.Hero1);
			var exp = -1;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Compare_TwoScoresForPlayer2_1()
		{
			var scores0 = new ScoreCollection(new GoldScore(1024), new GoldScore(0), new GoldScore(5), new GoldScore(7));
			var scores1 = new ScoreCollection(new GoldScore(1040), new GoldScore(3), new GoldScore(1), new GoldScore(2));

			var act = scores0.Compare(scores1, PlayerType.Hero2);
			var exp = 1;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Compare_TwoScoresForPlayer3_1()
		{
			var scores0 = new ScoreCollection(new GoldScore(1024), new GoldScore(3), new GoldScore(1), new GoldScore(2));
			var scores1 = new ScoreCollection(new GoldScore(1000), new GoldScore(0), new GoldScore(5), new GoldScore(7));

			var act = scores0.Compare(scores1, PlayerType.Hero3);
			var exp = 1;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Compare_TwoScoresForPlayer4_Minus1()
		{
			var scores0 = new ScoreCollection(new GoldScore(1024), new GoldScore(3), new GoldScore(1), new GoldScore(7));
			var scores1 = new ScoreCollection(new GoldScore(1000), new GoldScore(0), new GoldScore(5), new GoldScore(2));

			var act = scores0.Compare(scores1, PlayerType.Hero4);
			var exp = -1;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToConsoleDisplay_Hero3_H3Star()
		{
			var scores = new ScoreCollection(new GoldScore(1024), new GoldScore(3), new GoldScore(1), new GoldScore(7));

			var act = scores.ToConsoleDisplay(PlayerType.Hero3);
			var exp = "h1: 1,024, h2: 3, h3*: 1, h4: 7";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_None_DescribingString()
		{
			var scores = new ScoreCollection(new GoldScore(1024), new GoldScore(3), new GoldScore(1), new GoldScore(7));

			var act = scores.DebuggerDisplay;
			var exp = "h1: 1,024, h2: 3, h3: 1, h4: 7";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Sort_1234_0123()
		{
			int p1, p2, p3, p4;
			
			var s1 = 1;
			var s2 = 2;
			var s3 = 3;
			var s4 = 4;

			ScoreCollection.Sort(s1, s2, s3, s4, out p1, out p2, out p3, out p4);

			Assert.AreEqual(0, p1, "p1");
			Assert.AreEqual(1, p2, "p2");
			Assert.AreEqual(2, p3, "p3");
			Assert.AreEqual(3, p4, "p4");
		}
		[Test]
		public void Sort_2234_0023()
		{
			int p1, p2, p3, p4;

			var s1 = 2;
			var s2 = 2;
			var s3 = 3;
			var s4 = 4;

			ScoreCollection.Sort(s1, s2, s3, s4, out p1, out p2, out p3, out p4);

			Assert.AreEqual(0, p1, "p1");
			Assert.AreEqual(0, p2, "p2");
			Assert.AreEqual(2, p3, "p3");
			Assert.AreEqual(3, p4, "p4");
		}
		[Test]
		public void Sort_2224_0003()
		{
			int p1, p2, p3, p4;

			var s1 = 2;
			var s2 = 2;
			var s3 = 2;
			var s4 = 4;

			ScoreCollection.Sort(s1, s2, s3, s4, out p1, out p2, out p3, out p4);

			Assert.AreEqual(0, p1, "p1");
			Assert.AreEqual(0, p2, "p2");
			Assert.AreEqual(0, p3, "p3");
			Assert.AreEqual(3, p4, "p4");
		}
		[Test]
		public void Sort_2845_0312()
		{
			int p1, p2, p3, p4;

			var s1 = 2;
			var s2 = 8;
			var s3 = 4;
			var s4 = 5;

			ScoreCollection.Sort(s1, s2, s3, s4, out p1, out p2, out p3, out p4);

			Assert.AreEqual(0, p1, "p1");
			Assert.AreEqual(3, p2, "p2");
			Assert.AreEqual(1, p3, "p3");
			Assert.AreEqual(2, p4, "p4");
		}
		[Test]
		public void Sort_9773_3110()
		{
			int p1, p2, p3, p4;

			var s1 = 9;
			var s2 = 7;
			var s3 = 7;
			var s4 = 3;

			ScoreCollection.Sort(s1, s2, s3, s4, out p1, out p2, out p3, out p4);

			Assert.AreEqual(3, p1, "p1");
			Assert.AreEqual(1, p2, "p2");
			Assert.AreEqual(1, p3, "p3");
			Assert.AreEqual(0, p4, "p4");
		}

		[Test]
		public void ContinueProccesingAlphas_WorseAlphaForHero4_IsFalse()
		{
			var alphas = new ScoreCollection(new GoldScore(1000), new GoldScore(1000), new GoldScore(1000), new GoldScore(1000));
			var test = new ScoreCollection(new GoldScore(1000), new GoldScore(2000), new GoldScore(1000), new GoldScore(1001));

			ScoreCollection exp = new ScoreCollection(new GoldScore(1000), new GoldScore(1000), new GoldScore(1000), new GoldScore(1000));
			ScoreCollection act;

			var result = alphas.ContinueProccesingAlphas(test, PlayerType.Hero2, out act);

			Assert.IsFalse(result);
			Assert.AreEqual(exp.DebuggerDisplay, act.DebuggerDisplay);
		}

		[Test]
		public void ContinueProccesingAlphas_WorseAlpha_IsTrue()
		{
			var alphas = new ScoreCollection(new GoldScore(1000), new GoldScore(1000), new GoldScore(1000), new GoldScore(1000));
			var test = new ScoreCollection(new GoldScore(900), new GoldScore(750), new GoldScore(1000), new GoldScore(800));

			ScoreCollection exp = new ScoreCollection(new GoldScore(1000), new GoldScore(1000), new GoldScore(1000), new GoldScore(1000));
			ScoreCollection act;

			var result = alphas.ContinueProccesingAlphas(test, PlayerType.Hero2, out act);

			Assert.IsTrue(result);
			Assert.AreEqual(exp.DebuggerDisplay, act.DebuggerDisplay);
		}

		[Test]
		public void ContinueProccesingAlphas_BetterAlpha_IsTrue()
		{
			var alphas = new ScoreCollection(new GoldScore(1000), new GoldScore(1000), new GoldScore(1000), new GoldScore(1000));
			var test = new ScoreCollection(new GoldScore(900), new GoldScore(2000), new GoldScore(1000), new GoldScore(800));

			ScoreCollection exp = new ScoreCollection(new GoldScore(1000), new GoldScore(2000), new GoldScore(1000), new GoldScore(1000));
			ScoreCollection act;

			var result = alphas.ContinueProccesingAlphas(test, PlayerType.Hero2, out act);

			Assert.IsTrue(result);
			Assert.AreEqual(exp.DebuggerDisplay, act.DebuggerDisplay);
		}
	}
}
