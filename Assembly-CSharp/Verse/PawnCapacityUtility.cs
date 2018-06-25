using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D41 RID: 3393
	public static class PawnCapacityUtility
	{
		// Token: 0x06004AD9 RID: 19161 RVA: 0x00271030 File Offset: 0x0026F430
		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x00271054 File Offset: 0x0026F454
		public static float CalculateCapacityLevel(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float result;
			if (capacity.zeroIfCannotBeAwake && !diffSet.pawn.health.capacities.CanBeAwake)
			{
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
					{
						capacity = PawnCapacityDefOf.Consciousness
					});
				}
				result = 0f;
			}
			else
			{
				float num = capacity.Worker.CalculateCapacityLevel(diffSet, impactors);
				if (num > 0f && capacity.minValue <= 0f)
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
										impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
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
				num = GenMath.RoundedHundredth(num);
				result = num;
			}
			return result;
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x002711D8 File Offset: 0x0026F5D8
		public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartRecord rec;
			for (rec = part.parent; rec != null; rec = rec.parent)
			{
				if (diffSet.HasDirectlyAddedPartFor(rec))
				{
					Hediff_AddedPart hediff_AddedPart = (from x in diffSet.GetHediffs<Hediff_AddedPart>()
					where x.Part == rec
					select x).First<Hediff_AddedPart>();
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
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
						if (hediff_AddedPart2.def.addedPartProps.partEfficiency != 1f && impactors != null)
						{
							impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
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
					if (curStage.partEfficiencyOffset != 0f && curStage.becomeVisible && impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = diffSet.hediffs[j]
						});
					}
				}
			}
			if (!flag)
			{
				float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
				if (num3 != 1f)
				{
					if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(part))
					{
						num3 = Mathf.InverseLerp(0.1f, 1f, num3);
					}
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorBodyPartHealth
						{
							bodyPart = part
						});
					}
					num *= num3;
				}
			}
			num += num2;
			if (num > 0.0001f)
			{
				num = Mathf.Max(num, b);
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x002714C8 File Offset: 0x0026F8C8
		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float result;
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				result = 1f;
			}
			else
			{
				result = PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
			}
			return result;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00271504 File Offset: 0x0026F904
		public static float CalculateNaturalPartsAverageEfficiency(HediffSet diffSet, BodyPartGroupDef bodyPartGroup)
		{
			float num = 0f;
			int num2 = 0;
			IEnumerable<BodyPartRecord> enumerable = from x in diffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
			where x.groups.Contains(bodyPartGroup)
			select x;
			foreach (BodyPartRecord part in enumerable)
			{
				if (!diffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
				{
					num += PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, null);
				}
				num2++;
			}
			float result;
			if (num2 == 0 || num < 0f)
			{
				result = 0f;
			}
			else
			{
				result = num / (float)num2;
			}
			return result;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x002715D4 File Offset: 0x0026F9D4
		public static float CalculateTagEfficiency(HediffSet diffSet, BodyPartTagDef tag, float maximum = 3.40282347E+38f, FloatRange lerp = default(FloatRange), List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			List<PawnCapacityUtility.CapacityImpactor> list = null;
			foreach (BodyPartRecord bodyPartRecord in body.GetPartsWithTag(tag))
			{
				BodyPartRecord part = bodyPartRecord;
				List<PawnCapacityUtility.CapacityImpactor> impactors2 = list;
				float num3 = PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors2);
				if (impactors != null && num3 != 1f && list == null)
				{
					list = new List<PawnCapacityUtility.CapacityImpactor>();
					part = bodyPartRecord;
					impactors2 = list;
					PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors2);
				}
				num += num3;
				num2++;
			}
			float result;
			if (num2 == 0)
			{
				result = 1f;
			}
			else
			{
				float num4 = num / (float)num2;
				float num5 = num4;
				if (lerp != default(FloatRange))
				{
					num5 = lerp.LerpThroughRange(num5);
				}
				num5 = Mathf.Min(num5, maximum);
				if (impactors != null && list != null && (maximum != 1f || num4 <= 1f || num5 == 1f))
				{
					impactors.AddRange(list);
				}
				result = num5;
			}
			return result;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x00271734 File Offset: 0x0026FB34
		public static float CalculateLimbEfficiency(HediffSet diffSet, BodyPartTagDef limbCoreTag, BodyPartTagDef limbSegmentTag, BodyPartTagDef limbDigitTag, float appendageWeight, out float functionalPercentage, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			foreach (BodyPartRecord bodyPartRecord in body.GetPartsWithTag(limbCoreTag))
			{
				float num4 = PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, bodyPartRecord, impactors);
				foreach (BodyPartRecord part in bodyPartRecord.GetConnectedParts(limbSegmentTag))
				{
					num4 *= PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, part, impactors);
				}
				if (bodyPartRecord.HasChildParts(limbDigitTag))
				{
					num4 = Mathf.Lerp(num4, num4 * bodyPartRecord.GetChildParts(limbDigitTag).Average((BodyPartRecord digitPart) => PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors)), appendageWeight);
				}
				num += num4;
				num2++;
				if (num4 > 0f)
				{
					num3++;
				}
			}
			float result;
			if (num2 == 0)
			{
				functionalPercentage = 0f;
				result = 0f;
			}
			else
			{
				functionalPercentage = (float)num3 / (float)num2;
				result = num / (float)num2;
			}
			return result;
		}

		// Token: 0x02000D42 RID: 3394
		public abstract class CapacityImpactor
		{
			// Token: 0x17000BEF RID: 3055
			// (get) Token: 0x06004AE1 RID: 19169 RVA: 0x002718C8 File Offset: 0x0026FCC8
			public virtual bool IsDirect
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06004AE2 RID: 19170
			public abstract string Readable(Pawn pawn);
		}

		// Token: 0x02000D43 RID: 3395
		public class CapacityImpactorBodyPartHealth : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x04003278 RID: 12920
			public BodyPartRecord bodyPart;

			// Token: 0x06004AE4 RID: 19172 RVA: 0x002718E8 File Offset: 0x0026FCE8
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}
		}

		// Token: 0x02000D44 RID: 3396
		public class CapacityImpactorCapacity : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x04003279 RID: 12921
			public PawnCapacityDef capacity;

			// Token: 0x17000BF0 RID: 3056
			// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x0027194C File Offset: 0x0026FD4C
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06004AE7 RID: 19175 RVA: 0x00271964 File Offset: 0x0026FD64
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.LabelCap, (pawn.health.capacities.GetLevel(this.capacity) * 100f).ToString("F0"));
			}
		}

		// Token: 0x02000D45 RID: 3397
		public class CapacityImpactorHediff : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0400327A RID: 12922
			public Hediff hediff;

			// Token: 0x06004AE9 RID: 19177 RVA: 0x002719C0 File Offset: 0x0026FDC0
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}
		}

		// Token: 0x02000D46 RID: 3398
		public class CapacityImpactorPain : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17000BF1 RID: 3057
			// (get) Token: 0x06004AEB RID: 19179 RVA: 0x002719F4 File Offset: 0x0026FDF4
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06004AEC RID: 19180 RVA: 0x00271A0C File Offset: 0x0026FE0C
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), (pawn.health.hediffSet.PainTotal * 100f).ToString("F0"));
			}
		}
	}
}
