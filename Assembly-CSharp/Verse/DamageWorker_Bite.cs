using System;

namespace Verse
{
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		public DamageWorker_Bite()
		{
		}

		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}
	}
}
