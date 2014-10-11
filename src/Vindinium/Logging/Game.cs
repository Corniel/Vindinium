using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Vindinium.Logging
{
	[Serializable]
	public class Game
	{
		public static DirectoryInfo GamesDir
		{
			get
			{
				var games = ConfigurationManager.AppSettings["games"];
				return new DirectoryInfo(games);
			}
		}

		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("Hero")]
		public Hero[] Heros { get; set; }
		public Map Map { get; set; }

		[XmlElement("Turn")]
		public List<Turn> Turns { get; set; }

		#region I/O operations

		public void Save() { Save(GamesDir); }

		public void Save(DirectoryInfo dir)
		{
			Save(new FileInfo(Path.Combine(dir.FullName, Id + ".xml")));
		}

		/// <summary>Saves the game to a file.</summary>
		/// <param name="fileName">
		/// The name of the file.
		/// </param>
		/// <param name="mode">
		/// The file mode.
		/// </param>
		public void Save(string fileName, FileMode mode = FileMode.Create) { Save(new FileInfo(fileName), mode); }

		/// <summary>Saves the game to a file.</summary>
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

		/// <summary>Saves the game to a stream.</summary>
		/// <param name="stream">
		/// The stream to save to.
		/// </param>
		public void Save(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(Game));
			serializer.Serialize(stream, this);
		}

		/// <summary>Loads the game from a file.</summary>
		/// <param name="fileName">
		/// The name of the file.
		/// </param>
		public static Game Load(string fileName) { return Load(new FileInfo(fileName)); }

		/// <summary>Loads the game from a file.</summary>
		/// <param name="file">
		/// The file to load from.
		/// </param>
		public static Game Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
			{
				return Load(stream);
			}
		}

		/// <summary>Loads the game from a stream.</summary>
		/// <param name="stream">
		/// The stream to load from.
		/// </param>
		public static Game Load(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(Game));
			return (Game)serializer.Deserialize(stream);
		}

		#endregion

		public static Game Create(Serialization.Game serialization)
		{
			var game = new Game()
			{
				Id = serialization.id,
				Map = new Map(),
				Turns = new List<Turn>(),
			};
			game.Map.Mp = Vindinium.Map.Parse(serialization.board.ToRows());
			game.Heros = serialization.heroes.Select(h => new Hero()
				{
					Id = h.id,
					Elo = h.elo,
					Name = h.name,
				}).ToArray();
			return game;
		}
	}
}
