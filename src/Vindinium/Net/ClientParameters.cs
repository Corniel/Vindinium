using System;
using System.Diagnostics;
using System.IO;

namespace Vindinium.Net
{
	[Serializable, DebuggerDisplay("{DebugToString()}")]
	public class ClientParameters
	{
		/// <summary>Constructor.</summary>
		public ClientParameters()
		{
			this.Turns = 1200;
		}

		/// <summary>The indentifying key.</summary>
		public string Key { get; set; }
		/// <summary>The URL to connect with.</summary>
		public string Url { get; set; }

		/// <summary>Is training mode or not.</summary>
		public bool IsTraining { get; set; }
		/// <summary>The number of turns.</summary>
		public int Turns { get; set; }
		/// <summary>The name of the map.</summary>
		public string Map { get; set; }

		/// <summary>Creates data for a create game request.</summary>
		public string GetCreateGameData()
		{
			

			var data = "key=" + this.Key;
			if (this.IsTraining)
			{
				data += "&turns=" + this.Turns;

				if (!String.IsNullOrEmpty(this.Map))
				{
					data += "&map=" + this.Map;
				}
			}
			return data;
		}

		/// <summary>Creates data for a move request.</summary>
		public string GetMoveData(MoveDirection direction)
		{
			return String.Format("key={0}&dir={1}", this.Key, direction.ToLabel());
		}

		/// <summary>Represents the client parameters as debug string.</summary>
		public string DebugToString()
		{
			return String.Format("{0}/key={1}", Url, Key);
		}

		/// <summary>Creates client parameters.</summary>
		public static ClientParameters Create(string key, string serverUrl, bool trainingsmode, int turns = 1200, string map = null)
		{
			if (String.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
			if (String.IsNullOrEmpty(key)) { throw new ArgumentNullException("serverUrl"); }
			if (turns < 1) { throw new ArgumentOutOfRangeException("turns", "Turns should be bigger than 0."); }

			var parameters = new ClientParameters()
			{
				Key = key,
				Url = Path.Combine(serverUrl, "api", trainingsmode ? "training" : "arena"),
				IsTraining = trainingsmode,
				Turns = turns,
				Map = map,
			};
			return parameters;
		}
	}
}
