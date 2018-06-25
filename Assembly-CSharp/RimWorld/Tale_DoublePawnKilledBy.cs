using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200066B RID: 1643
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x0600226C RID: 8812 RVA: 0x00124AE0 File Offset: 0x00122EE0
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00124AE9 File Offset: 0x00122EE9
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
