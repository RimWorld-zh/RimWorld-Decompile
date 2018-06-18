using System;

namespace Verse
{
	// Token: 0x02000CF4 RID: 3316
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06004900 RID: 18688 RVA: 0x002651B4 File Offset: 0x002635B4
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x002651E8 File Offset: 0x002635E8
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage *= this.def.biteDamageMultiplier;
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
