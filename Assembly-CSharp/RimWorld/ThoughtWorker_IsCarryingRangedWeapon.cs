using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000228 RID: 552
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x000598E0 File Offset: 0x00057CE0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
