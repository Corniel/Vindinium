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
	/// <summary>Represents a Distance.</summary>
	[DebuggerDisplay("{DebugToString()}")]
	[Serializable]
	[TypeConverter(typeof(DistanceTypeConverter))]
	public struct Distance : ISerializable, IXmlSerializable, IFormattable, IComparable, IComparable<Distance>
	{
		/// <summary>Represents an unknown distance.</summary>
		public static readonly Distance Unknown = new Distance() { m_Value = default(UInt16) };

		/// <summary>Represents a distance of zero.</summary>
		public static readonly Distance Zero = new Distance() { m_Value = UInt16.MaxValue };

		/// <summary>Represents a distance of one.</summary>
		public static readonly Distance One = new Distance() { m_Value = UInt16.MaxValue - 1 };

		/// <summary>Constructs a distance.</summary>
		private Distance(UInt16 val)
		{
			m_Value = (UInt16)(val ^ UInt16.MaxValue);
		}

		#region Properties

		/// <summary>The inner value of the Distance.</summary>
		private UInt16 m_Value;

		#endregion

		#region Methods

		/// <summary>Returns true if the Distance is empty, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == default(System.Int32); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Distance based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Distance(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			m_Value = info.GetUInt16("Value");
		}

		/// <summary>Adds the underlying propererty of Distance to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a Distance.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Distance from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of Distance.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the Distance to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of Distance.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current Distance for debug purposes.</summary>
		private string DebugToString()
		{
			return ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Returns a System.String that represents the current Distance.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Distance.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Distance.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current Distance.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return IsUnknown() ? "?" : ((int)this).ToString(format, formatProvider);
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compair with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this Distance.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Distance left, Distance right)
		{
			return left.m_Value == right.m_Value;
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Distance left, Distance right)
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
		/// An object that evaluates to a Distance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a Distance.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Distance)
			{
				return CompareTo((Distance)obj);
			}
			throw new ArgumentException("Argument must be a distance.", "obj");
		}

		/// <summary>Compares this instance with a specified Distance and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified Distance.
		/// </summary>
		/// <param name="other">
		/// The Distance to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Distance other) { return other.m_Value.CompareTo(m_Value); }

		public static Distance operator ++(Distance org)
		{
			unchecked
			{
				return new Distance() { m_Value = (UInt16)(org.m_Value - 1) };
			}
		}
		public static Distance operator --(Distance org)
		{
			unchecked
			{
				return new Distance() { m_Value = (UInt16)(org.m_Value + 1) };
			}
		}

		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(Distance l, Distance r) { return l.m_Value > r.m_Value; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(Distance l, Distance r) { return l.m_Value < r.m_Value; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(Distance l, Distance r) { return l.m_Value >= r.m_Value; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(Distance l, Distance r) { return l.m_Value <= r.m_Value; }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a Distance to a System.String.</summary>
		public static explicit operator string(Distance val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a Distance.</summary>
		public static explicit operator Distance(string str) { return Distance.Parse(str, CultureInfo.CurrentCulture); }

		/// <summary>Casts a Distance to a System.Int32.</summary>
		public static explicit operator Int32(Distance val) { return val.m_Value ^ UInt16.MaxValue; }
		/// <summary>Casts an System.Int32 to a Distance.</summary>
		public static implicit operator Distance(Int32 val) { return new Distance((UInt16)val); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a Distance.</summary>
		/// <param name="s">
		/// A string containing a Distance to convert.
		/// </param>
		/// <returns>
		/// A Distance.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Distance Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a Distance.</summary>
		/// <param name="s">
		/// A string containing a Distance to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A Distance.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Distance Parse(string s, IFormatProvider formatProvider)
		{
			Distance val;
			if (Distance.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException("Not a valid distance.");
		}

		/// <summary>Converts the string to a Distance.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Distance to convert.
		/// </param>
		/// <returns>
		/// The Distance if the string was converted successfully, otherwise Distance.Empty.
		/// </returns>
		public static Distance TryParse(string s)
		{
			Distance val;
			if (Distance.TryParse(s, out val))
			{
				return val;
			}
			return Distance.Unknown;
		}

		/// <summary>Converts the string to a Distance.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Distance to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Distance result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a Distance.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Distance to convert.
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
		public static bool TryParse(string s, IFormatProvider formatProvider, out Distance result)
		{
			result = Distance.Unknown;
			if (string.IsNullOrEmpty(s) || s == "?")
			{
				return true;
			}
			UInt16 val;

			if (UInt16.TryParse(s, NumberStyles.Number, formatProvider, out val))
			{
				result = new Distance(val);
				return true;
			}
			return false;
		}

		#endregion
	}
}
