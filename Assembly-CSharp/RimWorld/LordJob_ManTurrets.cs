using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000171 RID: 369
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x0600079A RID: 1946 RVA: 0x0004AF88 File Offset: 0x00049388
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
