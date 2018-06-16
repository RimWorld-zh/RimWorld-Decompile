using System;

namespace Verse
{
	// Token: 0x02000CF9 RID: 3321
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x06004918 RID: 18712 RVA: 0x00266093 File Offset: 0x00264493
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
