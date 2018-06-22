using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000683 RID: 1667
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x06002329 RID: 9001 RVA: 0x0012E8C7 File Offset: 0x0012CCC7
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
