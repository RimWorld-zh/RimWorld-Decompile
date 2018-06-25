using System;
using Verse;

namespace RimWorld
{
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		public Verb_MeleeApplyHediff()
		{
		}

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

		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
