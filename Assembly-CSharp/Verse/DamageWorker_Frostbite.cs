using System;

namespace Verse
{
	// Token: 0x02000CF8 RID: 3320
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x0600492A RID: 18730 RVA: 0x0026783F File Offset: 0x00265C3F
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
