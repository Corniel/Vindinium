using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Vindinium
{
	/// <summary>Represents a Hero (state) in Vindinium.</summary>
	[DebuggerDisplay("{DebugToString()}")]
	public struct Hero : ISerializable, IXmlSerializable
	{
		public const int PositionX = 8;
		public const int PositionY = 16;
		public const int PositionMines = 24;
		public const int PositionGold = 32;

		public const ulong MaskHealth = Bits.Mask07;
		public const ulong MaskDimension = Bits.Mask08;
		public const ulong MaskDimensions = Bits.Mask16;
		public const ulong MaskMines = Bits.Mask08;
		public const ulong MaskClearMines = (ulong.MaxValue ^ (MaskMines << PositionMines));
		public const ulong MaskClearHealth = (ulong.MaxValue ^ MaskHealth);

		public const int HealthMax = 100;
		public const int HealthTavern = 50;
		public const int HealthBattle = 20;
		public const int HealthThirst = 1;
		public const int HealthThirsted = 1;
		public const int HealthMin = 0;

		public const int CostsTavern = 2;

		private const int DimensionMin = 0;
		private const int DimensionMax = 255;
		private const int GoldMin = 0;

		/// <summary>Constructs a new Hero</summary>
		public Hero(int health, int x, int y, int mines, int gold)
		{
			#region Garding (Debug only)
#if DEBUG
			if (health < Hero.HealthMin || health > Hero.HealthMax)
			{
				throw new ArgumentOutOfRangeException("health");
			}
			if (x < Hero.DimensionMin || x > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("x");
			}
			if (y < Hero.DimensionMin || y > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("y");
			}
			if (mines < Hero.DimensionMin || mines > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("mines");
			}
			if (gold < Hero.GoldMin)
			{
				throw new ArgumentOutOfRangeException("gold");
			}
#endif
			#endregion

			m_Value = (ulong)health;
			m_Value |= ((ulong)x) << PositionX;
			m_Value |= ((ulong)y) << PositionY;
			m_Value |= ((ulong)mines) << PositionMines;
			m_Value |= ((ulong)gold) << PositionGold;
		}

		/// <summary>The underlying value.</summary>
		private ulong m_Value;

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Hero based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Hero(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			m_Value = info.GetUInt64("Value");
		}

		/// <summary>Adds the underlying propererty of Hero to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) serialize a Hero.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Hero from an xml writer.</summary>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			m_Value = XmlConvert.ToUInt64(s);
		}

		/// <summary>Writes the Hero to an xml writer.</summary>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(XmlConvert.ToString(m_Value));
		}

		#endregion

		#region properties

		/// <summary>Gets the health of the Hero.</summary>
		public int Health { get { return (int)(m_Value & MaskHealth); } }

		/// <summary>Gets the x coordinate of the Hero.</summary>
		public int X { get { return (int)((m_Value >> PositionX) & MaskDimension); } }
		/// <summary>Gets the y coordinate of the Hero.</summary>
		public int Y { get { return (int)((m_Value >> PositionY) & MaskDimension); } }

		/// <summary>Gets the combined dimensions of the location.</summary>
		public ulong Dimensions { get { return m_Value & (MaskDimensions << PositionX); } }

		/// <summary>Gets the the number of gold mines owned by the Hero.</summary>
		public int Mines { get { return (int)((m_Value >> PositionMines) & MaskMines); } }

		/// <summary>Gets the the amount of gold of the Hero.</summary>
		public int Gold { get { return (int)(m_Value >> PositionGold); } }

		#endregion

		/// <summary>Returns true if the tile is the Hero's current location.</summary>
		public bool IsCurrentLocation(Tile tile)
		{
			return this.Dimensions == tile.Dimensions;
		}

		/// <summary>The hero loses a mine.</summary>
		public Hero LoseMine(int mines)
		{
#if DEBUG
			if (mines < 1 || mines != this.Mines)
			{
				throw new ArgumentOutOfRangeException("mines", "should be at least 1.");
			}
#endif
			unchecked
			{
				var val = m_Value & Hero.MaskClearMines;

				val |= (ulong)((mines - 1)) << PositionMines;

				return new Hero() { m_Value = val };
			}
		}

		/// <summary>Gets a version of the hero who is just slapped.</summary>
		public Hero Slapped(int health)
		{
#if DEBUG
			if (health < 1 || health != this.Health)
			{
				throw new ArgumentOutOfRangeException("health", "should be at least 1.");
			}
#endif
			unchecked
			{
				health -= Hero.HealthBattle;
				var val = m_Value & Hero.MaskClearHealth;
				val |= (ulong)health;
				return new Hero() { m_Value = val };
			}
		}

		/// <summary>Returns a System.String that represents the current BIC for debug purposes.</summary>
		public string DebugToString()
		{
			return string.Format(CultureInfo.InvariantCulture,
				"Hero[{0},{1}] Health: {2}, Mines: {3}, Gold: {4:#,##0}",
				X, Y, Health, Mines, Gold);
		}

		#region IEquatable

		/// <summary>Gets the hash code of the Hero.</summary>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the object equals the hero otherwise false.</summary>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Hero left, Hero right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Hero left, Hero right)
		{
			return !(left == right);
		}
		#endregion

		/// <summary>Gets an intial Hero.</summary>
		public static Hero Initial(int x, int y)
		{
			return new Hero(Hero.HealthMax, x, y, 0, 0);
		}

		/// <summary>Gets an intial Hero.</summary>
		public static Hero Initial(Map map, PlayerType player)
		{
			var tile = map.GetSpawn(player);
			return Hero.Initial(tile.X, tile.Y);
		}

		/// <summary>Respawns the Hero.</summary>
		/// <remarks>
		/// When a hero HP drops to zero, he ceases to live. The hero immediatly
		/// respawns on the map at its initial position, with HP=100. The hero
		/// loses control of all his gold mines, but keeps all his amassed gold.
		/// Be careful, when a hero respawns on its initial position, every
		/// opponent that may be at this position is automatically killed. So,
		/// you should avoid being at the respawn position of one of the heroes...
		///
		/// A hero can't die of thirst. Thirst can put the hero HP to 1, but not to 0.
		/// </remarks>
		public static Hero Respawn(Tile location, int gold)
		{
			#region Garding (Debug only)
#if DEBUG
			if (location.X < Hero.DimensionMin || location.X > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("location.X");
			}
			if (location.Y < Hero.DimensionMin || location.Y > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("location.Y");
			}
			if (gold < Hero.GoldMin)
			{
				throw new ArgumentOutOfRangeException("gold");
			}
#endif
			#endregion

			var hero = new Hero();

			hero.m_Value = (ulong)Hero.HealthMax;
			hero.m_Value |= location.Dimensions;
			hero.m_Value |= ((ulong)gold) << PositionGold;

			return hero;
		}

		/// <summary>Creates a new hero.</summary>
		public static Hero Create(int health, Tile location, int mines, int gold)
		{
			#region Garding (Debug only)
#if DEBUG
			if (health < Hero.HealthMin || health > Hero.HealthMax)
			{
				throw new ArgumentOutOfRangeException("health");
			}
			if (location.X < Hero.DimensionMin || location.X > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("location.X");
			}
			if (location.Y < Hero.DimensionMin || location.Y > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("location.Y");
			}
			if (mines < Hero.DimensionMin || mines > Hero.DimensionMax)
			{
				throw new ArgumentOutOfRangeException("mines");
			}
			if (gold < Hero.GoldMin)
			{
				throw new ArgumentOutOfRangeException("gold");
			}
#endif
			#endregion

			var hero = new Hero();

			hero.m_Value = (ulong)health;
			hero.m_Value |= location.Dimensions;
			hero.m_Value |= ((ulong)mines) << PositionMines;
			hero.m_Value |= ((ulong)gold) << PositionGold;

			return hero;
		}

		/// <summary>Creates a hero based on the serialization hero.</summary>
		/// <remarks>
		/// The X and Y coordinate of the Pos object are flipped.
		/// </remarks>
		public static Hero Create(Serialization.Hero hero)
		{
			return new Hero(hero.life, hero.pos.y, hero.pos.x, hero.mineCount, hero.gold);
		}
	}
}
