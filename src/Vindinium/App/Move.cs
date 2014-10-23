namespace Vindinium.App
{
	public class Move
	{
		public static readonly Move None = new Move(MoveDirection.x);
		private MoveDirection moveDirection;


		public Move(MoveDirection direction) : this(direction, null) { }
		public Move(MoveDirection direction, string evaluation)
		{
			this.Direction = direction;
			this.Evaluation = evaluation;
		}

		public MoveDirection Direction { get; protected set; }
		public string Evaluation { get; protected set; }

		public string ToLogMove() { return this.Direction.ToString(); }
	}
}
