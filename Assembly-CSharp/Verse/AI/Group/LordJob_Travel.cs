using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EC RID: 2540
	public class LordJob_Travel : LordJob
	{
		// Token: 0x0600390D RID: 14605 RVA: 0x001E61C8 File Offset: 0x001E45C8
		public LordJob_Travel()
		{
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x001E61D1 File Offset: 0x001E45D1
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x001E61E4 File Offset: 0x001E45E4
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

		// Token: 0x06003910 RID: 14608 RVA: 0x001E6290 File Offset: 0x001E4690
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x0400246B RID: 9323
		private IntVec3 travelDest;
	}
}
