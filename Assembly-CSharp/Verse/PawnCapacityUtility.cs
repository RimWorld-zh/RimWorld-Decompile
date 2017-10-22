using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public static class PawnCapacityUtility
	{
		public abstract class CapacityImpactor
		{
			public abstract string Readable(Pawn pawn);
		}

		public class CapacityImpactorBodyPartHealth : CapacityImpactor
		{
			public BodyPartRecord bodyPart;

			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.def.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}
		}

		public class CapacityImpactorCapacity : CapacityImpactor
		{
			public PawnCapacityDef capacity;

			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.LabelCap, ((float)(pawn.health.capacities.GetLevel(this.capacity) * 100.0)).ToString("F0"));
			}
		}

		public class CapacityImpactorHediff : CapacityImpactor
		{
			public Hediff hediff;

			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}
		}

		public class CapacityImpactorPain : CapacityImpactor
		{
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), ((float)(pawn.health.hediffSet.PainTotal * 100.0)).ToString("F0"));
			}
		}

		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		public static float CalculateCapacityLevel(HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors = null)
		{
			if (capacity.zeroIfCannotBeAwake && !diffSet.pawn.health.capacities.CanBeAwake)
			{
				if (impactors != null)
				{
					impactors.Add(new CapacityImpactorCapacity
					{
						capacity = PawnCapacityDefOf.Consciousness
					});
				}
				return 0f;
			}
			float num = capacity.Worker.CalculateCapacityLevel(diffSet, impactors);
			if (num > 0.0 && capacity.minValue <= 0.0)
			{
				float num2 = 99999f;
				float num3 = 1f;
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff hediff = diffSet.hediffs[i];
					List<PawnCapacityModifier> capMods = hediff.CapMods;
					if (capMods != null)
					{
						for (int j = 0; j < capMods.Count; j++)
						{
							PawnCapacityModifier pawnCapacityModifier = capMods[j];
							if (pawnCapacityModifier.capacity == capacity)
							{
								num += pawnCapacityModifier.offset;
								num3 *= pawnCapacityModifier.postFactor;
								if (pawnCapacityModifier.setMax < num2)
								{
									num2 = pawnCapacityModifier.setMax;
								}
								if (impactors != null)
								{
									impactors.Add(new CapacityImpactorHediff
									{
										hediff = hediff
									});
								}
							}
						}
					}
				}
				num *= num3;
				num = Mathf.Min(num, num2);
			}
			num = Mathf.Max(num, capacity.minValue);
			return GenMath.RoundedHundredth(num);
		}

		public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<CapacityImpactor> impactors = null)
		{
			BodyPartRecord rec;
			for (rec = part.parent; rec != null; rec = rec.parent)
			{
				if (diffSet.HasDirectlyAddedPartFor(rec))
				{
					Hediff_AddedPart hediff_AddedPart = (from x in diffSet.GetHediffs<Hediff_AddedPart>()
					where x.Part == rec
					select x).First();
					if (impactors != null)
					{
						impactors.Add(new CapacityImpactorHediff
						{
							hediff = hediff_AddedPart
						});
					}
					return hediff_AddedPart.def.addedPartProps.partEfficiency;
				}
			}
			if (part.parent != null && diffSet.PartIsMissing(part.parent))
			{
				return 0f;
			}
			float num = 1f;
			if (!ignoreAddedParts)
			{
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff_AddedPart hediff_AddedPart2 = diffSet.hediffs[i] as Hediff_AddedPart;
					if (hediff_AddedPart2 != null && hediff_AddedPart2.Part == part)
					{
						num *= hediff_AddedPart2.def.addedPartProps.partEfficiency;
						if (hediff_AddedPart2.def.addedPartProps.partEfficiency != 1.0 && impactors != null)
						{
							impactors.Add(new CapacityImpactorHediff
							{
								hediff = hediff_AddedPart2
							});
						}
					}
				}
			}
			float b = -1f;
			float num2 = 0f;
			bool flag = false;
			for (int j = 0; j < diffSet.hediffs.Count; j++)
			{
				if (diffSet.hediffs[j].Part == part && diffSet.hediffs[j].CurStage != null)
				{
					HediffStage curStage = diffSet.hediffs[j].CurStage;
					num2 += curStage.partEfficiencyOffset;
					flag |= curStage.partIgnoreMissingHP;
					if (curStage.partEfficiencyOffset != 0.0 && curStage.everVisible && impactors != null)
					{
						impactors.Add(new CapacityImpactorHediff
						{
							hediff = diffSet.hediffs[j]
						});
					}
				}
			}
			if (!flag)
			{
				float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
				if (num3 != 1.0 && impactors != null)
				{
					impactors.Add(new CapacityImpactorBodyPartHealth
					{
						bodyPart = part
					});
				}
				num *= num3;
			}
			num += num2;
			if (num > 9.9999997473787516E-05)
			{
				num = Mathf.Max(num, b);
			}
			return Mathf.Max(num, 0f);
		}

		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<CapacityImpactor> impactors = null)
		{
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				return 1f;
			}
			return PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
		}

		public static float CalculateNaturalPartsAverageEfficiency(HediffSet diffSet, BodyPartGroupDef bodyPartGroup)
		{
			float num = 0f;
			int num2 = 0;
			IEnumerable<BodyPartRecord> enumerable = from x in diffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
			where x.groups.Contains(bodyPartGroup)
			select x;
			foreach (BodyPartRecord item in enumerable)
			{
				if (!diffSet.PartOrAnyAncestorHasDirectlyAddedParts(item))
				{
					num += PawnCapacityUtility.CalculatePartEfficiency(diffSet, item, false, null);
				}
				num2++;
			}
			if (num2 != 0 && !(num < 0.0))
			{
				return num / (float)num2;
			}
			return 0f;
		}

		public static float CalculateTagEfficiency(HediffSet diffSet, string tag, float maximum = 3.40282347E+38f, List<CapacityImpactor> impactors = null)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			List<CapacityImpactor> list = null;
			foreach (BodyPartRecord item in body.GetPartsWithTag(tag))
			{
				List<CapacityImpactor> impactors2 = list;
				float num3 = PawnCapacityUtility.CalculatePartEfficiency(diffSet, item, false, impactors2);
				if (impactors != null && num3 != 1.0 && list == null)
				{
					list = (impactors2 = new List<CapacityImpactor>());
					PawnCapacityUtility.CalculatePartEfficiency(diffSet, item, false, impactors2);
				}
				num += num3;
				num2++;
			}
			if (num2 == 0)
			{
				return 1f;
			}
			float num4 = num / (float)num2;
			float num5 = Mathf.Min(num4, maximum);
			if (impactors != null && list != null && (maximum != 1.0 || num4 <= 1.0 || num5 == 1.0))
			{
				impactors.AddRange(list);
			}
			return num5;
		}

		public static float CalculateLimbEfficiency(HediffSet diffSet, string limbCoreTag, string limbSegmentTag, string limbDigitTag, float appendageWeight, out float functionalPercentage, List<CapacityImpactor> impactors)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			foreach (BodyPartRecord item in body.GetPartsWithTag(limbCoreTag))
			{
				float num4 = PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, item, impactors);
				foreach (BodyPartRecord connectedPart in item.GetConnectedParts(limbSegmentTag))
				{
					num4 *= PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, connectedPart, impactors);
				}
				if (item.HasChildParts(limbDigitTag))
				{
					num4 = Mathf.Lerp(num4, num4 * item.GetChildParts(limbDigitTag).Average((Func<BodyPartRecord, float>)((BodyPartRecord digitPart) => PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors))), appendageWeight);
				}
				num += num4;
				num2++;
				if (num4 > 0.0)
				{
					num3++;
				}
			}
			if (num2 == 0)
			{
				functionalPercentage = 0f;
				return 0f;
			}
			functionalPercentage = (float)num3 / (float)num2;
			return num / (float)num2;
		}
	}
}
