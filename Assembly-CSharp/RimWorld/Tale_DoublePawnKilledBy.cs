using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200066D RID: 1645
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x06002271 RID: 8817 RVA: 0x001245F0 File Offset: 0x001229F0
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x001245F9 File Offset: 0x001229F9
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
