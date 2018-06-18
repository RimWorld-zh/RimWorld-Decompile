using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D7 RID: 2519
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		// Token: 0x0600386B RID: 14443 RVA: 0x001E2380 File Offset: 0x001E0780
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			DamageWorker.DamageResult result;
			if (this.tool == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without a tool", 38381735, false);
				result = damageResult;
			}
			else
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn == null)
				{
					Log.ErrorOnce("Attempted to apply melee hediff without pawn target", 78330053, false);
					result = damageResult;
				}
				else
				{
					HediffSet hediffSet = pawn.health.hediffSet;
					BodyPartTagDef bodypartTagTarget = this.verbProps.bodypartTagTarget;
					foreach (BodyPartRecord part in hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, bodypartTagTarget))
					{
						damageResult.AddHediff(pawn.health.AddHediff(this.tool.hediff, part, null, null));
						damageResult.AddPart(pawn, part);
						damageResult.wounded = true;
					}
					result = damageResult;
				}
			}
			return result;
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x001E248C File Offset: 0x001E088C
		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
