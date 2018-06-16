using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000755 RID: 1877
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x001611D4 File Offset: 0x0015F5D4
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x001611F4 File Offset: 0x0015F5F4
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
