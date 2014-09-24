using System;
using System.Net;
using Vindinium.Serialization;

namespace Vindinium.Net
{
	public class Client
	{
		private const string ContentType = "application/x-www-form-urlencoded";

		public Client(ClientParameters parameters)
		{
			this.Parameters = parameters;
		}

		/// <summary>Gets the last response.</summary>
		public GameResponse Response { get; protected set; }

		/// <summary>Returns true if the game is finished, otherwise false.</summary>
		public bool IsFinished { get { return this.Response.game.finished || this.IsCrashed; } }

		/// <summary>Returns true if the game crashed, otherwise false.</summary>
		public bool IsCrashed { get; set; }

		/// <summary>Gets the client parameters.</summary>
		public ClientParameters Parameters { get; protected set; }

		/// <summary>Gets the game URL.</summary>
		public string GameUrl { get { return this.Response.playUrl; } }

		/// <summary>Gets the player.</summary>
		public PlayerType Player { get { return (PlayerType)this.Response.hero.id; } }

		/// <summary>Creates a game.</summary>
		public void CreateGame()
		{
			SendRequest(this.Parameters.Url, this.Parameters.GetCreateGameData());
		}
		
		/// <summary>Plays a move.</summary>
		public void Move(MoveDirection direction)
		{
			SendRequest(this.GameUrl, this.Parameters.GetMoveData(direction));
		}

		private void SendRequest(string uri, string parameters)
		{
			try
			{
				using (var client = new WebClient())
				{
					client.Headers[HttpRequestHeader.ContentType] = Client.ContentType;
					var json = client.UploadString(uri, parameters);
					this.Response = GameResponse.FromJson(json);
				}
			}
			catch (Exception x)
			{
				Console.WriteLine(x);
				this.IsCrashed = true;
			}
		}
	}
}
