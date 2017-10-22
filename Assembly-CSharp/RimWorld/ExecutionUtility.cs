using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ExecutionUtility
	{
		public static void DoExecutionByCut(Pawn executioner, Pawn victim)
		{
			Map map = victim.Map;
			IntVec3 position = victim.Position;
			int num = Mathf.Max(GenMath.RoundRandom((float)(victim.BodySize * 8.0)), 1);
			for (int num2 = 0; num2 < num; num2++)
			{
				victim.health.DropBloodFilth();
			}
			BodyPartRecord bodyPartRecord = ExecutionUtility.ExecuteCutPart(victim);
			int num3 = Mathf.Clamp((int)victim.health.hediffSet.GetPartHealth(bodyPartRecord) - 1, 1, 20);
			DamageDef executionCut = DamageDefOf.ExecutionCut;
			int amount = num3;
			DamageInfo damageInfo = new DamageInfo(executionCut, amount, -1f, executioner, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown);
			victim.TakeDamage(damageInfo);
			if (!victim.Dead)
			{
				victim.Kill(new DamageInfo?(damageInfo), null);
			}
		}

		private static BodyPartRecord ExecuteCutPart(Pawn pawn)
		{
			BodyPartRecord bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).FirstOrDefault((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.def == BodyPartDefOf.Neck));
			BodyPartRecord result;
			if (bodyPartRecord != null)
			{
				result = bodyPartRecord;
			}
			else
			{
				bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).FirstOrDefault((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.def == BodyPartDefOf.Head));
				if (bodyPartRecord != null)
				{
					result = bodyPartRecord;
				}
				else
				{
					bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).FirstOrDefault((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.def == BodyPartDefOf.InsectHead));
					if (bodyPartRecord != null)
					{
						result = bodyPartRecord;
					}
					else
					{
						bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).FirstOrDefault((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.def == BodyPartDefOf.Body));
						if (bodyPartRecord != null)
						{
							result = bodyPartRecord;
						}
						else
						{
							bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).FirstOrDefault((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.def == BodyPartDefOf.Torso));
							if (bodyPartRecord != null)
							{
								result = bodyPartRecord;
							}
							else
							{
								Log.Error("No good slaughter cut part found for " + pawn);
								result = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).RandomElementByWeight((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.coverageAbsWithChildren));
							}
						}
					}
				}
			}
			return result;
		}
	}
}
