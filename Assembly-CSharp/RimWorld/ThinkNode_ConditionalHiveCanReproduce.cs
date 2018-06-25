using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E7 RID: 487
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x06000988 RID: 2440 RVA: 0x00056BA8 File Offset: 0x00054FA8
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
