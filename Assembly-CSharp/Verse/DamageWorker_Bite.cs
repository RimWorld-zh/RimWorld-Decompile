using System;

namespace Verse
{
	// Token: 0x02000CF3 RID: 3315
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06004914 RID: 18708 RVA: 0x002666A8 File Offset: 0x00264AA8
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x002666DC File Offset: 0x00264ADC
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage *= this.def.biteDamageMultiplier;
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
