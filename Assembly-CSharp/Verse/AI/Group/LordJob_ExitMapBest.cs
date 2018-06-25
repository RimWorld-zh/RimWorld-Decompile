using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E9 RID: 2537
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x0400246F RID: 9327
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002470 RID: 9328
		private bool canDig = false;

		// Token: 0x06003904 RID: 14596 RVA: 0x001E66B0 File Offset: 0x001E4AB0
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001E66C7 File Offset: 0x001E4AC7
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001E66EC File Offset: 0x001E4AEC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_ExitMap(this.locomotion, this.canDig)
			{
				avoidGridMode = AvoidGridMode.Smart
			});
			return stateGraph;
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x001E6728 File Offset: 0x001E4B28
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}
	}
}
