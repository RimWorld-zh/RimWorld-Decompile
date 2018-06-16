using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000226 RID: 550
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x00059750 File Offset: 0x00057B50
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
