using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000685 RID: 1669
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x0600232D RID: 9005 RVA: 0x0012EA17 File Offset: 0x0012CE17
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
