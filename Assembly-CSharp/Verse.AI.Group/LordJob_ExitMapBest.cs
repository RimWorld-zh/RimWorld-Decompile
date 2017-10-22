namespace Verse.AI.Group
{
	public class LordJob_ExitMapBest : LordJob
	{
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		private bool canDig;

		public LordJob_ExitMapBest()
		{
		}

		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(this.locomotion, this.canDig);
			lordToil_ExitMap.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_ExitMap);
			return stateGraph;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}
	}
}
