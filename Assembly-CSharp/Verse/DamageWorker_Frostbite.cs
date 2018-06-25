using System;

namespace Verse
{
	// Token: 0x02000CF7 RID: 3319
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x0600492A RID: 18730 RVA: 0x0026755F File Offset: 0x0026595F
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
