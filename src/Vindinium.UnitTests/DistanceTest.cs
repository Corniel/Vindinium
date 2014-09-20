using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using NUnit.Framework;
using Vindinium;

namespace Vindinium.UnitTests
{
    /// <summary>Tests the Distance SVO.</summary>
    [TestFixture]
    public class DistanceTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Distance TestStruct = 123;

        #region Distance const tests

        /// <summary>Distance.Empty should be equal to the default of Distance.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Distance), Distance.Unknown);
        }

        #endregion

        #region Distance IsEmpty tests

        /// <summary>Distance.IsEmpty() should be true for the default of Distance.</summary>
        [Test]
		public void IsUnknown_Default_IsTrue()
        {
            Assert.IsTrue(default(Distance).IsUnknown());
        }
        /// <summary>Distance.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
		public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            Distance val;

            string str = null;

            Assert.IsTrue(Distance.TryParse(str, out val), "Valid");
            Assert.AreEqual("?", val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            Distance val;

            string str = string.Empty;

            Assert.IsTrue(Distance.TryParse(str, out val), "Valid");
            Assert.AreEqual("?", val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be Distance.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            Distance val;

            string str = "?";

            Assert.IsTrue(Distance.TryParse(str, out val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            Distance val;

            string str = "123";

            Assert.IsTrue(Distance.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            Distance val;

            string str = "string";

            Assert.IsFalse(Distance.TryParse(str, out val), "Valid");
            Assert.AreEqual("?", val.ToString(), "Value");
        }

		#endregion
 
        #region IFormattable / ToString tests

        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = Distance.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

		[Test]
        public void ToString_TestStruct_123()
        {
            var act = TestStruct.ToString("");
            var exp = "123";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Distance.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Distance.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Distance.Unknown.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(65412, DistanceTest.TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Distance.Unknown.Equals(Distance.Unknown));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Distance.Parse("12.0", CultureInfo.InvariantCulture);
            var r = Distance.Parse("12", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(DistanceTest.TestStruct.Equals(DistanceTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(DistanceTest.TestStruct.Equals(Distance.Unknown));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Distance.Unknown.Equals(DistanceTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(DistanceTest.TestStruct.Equals((object)DistanceTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(DistanceTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(DistanceTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = DistanceTest.TestStruct;
            var r = DistanceTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = DistanceTest.TestStruct;
            var r = DistanceTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion
        
        #region IComparable tests

        /// <summary>Orders a list of Distances ascending.</summary>
        [Test]
        public void OrderBy_Distance_AreEqual()
        {
			Distance item0 = 132;
			Distance item1 = 232;
			Distance item2 = 332;
			Distance item3 = 432;

            var inp = new List<Distance>() { Distance.Unknown, item3, item2, item0, item1, Distance.Unknown };
			var exp = new List<Distance>() { item0, item1, item2, item3, Distance.Unknown, Distance.Unknown };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Distances descending.</summary>
        [Test]
        public void OrderByDescending_Distance_AreEqual()
        {
			Distance item0 = 132;
			Distance item1 = 232;
			Distance item2 = 332;
			Distance item3 = 432;

            var inp = new List<Distance>() { Distance.Unknown, item3, item2, item0, item1, Distance.Unknown };
            var exp = new List<Distance>() { Distance.Unknown, Distance.Unknown, item3, item2, item1, item0};
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
            Distance l = 17;
            Distance r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Distance l = 21;
            Distance r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Distance l = 17;
            Distance r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Distance l = 21;
            Distance r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Distance l = 17;
            Distance r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Distance l = 21;
            Distance r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion
        
        #region Casting tests

        [Test]
        public void Explicit_StringToDistance_AreEqual()
        {
            var exp = TestStruct;
            var act = (Distance)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_DistanceToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Explicit_Int32ToDistance_AreEqual()
        {
            var exp = TestStruct;
            var act = (Distance)123;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_DistanceToInt32_AreEqual()
        {
            var exp = 123;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion
    }

    [Serializable]
    public class DistanceSerializeObject
    {
        public int Id { get; set; }
        public Distance Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
