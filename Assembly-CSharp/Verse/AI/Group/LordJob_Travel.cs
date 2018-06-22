using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E8 RID: 2536
	public class LordJob_Travel : LordJob
	{
		// Token: 0x06003907 RID: 14599 RVA: 0x001E6408 File Offset: 0x001E4808
		public LordJob_Travel()
		{
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x001E6411 File Offset: 0x001E4811
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x001E6424 File Offset: 0x001E4824
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

		// Token: 0x0600390A RID: 14602 RVA: 0x001E64D0 File Offset: 0x001E48D0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04002466 RID: 9318
		private IntVec3 travelDest;
	}
}
