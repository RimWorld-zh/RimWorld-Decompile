namespace Verse.AI.Group
{
	public class LordJob_TravelAndExit : LordJob
	{
		private IntVec3 travelDest;

		public LordJob_TravelAndExit()
		{
		}

		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil firstSource = stateGraph.StartingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.travelDest).CreateGraph()).StartingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(firstSource, lordToil_ExitMap);
			transition.triggers.Add(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition);
			return stateGraph;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}
	}
}
