using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EA RID: 2538
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x06003903 RID: 14595 RVA: 0x001E5F44 File Offset: 0x001E4344
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x001E5F5B File Offset: 0x001E435B
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001E5F80 File Offset: 0x001E4380
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_ExitMap(this.locomotion, this.canDig)
			{
				avoidGridMode = AvoidGridMode.Smart
			});
			return stateGraph;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001E5FBC File Offset: 0x001E43BC
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
