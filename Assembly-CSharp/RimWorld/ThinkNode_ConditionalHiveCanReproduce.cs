using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E7 RID: 487
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x0600098B RID: 2443 RVA: 0x00056B98 File Offset: 0x00054F98
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
