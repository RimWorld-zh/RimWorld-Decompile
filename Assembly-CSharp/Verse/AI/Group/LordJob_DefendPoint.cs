using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E7 RID: 2535
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x0400245E RID: 9310
		private IntVec3 point;

		// Token: 0x060038FF RID: 14591 RVA: 0x001E630E File Offset: 0x001E470E
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x001E6317 File Offset: 0x001E4717
		public LordJob_DefendPoint(IntVec3 point)
		{
			this.point = point;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x001E6328 File Offset: 0x001E4728
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f));
			return stateGraph;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001E635C File Offset: 0x001E475C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
		}
	}
}
