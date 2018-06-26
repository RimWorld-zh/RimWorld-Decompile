using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ExecutionUtility
	{
		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache5;

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
			float armorPenetration = 999f;
			DamageInfo damageInfo = new DamageInfo(executionCut, amount, armorPenetration, -1f, executioner, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
			victim.TakeDamage(damageInfo);
			if (!victim.Dead)
			{
				victim.Kill(new DamageInfo?(damageInfo), null);
			}
		}

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

		[CompilerGenerated]
		private static bool <ExecuteCutPart>m__0(BodyPartRecord x)
		{
			return x.def == BodyPartDefOf.Neck;
		}

		[CompilerGenerated]
		private static bool <ExecuteCutPart>m__1(BodyPartRecord x)
		{
			return x.def == BodyPartDefOf.Head;
		}

		[CompilerGenerated]
		private static bool <ExecuteCutPart>m__2(BodyPartRecord x)
		{
			return x.def == BodyPartDefOf.InsectHead;
		}

		[CompilerGenerated]
		private static bool <ExecuteCutPart>m__3(BodyPartRecord x)
		{
			return x.def == BodyPartDefOf.Body;
		}

		[CompilerGenerated]
		private static bool <ExecuteCutPart>m__4(BodyPartRecord x)
		{
			return x.def == BodyPartDefOf.Torso;
		}

		[CompilerGenerated]
		private static float <ExecuteCutPart>m__5(BodyPartRecord x)
		{
			return x.coverageAbsWithChildren;
		}
	}
}
