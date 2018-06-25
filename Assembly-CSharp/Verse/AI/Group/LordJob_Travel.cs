using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EA RID: 2538
	public class LordJob_Travel : LordJob
	{
		// Token: 0x04002467 RID: 9319
		private IntVec3 travelDest;

		// Token: 0x0600390B RID: 14603 RVA: 0x001E6534 File Offset: 0x001E4934
		public LordJob_Travel()
		{
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x001E653D File Offset: 0x001E493D
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x001E6550 File Offset: 0x001E4950
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.travelDest);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(false);
			stateGraph.AddToil(lordToil_DefendPoint);
			Transition transition = new Transition(lordToil_Travel, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			transition.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_Travel, false, true);
			transition2.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			transition2.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition2);
			return stateGraph;
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x001E65FC File Offset: 0x001E49FC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}
	}
}
