using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Vindinium.Logging;

namespace Vindinium.Data
{
	[Serializable]
	public class Ratings
	{
		[XmlElement("p")]
		public List<Player> Players { get; set; }

		public Player GetPlayer(string query)
		{
			query = (query ?? String.Empty).ToLower();
			return this.Players.FirstOrDefault(p => p.Name.ToLower() == query);
		}

		#region I/O operations

		public void Save() { Save(Game.GamesDir); }

		public void Save(DirectoryInfo dir)
		{
			Save(new FileInfo(Path.Combine(dir.FullName, "Ratings.xml")));
		}

		/// <summary>Saves the ratings to a file.</summary>
		/// <param name="fileName">
		/// The name of the file.
		/// </param>
		/// <param name="mode">
		/// The file mode.
		/// </param>
		public void Save(string fileName, FileMode mode = FileMode.Create) { Save(new FileInfo(fileName), mode); }

		/// <summary>Saves the ratings to a file.</summary>
		/// <param name="file">
		/// The file to save to.
		/// </param>
		/// <param name="mode">
		/// The file mode.
		/// </param>
		public void Save(FileInfo file, FileMode mode = FileMode.Create)
		{
			using (var stream = new FileStream(file.FullName, mode, FileAccess.Write))
			{
				Save(stream);
			}
		}

		/// <summary>Saves the ratings to a stream.</summary>
		/// <param name="stream">
		/// The stream to save to.
		/// </param>
		public void Save(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(Ratings));
			serializer.Serialize(stream, this);
		}

		public static Ratings Load() { return Load(Game.GamesDir); }

		public static Ratings Load(DirectoryInfo dir)
		{
			return Load(new FileInfo(Path.Combine(dir.FullName, "Ratings.xml")));
		}

		/// <summary>Loads the ratings from a file.</summary>
		/// <param name="fileName">
		/// The name of the file.
		/// </param>
		public static Ratings Load(string fileName) { return Load(new FileInfo(fileName)); }

		/// <summary>Loads the ratings from a file.</summary>
		/// <param name="file">
		/// The file to load from.
		/// </param>
		public static Ratings Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
			{
				return Load(stream);
			}
		}

		/// <summary>Loads the ratings from a stream.</summary>
		/// <param name="stream">
		/// The stream to load from.
		/// </param>
		public static Ratings Load(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(Ratings));
			return (Ratings)serializer.Deserialize(stream);
		}

		#endregion
	}
}
