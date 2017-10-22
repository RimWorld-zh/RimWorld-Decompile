using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_ManTurrets : LordJob
	{
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.StartingToil = new LordToil_ManClosestTurrets();
			return stateGraph;
		}
	}
}
