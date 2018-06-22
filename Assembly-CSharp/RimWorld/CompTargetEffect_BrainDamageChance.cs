using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000751 RID: 1873
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x00161440 File Offset: 0x0015F840
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00161460 File Offset: 0x0015F860
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead)
			{
				if (Rand.Value <= this.PropsBrainDamageChance.brainDamageChance)
				{
					BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
					if (brain != null)
					{
						int num = Rand.RangeInclusive(1, 5);
						Thing thing = pawn;
						DamageDef flame = DamageDefOf.Flame;
						float amount = (float)num;
						thing.TakeDamage(new DamageInfo(flame, amount, -1f, user, brain, this.parent.def, DamageInfo.SourceCategory.ThingOrUnknown, null));
					}
				}
			}
		}
	}
}
