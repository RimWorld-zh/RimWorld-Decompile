using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000685 RID: 1669
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x0600232C RID: 9004 RVA: 0x0012EC7F File Offset: 0x0012D07F
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
