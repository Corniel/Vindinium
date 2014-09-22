using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Vindinium.Serialization
{
	[DataContract, DebuggerDisplay("{DebugToString()}")]
	public class GameResponse
	{
		[DataMember]
		public Game game;

		[DataMember]
		public Hero hero;

		[DataMember]
		public string token;

		[DataMember]
		public string viewUrl;

		[DataMember]
		public string playUrl;

		/// <summary>Represents the hero as debug string.</summary>
		public string DebugToString()
		{
			return string.Format("Response, Hero: {0}. Token: {1}", hero.id, token);
		}

		/// <summary>Seserializes a game response to a JSON string.</summary>
		public string ToJson()
		{
			using (var jsonStream = new MemoryStream())
			{
				Serializer.WriteObject(jsonStream, this);
				jsonStream.Position = 0;

				var reader = new StreamReader(jsonStream);
				return reader.ReadToEnd();
			}
		}

		/// <summary>Deserializes a game response from a JSON string.</summary>
		/// <param name="json">
		/// The JSON string representing the game response.
		/// </param>
		public static GameResponse FromJson(string json)
		{
			// convert string to stream
			var bytes = Encoding.UTF8.GetBytes(json);
			using (var jsonStream = new MemoryStream(bytes))
			{
				var response = (GameResponse)Serializer.ReadObject(jsonStream);
				return response;
			}
		}

		private static DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(GameResponse));
	}
}
