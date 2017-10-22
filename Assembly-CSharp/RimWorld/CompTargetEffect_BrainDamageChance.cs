using Verse;

namespace RimWorld
{
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		private const float Chance = 0.3f;

		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead && Rand.Value <= 0.30000001192092896)
			{
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain != null)
				{
					int amount = Rand.RangeInclusive(1, 5);
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, amount, -1f, user, brain, base.parent.def, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			}
		}
	}
}
