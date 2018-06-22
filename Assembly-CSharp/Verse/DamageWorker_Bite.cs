using System;

namespace Verse
{
	// Token: 0x02000CF1 RID: 3313
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06004911 RID: 18705 RVA: 0x002665CC File Offset: 0x002649CC
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x00266600 File Offset: 0x00264A00
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage *= this.def.biteDamageMultiplier;
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
