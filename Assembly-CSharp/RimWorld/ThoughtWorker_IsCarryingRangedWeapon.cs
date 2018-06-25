using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000228 RID: 552
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x000598E4 File Offset: 0x00057CE4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
