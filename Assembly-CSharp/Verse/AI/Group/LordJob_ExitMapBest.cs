using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E6 RID: 2534
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x060038FF RID: 14591 RVA: 0x001E6258 File Offset: 0x001E4658
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x001E626F File Offset: 0x001E466F
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x001E6294 File Offset: 0x001E4694
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_ExitMap(this.locomotion, this.canDig)
			{
				avoidGridMode = AvoidGridMode.Smart
			});
			return stateGraph;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001E62D0 File Offset: 0x001E46D0
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}

		// Token: 0x0400245E RID: 9310
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x0400245F RID: 9311
		private bool canDig = false;
	}
}
