using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000753 RID: 1875
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x001617F0 File Offset: 0x0015FBF0
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00161810 File Offset: 0x0015FC10
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
