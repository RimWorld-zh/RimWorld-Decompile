using System;

namespace Verse
{
	// Token: 0x02000CF4 RID: 3316
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06004914 RID: 18708 RVA: 0x00266988 File Offset: 0x00264D88
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x002669BC File Offset: 0x00264DBC
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage *= this.def.biteDamageMultiplier;
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
