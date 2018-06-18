using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000755 RID: 1877
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600298C RID: 10636 RVA: 0x00161268 File Offset: 0x0015F668
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x00161288 File Offset: 0x0015F688
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
