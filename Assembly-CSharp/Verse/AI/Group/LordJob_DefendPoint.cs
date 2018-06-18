using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E9 RID: 2537
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x06003901 RID: 14593 RVA: 0x001E5FA2 File Offset: 0x001E43A2
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001E5FAB File Offset: 0x001E43AB
		public LordJob_DefendPoint(IntVec3 point)
		{
			this.point = point;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x001E5FBC File Offset: 0x001E43BC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f));
			return stateGraph;
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x001E5FF0 File Offset: 0x001E43F0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
		}

		// Token: 0x04002462 RID: 9314
		private IntVec3 point;
	}
}
