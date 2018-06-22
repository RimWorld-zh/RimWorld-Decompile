using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D3 RID: 2515
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		// Token: 0x06003865 RID: 14437 RVA: 0x001E2558 File Offset: 0x001E0958
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

		// Token: 0x06003866 RID: 14438 RVA: 0x001E2664 File Offset: 0x001E0A64
		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
