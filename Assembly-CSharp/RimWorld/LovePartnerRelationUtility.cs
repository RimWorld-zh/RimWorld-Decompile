using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BF RID: 1215
	public static class LovePartnerRelationUtility
	{
		// Token: 0x0600159F RID: 5535 RVA: 0x000C03F0 File Offset: 0x000BE7F0
		public static bool HasAnyLovePartner(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null) != null || pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null) != null || pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null) != null;
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000C044C File Offset: 0x000BE84C
		public static bool IsLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Spouse;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x000C0484 File Offset: 0x000BE884
		public static bool IsExLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x000C04B0 File Offset: 0x000BE8B0
		public static bool HasAnyLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x000C04FC File Offset: 0x000BE8FC
		public static bool HasAnyExLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x000C0548 File Offset: 0x000BE948
		public static bool HasAnyLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x000C0594 File Offset: 0x000BE994
		public static bool HasAnyExLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x000C05E0 File Offset: 0x000BE9E0
		public static Pawn ExistingLovePartner(Pawn pawn)
		{
			Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
			Pawn result;
			if (firstDirectRelationPawn != null)
			{
				result = firstDirectRelationPawn;
			}
			else
			{
				firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
				if (firstDirectRelationPawn != null)
				{
					result = firstDirectRelationPawn;
				}
				else
				{
					firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
					if (firstDirectRelationPawn != null)
					{
						result = firstDirectRelationPawn;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x000C0654 File Offset: 0x000BEA54
		public static bool LovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.Lover, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Fiance, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Spouse, second);
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x000C06AC File Offset: 0x000BEAAC
		public static bool ExLovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, second) || first.relations.DirectRelationExists(PawnRelationDefOf.ExLover, second);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x000C06EC File Offset: 0x000BEAEC
		public static void GiveRandomExLoverOrExSpouseRelation(Pawn first, Pawn second)
		{
			PawnRelationDef def;
			if (Rand.Value < 0.5f)
			{
				def = PawnRelationDefOf.ExLover;
			}
			else
			{
				def = PawnRelationDefOf.ExSpouse;
			}
			first.relations.AddDirectRelation(def, second);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x000C0728 File Offset: 0x000BEB28
		public static Pawn GetPartnerInMyBed(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			Pawn result;
			if (building_Bed == null)
			{
				result = null;
			}
			else if (building_Bed.SleepingSlotsCount <= 1)
			{
				result = null;
			}
			else if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn))
			{
				result = null;
			}
			else
			{
				foreach (Pawn pawn2 in building_Bed.CurOccupants)
				{
					if (pawn2 != pawn)
					{
						if (LovePartnerRelationUtility.LovePartnerRelationExists(pawn, pawn2))
						{
							return pawn2;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x000C07E0 File Offset: 0x000BEBE0
		public static Pawn ExistingMostLikedLovePartner(Pawn p, bool allowDead)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, allowDead);
			Pawn result;
			if (directPawnRelation != null)
			{
				result = directPawnRelation.otherPawn;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x000C0810 File Offset: 0x000BEC10
		public static DirectPawnRelation ExistingMostLikedLovePartnerRel(Pawn p, bool allowDead)
		{
			DirectPawnRelation result;
			if (!p.RaceProps.IsFlesh)
			{
				result = null;
			}
			else
			{
				DirectPawnRelation directPawnRelation = null;
				int num = int.MinValue;
				List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (allowDead || !directRelations[i].otherPawn.Dead)
					{
						if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def))
						{
							int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
							if (directPawnRelation == null || num2 > num)
							{
								directPawnRelation = directRelations[i];
								num = num2;
							}
						}
					}
				}
				result = directPawnRelation;
			}
			return result;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x000C08E0 File Offset: 0x000BECE0
		public static float GetLovinMtbHours(Pawn pawn, Pawn partner)
		{
			float result;
			if (pawn.Dead || partner.Dead)
			{
				result = -1f;
			}
			else if (DebugSettings.alwaysDoLovin)
			{
				result = 0.1f;
			}
			else if (pawn.needs.food.Starving || partner.needs.food.Starving)
			{
				result = -1f;
			}
			else if (pawn.health.hediffSet.BleedRateTotal > 0f || partner.health.hediffSet.BleedRateTotal > 0f)
			{
				result = -1f;
			}
			else
			{
				float num = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(pawn);
				if (num <= 0f)
				{
					result = -1f;
				}
				else
				{
					float num2 = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(partner);
					if (num2 <= 0f)
					{
						result = -1f;
					}
					else
					{
						float num3 = 12f;
						num3 *= num;
						num3 *= num2;
						num3 /= Mathf.Max(pawn.relations.SecondaryLovinChanceFactor(partner), 0.1f);
						num3 /= Mathf.Max(partner.relations.SecondaryLovinChanceFactor(pawn), 0.1f);
						num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)pawn.relations.OpinionOf(partner));
						num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)partner.relations.OpinionOf(pawn));
						result = num3;
					}
				}
			}
			return result;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x000C0A6C File Offset: 0x000BEE6C
		private static float LovinMtbSinglePawnFactor(Pawn pawn)
		{
			float num = 1f;
			num /= 1f - pawn.health.hediffSet.PainTotal;
			float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
			if (level < 0.5f)
			{
				num /= level * 2f;
			}
			return num / GenMath.FlatHill(0f, 14f, 16f, 25f, 80f, 0.2f, pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x000C0AFD File Offset: 0x000BEEFD
		public static void TryToShareBed(Pawn first, Pawn second)
		{
			if (!LovePartnerRelationUtility.TryToShareBed_Int(first, second))
			{
				LovePartnerRelationUtility.TryToShareBed_Int(second, first);
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x000C0B1C File Offset: 0x000BEF1C
		private static bool TryToShareBed_Int(Pawn bedOwner, Pawn otherPawn)
		{
			Building_Bed ownedBed = bedOwner.ownership.OwnedBed;
			bool result;
			if (ownedBed != null && ownedBed.AnyUnownedSleepingSlot)
			{
				otherPawn.ownership.ClaimBedIfNonMedical(ownedBed);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x000C0B64 File Offset: 0x000BEF64
		public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request, bool ex)
		{
			float result;
			if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				result = 0f;
			}
			else if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				result = 0f;
			}
			else if (generated.gender == other.gender && (!other.story.traits.HasTrait(TraitDefOf.Gay) || !request.AllowGay))
			{
				result = 0f;
			}
			else if (generated.gender != other.gender && other.story.traits.HasTrait(TraitDefOf.Gay))
			{
				result = 0f;
			}
			else
			{
				float num = 1f;
				if (ex)
				{
					int num2 = 0;
					List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
					for (int i = 0; i < directRelations.Count; i++)
					{
						if (LovePartnerRelationUtility.IsExLovePartnerRelation(directRelations[i].def))
						{
							num2++;
						}
					}
					num = Mathf.Pow(0.2f, (float)num2);
				}
				else if (LovePartnerRelationUtility.HasAnyLovePartner(other))
				{
					return 0f;
				}
				float num3 = (generated.gender != other.gender) ? 1f : 0.01f;
				float generationChanceAgeFactor = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(generated);
				float generationChanceAgeFactor2 = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(other);
				float generationChanceAgeGapFactor = LovePartnerRelationUtility.GetGenerationChanceAgeGapFactor(generated, other, ex);
				float num4 = 1f;
				if (generated.GetRelations(other).Any((PawnRelationDef x) => x.familyByBloodRelation))
				{
					num4 = 0.01f;
				}
				float num5;
				if (request.FixedMelanin != null)
				{
					num5 = ChildRelationUtility.GetMelaninSimilarityFactor(request.FixedMelanin.Value, other.story.melanin);
				}
				else
				{
					num5 = PawnSkinColors.GetMelaninCommonalityFactor(other.story.melanin);
				}
				result = num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3 * num5 * num4;
			}
			return result;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x000C0D90 File Offset: 0x000BF190
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			float value = GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat);
			return Mathf.Clamp(value, 0f, 1f);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000C0DDC File Offset: 0x000BF1DC
		private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
		{
			float num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
			if (ex)
			{
				float num2 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
				if (num2 >= 0f)
				{
					num = Mathf.Min(num, num2);
				}
				float num3 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
				if (num3 >= 0f)
				{
					num = Mathf.Min(num, num3);
				}
			}
			float result;
			if (num > 40f)
			{
				result = 0f;
			}
			else
			{
				float num4 = GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num);
				num4 = Mathf.Clamp(num4, 0.001f, 1f);
				result = num4;
			}
			return result;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000C0E98 File Offset: 0x000BF298
		private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
		{
			float num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
			float result;
			if (num < 0f)
			{
				Log.Warning("at < 0", false);
				result = 0f;
			}
			else
			{
				float num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, p2.ageTracker.AgeChronologicalYearsFloat, num);
				float num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
				if (num2 < 0f)
				{
					result = -1f;
				}
				else if (num2 < 14f)
				{
					result = -1f;
				}
				else if (num3 <= 14f)
				{
					result = 0f;
				}
				else
				{
					result = num3 - 14f;
				}
			}
			return result;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000C0F5C File Offset: 0x000BF35C
		public static void TryToShareChildrenForGeneratedLovePartner(Pawn generated, Pawn other, PawnGenerationRequest request, float extraChanceFactor)
		{
			if (generated.gender != other.gender)
			{
				List<Pawn> list = other.relations.Children.ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					float num = 1f;
					if (generated.gender == Gender.Male)
					{
						num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, generated, other, null, new PawnGenerationRequest?(request), null);
					}
					else if (generated.gender == Gender.Female)
					{
						num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, other, generated, null, null, new PawnGenerationRequest?(request));
					}
					num *= extraChanceFactor;
					if (Rand.Value < num)
					{
						if (generated.gender == Gender.Male)
						{
							pawn.SetFather(generated);
						}
						else if (generated.gender == Gender.Female)
						{
							pawn.SetMother(generated);
						}
					}
				}
			}
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000C1058 File Offset: 0x000BF458
		public static void ChangeSpouseRelationsToExSpouse(Pawn pawn)
		{
			for (;;)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn == null)
				{
					break;
				}
				pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, firstDirectRelationPawn);
				pawn.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, firstDirectRelationPawn);
			}
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x000C10B0 File Offset: 0x000BF4B0
		public static Pawn GetMostDislikedNonPartnerBedOwner(Pawn p)
		{
			Building_Bed ownedBed = p.ownership.OwnedBed;
			Pawn result;
			if (ownedBed == null)
			{
				result = null;
			}
			else
			{
				Pawn pawn = null;
				int num = 0;
				for (int i = 0; i < ownedBed.owners.Count; i++)
				{
					if (ownedBed.owners[i] != p)
					{
						if (!LovePartnerRelationUtility.LovePartnerRelationExists(p, ownedBed.owners[i]))
						{
							int num2 = p.relations.OpinionOf(ownedBed.owners[i]);
							if (pawn == null || num2 < num)
							{
								pawn = ownedBed.owners[i];
								num = num2;
							}
						}
					}
				}
				result = pawn;
			}
			return result;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000C1178 File Offset: 0x000BF578
		public static float IncestOpinionOffsetFor(Pawn other, Pawn pawn)
		{
			float num = 0f;
			List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def))
				{
					if (directRelations[i].otherPawn != pawn)
					{
						if (!directRelations[i].otherPawn.Dead)
						{
							foreach (PawnRelationDef pawnRelationDef in other.GetRelations(directRelations[i].otherPawn))
							{
								float incestOpinionOffset = pawnRelationDef.incestOpinionOffset;
								if (incestOpinionOffset < num)
								{
									num = incestOpinionOffset;
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x04000CC6 RID: 3270
		private const float MinAgeToGenerateWithLovePartnerRelation = 14f;
	}
}
