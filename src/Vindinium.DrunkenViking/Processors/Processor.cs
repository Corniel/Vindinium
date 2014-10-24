namespace Vindinium.DrunkenViking.Processors
{
	public abstract class Processor
	{
		public void Initialize(Map map, State state)
		{
			this.Map = map;
			this.State = state;
			this.PlayerToMove = state.PlayerToMove;
		}
		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public PlayerType PlayerToMove { get; protected set; }

		public abstract void Process(PotentialPath potentialPath, SafePathCollection collection);


		/// <summary>Returns an empty distance map.</summary>
		protected Distance[,] GetEmptyDistances()
		{
			return Distances.Create(this.Map);
		}
	}
}
