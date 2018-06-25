using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000093 RID: 147
	public static class ExecutionUtility
	{
		// Token: 0x060003AE RID: 942 RVA: 0x00029B60 File Offset: 0x00027F60
		public static void DoExecutionByCut(Pawn executioner, Pawn victim)
		{
			Map map = victim.Map;
			IntVec3 position = victim.Position;
			int num = Mathf.Max(GenMath.RoundRandom(victim.BodySize * 8f), 1);
			for (int i = 0; i < num; i++)
			{
				victim.health.DropBloodFilth();
			}
			BodyPartRecord bodyPartRecord = ExecutionUtility.ExecuteCutPart(victim);
			int num2 = Mathf.Clamp((int)victim.health.hediffSet.GetPartHealth(bodyPartRecord) - 1, 1, 20);
			DamageDef executionCut = DamageDefOf.ExecutionCut;
			float amount = (float)num2;
			DamageInfo damageInfo = new DamageInfo(executionCut, amount, -1f, executioner, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
			victim.TakeDamage(damageInfo);
			if (!victim.Dead)
			{
				victim.Kill(new DamageInfo?(damageInfo), null);
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00029C24 File Offset: 0x00028024
		private static BodyPartRecord ExecuteCutPart(Pawn pawn)
		{
			BodyPartRecord bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Neck);
			BodyPartRecord result;
			if (bodyPartRecord != null)
			{
				result = bodyPartRecord;
			}
			else
			{
				bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Head);
				if (bodyPartRecord != null)
				{
					result = bodyPartRecord;
				}
				else
				{
					bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.InsectHead);
					if (bodyPartRecord != null)
					{
						result = bodyPartRecord;
					}
					else
					{
						bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Body);
						if (bodyPartRecord != null)
						{
							result = bodyPartRecord;
						}
						else
						{
							bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Torso);
							if (bodyPartRecord != null)
							{
								result = bodyPartRecord;
							}
							else
							{
								Log.Error("No good slaughter cut part found for " + pawn, false);
								result = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).RandomElementByWeight((BodyPartRecord x) => x.coverageAbsWithChildren);
							}
						}
					}
				}
			}
			return result;
		}
	}
}
