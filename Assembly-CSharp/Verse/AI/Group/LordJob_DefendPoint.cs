using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E5 RID: 2533
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x060038FB RID: 14587 RVA: 0x001E61E2 File Offset: 0x001E45E2
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x001E61EB File Offset: 0x001E45EB
		public LordJob_DefendPoint(IntVec3 point)
		{
			this.point = point;
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x001E61FC File Offset: 0x001E45FC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f));
			return stateGraph;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x001E6230 File Offset: 0x001E4630
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
		}

		// Token: 0x0400245D RID: 9309
		private IntVec3 point;
	}
}
