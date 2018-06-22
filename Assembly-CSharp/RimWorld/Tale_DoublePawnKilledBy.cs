using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000669 RID: 1641
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x06002269 RID: 8809 RVA: 0x00124728 File Offset: 0x00122B28
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00124731 File Offset: 0x00122B31
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
