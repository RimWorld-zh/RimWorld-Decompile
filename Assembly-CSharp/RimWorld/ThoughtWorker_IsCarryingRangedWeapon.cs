using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000226 RID: 550
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x00059794 File Offset: 0x00057B94
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
