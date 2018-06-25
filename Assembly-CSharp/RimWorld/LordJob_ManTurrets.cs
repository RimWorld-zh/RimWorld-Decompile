using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000171 RID: 369
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x0004AF70 File Offset: 0x00049370
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
