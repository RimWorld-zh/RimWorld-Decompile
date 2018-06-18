using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000687 RID: 1671
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x06002331 RID: 9009 RVA: 0x0012E77F File Offset: 0x0012CB7F
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
