using System;

namespace Verse
{
	// Token: 0x02000CF5 RID: 3317
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x06004927 RID: 18727 RVA: 0x00267483 File Offset: 0x00265883
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
