using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E9 RID: 2537
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x060038FF RID: 14591 RVA: 0x001E5ECE File Offset: 0x001E42CE
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x001E5ED7 File Offset: 0x001E42D7
		public LordJob_DefendPoint(IntVec3 point)
		{
			this.point = point;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x001E5EE8 File Offset: 0x001E42E8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f));
			return stateGraph;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001E5F1C File Offset: 0x001E431C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
		}

		// Token: 0x04002462 RID: 9314
		private IntVec3 point;
	}
}
