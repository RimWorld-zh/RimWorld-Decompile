using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200066D RID: 1645
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x0600226F RID: 8815 RVA: 0x00124578 File Offset: 0x00122978
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00124581 File Offset: 0x00122981
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
