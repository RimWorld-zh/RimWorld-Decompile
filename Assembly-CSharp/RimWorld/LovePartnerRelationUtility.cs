using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BB RID: 1211
	public static class LovePartnerRelationUtility
	{
		// Token: 0x04000CC3 RID: 3267
		private const float MinAgeToGenerateWithLovePartnerRelation = 14f;

		// Token: 0x06001596 RID: 5526 RVA: 0x000C03DC File Offset: 0x000BE7DC
		public static bool HasAnyLovePartner(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null) != null || pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null) != null || pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null) != null;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000C0438 File Offset: 0x000BE838
		public static bool IsLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Spouse;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x000C0470 File Offset: 0x000BE870
		public static bool IsExLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x000C049C File Offset: 0x000BE89C
		public static bool HasAnyLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000C04E8 File Offset: 0x000BE8E8
		public static bool HasAnyExLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x000C0534 File Offset: 0x000BE934
		public static bool HasAnyLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x000C0580 File Offset: 0x000BE980
		public static bool HasAnyExLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x000C05CC File Offset: 0x000BE9CC
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

		// Token: 0x0600159E RID: 5534 RVA: 0x000C0640 File Offset: 0x000BEA40
		public static bool LovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.Lover, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Fiance, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Spouse, second);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x000C0698 File Offset: 0x000BEA98
		public static bool ExLovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, second) || first.relations.DirectRelationExists(PawnRelationDefOf.ExLover, second);
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000C06D8 File Offset: 0x000BEAD8
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

		// Token: 0x060015A1 RID: 5537 RVA: 0x000C0714 File Offset: 0x000BEB14
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

		// Token: 0x060015A2 RID: 5538 RVA: 0x000C07CC File Offset: 0x000BEBCC
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

		// Token: 0x060015A3 RID: 5539 RVA: 0x000C07FC File Offset: 0x000BEBFC
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

		// Token: 0x060015A4 RID: 5540 RVA: 0x000C08CC File Offset: 0x000BECCC
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

		// Token: 0x060015A5 RID: 5541 RVA: 0x000C0A58 File Offset: 0x000BEE58
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

		// Token: 0x060015A6 RID: 5542 RVA: 0x000C0AE9 File Offset: 0x000BEEE9
		public static void TryToShareBed(Pawn first, Pawn second)
		{
			if (!LovePartnerRelationUtility.TryToShareBed_Int(first, second))
			{
				LovePartnerRelationUtility.TryToShareBed_Int(second, first);
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x000C0B08 File Offset: 0x000BEF08
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

		// Token: 0x060015A8 RID: 5544 RVA: 0x000C0B50 File Offset: 0x000BEF50
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

		// Token: 0x060015A9 RID: 5545 RVA: 0x000C0D7C File Offset: 0x000BF17C
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			float value = GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat);
			return Mathf.Clamp(value, 0f, 1f);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x000C0DC8 File Offset: 0x000BF1C8
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

		// Token: 0x060015AB RID: 5547 RVA: 0x000C0E84 File Offset: 0x000BF284
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

		// Token: 0x060015AC RID: 5548 RVA: 0x000C0F48 File Offset: 0x000BF348
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

		// Token: 0x060015AD RID: 5549 RVA: 0x000C1044 File Offset: 0x000BF444
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

		// Token: 0x060015AE RID: 5550 RVA: 0x000C109C File Offset: 0x000BF49C
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

		// Token: 0x060015AF RID: 5551 RVA: 0x000C1164 File Offset: 0x000BF564
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
	}
}
