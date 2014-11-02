using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Vindinium.UnitTests
{
    /// <summary>Tests the health SVO.</summary>
    [TestFixture]
    public class HealthTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Health TestStruct = 17;

        #region health const tests

        /// <summary>Health.MinValue should be equal to the default of health.</summary>
        [Test]
		public void MinValue_None_EqualsDefault()
        {
            Assert.AreEqual(default(Health), Health.MinValue);
        }

        #endregion

        #region TryParse tests

		/// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            Health val;

            string str = "17";

            Assert.IsTrue(Health.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "17");
        }
 
        #endregion

		#region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = Health.MinValue.ToString();
            var exp = "1";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = Health.MaxValue.ToString();
            var exp = "100";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "17";
            Assert.AreEqual(exp, act);
        }

		[Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Health.Parse("17").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "00017,0";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Health.MinValue.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Health.MinValue.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(16, HealthTest.TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
			Assert.IsTrue(Health.MinValue.Equals(Health.MinValue));
        }
		
        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(HealthTest.TestStruct.Equals(HealthTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(HealthTest.TestStruct.Equals(Health.MinValue));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Health.MinValue.Equals(HealthTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(HealthTest.TestStruct.Equals((object)HealthTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(HealthTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(HealthTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = HealthTest.TestStruct;
            var r = HealthTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = HealthTest.TestStruct;
            var r = HealthTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion
        
        #region IComparable tests

        /// <summary>Orders a list of healths ascending.</summary>
        [Test]
        public void OrderBy_Health_AreEqual()
        {
            var item0 = Health.Parse("17");
            var item1 = Health.Parse("27");
            var item2 = Health.Parse("34");
            var item3 = Health.Parse("97");

            var inp = new List<Health>() { Health.MinValue, item3, item2, item0, item1, Health.MinValue };
            var exp = new List<Health>() { Health.MinValue, Health.MinValue, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of healths descending.</summary>
        [Test]
        public void OrderByDescending_Health_AreEqual()
        {
			var item0 = Health.Parse("17");
			var item1 = Health.Parse("27");
			var item2 = Health.Parse("34");
			var item3 = Health.Parse("97");

            var inp = new List<Health>() { Health.MinValue, item3, item2, item0, item1, Health.MinValue };
            var exp = new List<Health>() { item3, item2, item1, item0, Health.MinValue, Health.MinValue };
            var act = inp.OrderByDescending(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [Test]
        public void CompareTo_ObjectTestStruct_0()
        {
            object other = (object)TestStruct;

            var exp = 0;
            var act = TestStruct.CompareTo(other);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Health l = 17;
            Health r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Health l = 21;
            Health r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Health l = 17;
            Health r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Health l = 21;
            Health r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Health l = 17;
            Health r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Health l = 21;
            Health r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion
        
        #region Casting tests

        [Test]
        public void Explicit_StringToHealth_AreEqual()
        {
            var exp = TestStruct;
            var act = (Health)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_HealthToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Explicit_Int32ToHealth_AreEqual()
        {
            var exp = TestStruct;
            var act = (Health)17;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_HealthToInt32_AreEqual()
        {
            var exp = 17;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Properties

		[Test]
		public void HitThreashold_1To20_1()
		{
			for (int i = 1; i <= 20; i++)
			{
				Health h = i;

				Assert.AreEqual(1, h.HitThreashold, "{0}", i);
			}
		}
		[Test]
		public void HitThreashold_21To40_2()
		{
			for (int i = 21; i <= 40; i++)
			{
				Health h = i;

				Assert.AreEqual(2, h.HitThreashold, "{0}", i);
			}
		}
		[Test]
		public void HitThreashold_41To60_3()
		{
			for (int i = 41; i <= 60; i++)
			{
				Health h = i;

				Assert.AreEqual(3, h.HitThreashold, "{0}", i);
			}
		}
		[Test]
		public void HitThreashold_61To80_4()
		{
			for (int i = 61; i <= 80; i++)
			{
				Health h = i;

				Assert.AreEqual(4, h.HitThreashold, "{0}", i);
			}
		}
		[Test]
		public void HitThreashold_81To100_5()
		{
			for (int i = 81; i <= 100; i++)
			{
				Health h = i;

				Assert.AreEqual(5, h.HitThreashold, "{0}", i);
			}
		}

        #endregion

		#region Methods

		[Test]
		public void Step_2To100_1Lower()
		{
			for (int i = 2; i <= 100; i++)
			{
				Health h = i;
				var act = h.Step();
				var exp = (Health)(i-1);
				Assert.AreEqual(act, exp);
			}
		}

		[Test]
		public void Step_MinValue_MinValue()
		{
			var act = Health.MinValue.Step();
			var exp = Health.MinValue;
			Assert.AreEqual(act, exp);
		}

		[Test]
		public void Drink_1To50_50Higher()
		{
			for (int i = 1; i <= 50; i++)
			{
				Health h = i;
				var act = h.Drink();
				var exp = (Health)(i + 50);
				Assert.AreEqual(act, exp);
			}
		}

		[Test]
		public void Drink_50To100_MaxValue()
		{
			for (int i = 50; i <= 100; i++)
			{
				Health h = i;
				var act = h.Drink();
				var exp = Health.MaxValue;
				Assert.AreEqual(act, exp);
			}
		}

		[Test]
		public void Slammed_21To100_20Lower()
		{
			for (int i = 21; i <= 100; i++)
			{
				Health h = i;
				var act = h.Slammed();
				var exp = (Health)(i - 20);
				Assert.AreEqual(act, exp);
			}
		}

		[Test]
		public void Slammed_1To20_MaxValue()
		{
			for (int i = 1; i <= 20; i++)
			{
				Health h = i;
				var act = h.Slammed();
				var exp = Health.Dead;
				Assert.AreEqual(act, exp);
			}
		}

		#endregion

	}

    [Serializable]
    public class HealthSerializeObject
    {
        public int Id { get; set; }
        public Health Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
