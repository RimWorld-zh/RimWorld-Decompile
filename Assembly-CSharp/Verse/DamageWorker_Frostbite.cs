using System;

namespace Verse
{
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		public DamageWorker_Frostbite()
		{
		}

		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
