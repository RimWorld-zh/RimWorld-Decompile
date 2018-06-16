using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000687 RID: 1671
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x0600232F RID: 9007 RVA: 0x0012E707 File Offset: 0x0012CB07
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
