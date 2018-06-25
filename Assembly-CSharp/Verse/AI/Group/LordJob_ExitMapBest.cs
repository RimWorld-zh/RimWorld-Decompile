using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E8 RID: 2536
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x0400245F RID: 9311
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002460 RID: 9312
		private bool canDig = false;

		// Token: 0x06003903 RID: 14595 RVA: 0x001E6384 File Offset: 0x001E4784
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x001E639B File Offset: 0x001E479B
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001E63C0 File Offset: 0x001E47C0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_ExitMap(this.locomotion, this.canDig)
			{
				avoidGridMode = AvoidGridMode.Smart
			});
			return stateGraph;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001E63FC File Offset: 0x001E47FC
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}
	}
}
