using Vindinium.Ygritte2.Evaluation;

namespace Vindinium.Ygritte2.Decisions
{
	public class ProcessData
	{
		public ProcessData(NodeLookup lookup, Map map, Evaluator evaluator)
		{
			this.Lookup = lookup;
			this.Map = map;
			this.MoveGenerator = new MoveGenerator();
			this.Evalutor = evaluator;
		}
		public NodeLookup Lookup { get; set; }
		public Map Map { get; set; }
		public MoveGenerator MoveGenerator { get; protected set; }
		public Evaluator Evalutor { get; protected set; }
		
	}
}
