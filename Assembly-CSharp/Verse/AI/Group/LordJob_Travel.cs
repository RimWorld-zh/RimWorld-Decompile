using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EC RID: 2540
	public class LordJob_Travel : LordJob
	{
		// Token: 0x0600390B RID: 14603 RVA: 0x001E60F4 File Offset: 0x001E44F4
		public LordJob_Travel()
		{
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x001E60FD File Offset: 0x001E44FD
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x001E6110 File Offset: 0x001E4510
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

		// Token: 0x0600390E RID: 14606 RVA: 0x001E61BC File Offset: 0x001E45BC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x0400246B RID: 9323
		private IntVec3 travelDest;
	}
}
