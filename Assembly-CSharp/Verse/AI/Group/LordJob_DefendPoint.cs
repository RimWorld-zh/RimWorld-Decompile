using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E8 RID: 2536
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x0400246E RID: 9326
		private IntVec3 point;

		// Token: 0x06003900 RID: 14592 RVA: 0x001E663A File Offset: 0x001E4A3A
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x001E6643 File Offset: 0x001E4A43
		public LordJob_DefendPoint(IntVec3 point)
		{
			this.point = point;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001E6654 File Offset: 0x001E4A54
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f));
			return stateGraph;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x001E6688 File Offset: 0x001E4A88
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
		}
	}
}
