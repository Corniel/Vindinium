using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Decisions
{
	public interface IStateEvaluator
	{
		ScoreCollection Evaluate(State state, Map map);
	}
}
