using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EA RID: 2538
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x06003905 RID: 14597 RVA: 0x001E6018 File Offset: 0x001E4418
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001E602F File Offset: 0x001E442F
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x001E6054 File Offset: 0x001E4454
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_ExitMap(this.locomotion, this.canDig)
			{
				avoidGridMode = AvoidGridMode.Smart
			});
			return stateGraph;
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x001E6090 File Offset: 0x001E4490
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}

		// Token: 0x04002463 RID: 9315
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002464 RID: 9316
		private bool canDig = false;
	}
}
