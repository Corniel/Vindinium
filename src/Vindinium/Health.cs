using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Vindinium.Conversion;

namespace Vindinium
{
    /// <summary>Represents a health.</summary>
    [DebuggerDisplay("{DebugToString()}")]
    [TypeConverter(typeof(HealthTypeConverter))]
    public struct Health : ISerializable, IXmlSerializable, IFormattable, IComparable, IComparable<Health>
    {
		private const int DEAD = 0;
		private const int MIN = 1;
		private const int MAX = 100;

		private const int STEP = 1;
		private const int HIT = 20;
		private const int TAVERN = 50;


		/// <summary>Constructs a new health value type.</summary>
		private Health(int health)
		{
#if DEBUG
			if (health < MIN || health > MAX)
			{
				throw new ArgumentOutOfRangeException("health", "The value should be in the range [1,100].");
			}
#endif
			m_Value = health;
		}

		/// <summary>Represents an empty/not set health.</summary>
		public static readonly Health Dead = new Health() { m_Value = DEAD };

        /// <summary>Represents an empty/not set health.</summary>
		public static readonly Health MinValue = new Health() { m_Value = MIN };

        /// <summary>Represents an unknown (but set) health.</summary>
		public static readonly Health MaxValue = new Health() { m_Value = MAX };

        #region Properties

        /// <summary>The inner value of the health.</summary>
        private Int32 m_Value;

		/// <summary>The number of hits that can be absorbed.</summary>
		public Int32 HitThreashold { get { return 1 + (m_Value - 1) / HIT; } }

		/// <summary>Returns true if still alive, otherwise false.</summary>
		public bool IsAlive { get { return m_Value > DEAD; } }

		/// <summary>Returns true if dead, otherwise false.</summary>
		public bool IsDead { get { return m_Value == DEAD; } }

        #endregion

        #region Methods

		/// <summary>Do a step.</summary>
		public Health Step()
		{
			return new Health() { m_Value = m_Value > MIN ? m_Value - STEP : MIN };
		}

		public Health Step(int turns)
		{
			var health = m_Value - turns * STEP;
			return new Health() { m_Value = health < MIN ? MIN : health };
		}

		/// <summary>Drinks a beer.</summary>
		public Health Drink()
		{
			return new Health() { m_Value = m_Value >= (MAX - TAVERN) ? MAX : m_Value + TAVERN };
		}

		/// <summary>Handles being slammed.</summary>
		/// <remarks>
		/// If not strong enough anymore, die and get max health.
		/// </remarks>
		public Health Slammed()
		{
			return new Health() { m_Value = m_Value > HIT ? m_Value - HIT : DEAD };
		}

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of health based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Health(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new ArgumentNullException("info"); }
            m_Value = info.GetInt32("Value");
        }

        /// <summary>Adds the underlying propererty of health to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new ArgumentNullException("info"); }
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the xml schema to (de) xml serialize a health.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() { return null; }

        /// <summary>Reads the health from an xml writer.</summary>
        /// <remarks>
        /// Uses the string parse function of health.
        /// </remarks>
        /// <param name="reader">An xml reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the health to an xml writer.</summary>
        /// <remarks>
        /// Uses the string representation of health.
        /// </remarks>
        /// <param name="writer">An xml writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion
  
        #region IFormattable / ToString

        /// <summary>Returns a System.String that represents the current health for debug purposes.</summary>
         private string DebugToString()
        {
			return string.Format(CultureInfo.InvariantCulture, "Health: {0}, Threshold {1}", m_Value, HitThreashold);
        }

         /// <summary>Returns a System.String that represents the current health.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted System.String that represents the current health.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted System.String that represents the current health.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        /// <summary>Returns a formatted System.String that represents the current health.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
			return m_Value.ToString(format, formatProvider);
        }

        #endregion
        
        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compair with.</param>
        public override bool Equals(object obj){ return base.Equals(obj); }

        /// <summary>Returns the hash code for this health.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() { return m_Value.GetHashCode(); }

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Health left, Health right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Health left, Health right)
        {
            return !(left == right);
        }

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a health.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// value is not a health.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Health)
            {
                return CompareTo((Health)obj);
            }
            throw new ArgumentException("Argument must by of the type Health.", "obj");
        }

        /// <summary>Compares this instance with a specified health and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified health.
        /// </summary>
        /// <param name="other">
        /// The health to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Health other) { return m_Value.CompareTo(other.m_Value); }

        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Health l, Health r) { return l.CompareTo(r) < 0; }

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Health l, Health r) { return l.CompareTo(r) > 0; }

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Health l, Health r) { return l.CompareTo(r) <= 0; }

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Health l, Health r) { return l.CompareTo(r) >= 0; }

        #endregion
       
        #region (Explicit) casting

        /// <summary>Casts a health to a System.String.</summary>
        public static explicit operator string(Health val) { return val.ToString(CultureInfo.CurrentCulture); }
        /// <summary>Casts a System.String to a health.</summary>
        public static explicit operator Health(string str) { return Health.Parse(str, CultureInfo.CurrentCulture); }

        /// <summary>Casts a health to a System.Int32.</summary>
		public static explicit operator Int32(Health val) { return val.m_Value; }
        /// <summary>Casts an System.Int32 to a health.</summary>
        public static implicit operator Health(Int32 val) { return new Health(val); }

		/// <summary>Casts a health to a System.Int32.</summary>
		public static explicit operator UInt64(Health val) { return (ulong)val.m_Value; }
		/// <summary>Casts an System.Int32 to a health.</summary>
		public static implicit operator Health(UInt64 val) { return new Health((int)val); }

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a health.</summary>
        /// <param name="s">
        /// A string containing a health to convert.
        /// </param>
        /// <returns>
        /// A health.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Health Parse(string s)
        {
           return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a health.</summary>
        /// <param name="s">
        /// A string containing a health to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A health.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Health Parse(string s, IFormatProvider formatProvider)
        {
            Health val;
            if (Health.TryParse(s, formatProvider, out val))
            {
                return val;
            }
            throw new FormatException("Not a valid Health.");
        }

        /// <summary>Converts the string to a health.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a health to convert.
        /// </param>
        /// <returns>
        /// The health if the string was converted successfully, otherwise Health.Empty.
        /// </returns>
        public static Health TryParse(string s)
        {
            Health val;
            if (Health.TryParse(s, out val))
            {
                return val;
            }
            return Health.MinValue;
        }

        /// <summary>Converts the string to a health.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a health to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Health result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a health.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a health to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out Health result)
        {
            result = Health.MinValue;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
			Int32 val;
			if (Int32.TryParse(s, NumberStyles.Integer, formatProvider, out val) && val >= MIN && val <= MAX)
            {
				result = new Health(val);
                return true;
            }
            return false;
        }
		#endregion
	}
}