using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B0 RID: 1200
	public static class ChildRelationUtility
	{
		// Token: 0x04000CA4 RID: 3236
		public const float MinFemaleAgeToHaveChildren = 16f;

		// Token: 0x04000CA5 RID: 3237
		public const float MaxFemaleAgeToHaveChildren = 45f;

		// Token: 0x04000CA6 RID: 3238
		public const float UsualFemaleAgeToHaveChildren = 27f;

		// Token: 0x04000CA7 RID: 3239
		public const float MinMaleAgeToHaveChildren = 14f;

		// Token: 0x04000CA8 RID: 3240
		public const float MaxMaleAgeToHaveChildren = 50f;

		// Token: 0x04000CA9 RID: 3241
		public const float UsualMaleAgeToHaveChildren = 30f;

		// Token: 0x04000CAA RID: 3242
		public const float ChanceForChildToHaveNameOfAnyParent = 0.99f;

		// Token: 0x06001564 RID: 5476 RVA: 0x000BE200 File Offset: 0x000BC600
		public static float ChanceOfBecomingChildOf(Pawn child, Pawn father, Pawn mother, PawnGenerationRequest? childGenerationRequest, PawnGenerationRequest? fatherGenerationRequest, PawnGenerationRequest? motherGenerationRequest)
		{
			float result;
			if (father != null && father.gender != Gender.Male)
			{
				Log.Warning("Tried to calculate chance for father with gender \"" + father.gender + "\".", false);
				result = 0f;
			}
			else if (mother != null && mother.gender != Gender.Female)
			{
				Log.Warning("Tried to calculate chance for mother with gender \"" + mother.gender + "\".", false);
				result = 0f;
			}
			else if (father != null && child.GetFather() != null && child.GetFather() != father)
			{
				result = 0f;
			}
			else if (mother != null && child.GetMother() != null && child.GetMother() != mother)
			{
				result = 0f;
			}
			else if (mother != null && father != null && !LovePartnerRelationUtility.LovePartnerRelationExists(mother, father) && !LovePartnerRelationUtility.ExLovePartnerRelationExists(mother, father))
			{
				result = 0f;
			}
			else
			{
				float? melanin = ChildRelationUtility.GetMelanin(child, childGenerationRequest);
				float? melanin2 = ChildRelationUtility.GetMelanin(father, fatherGenerationRequest);
				float? melanin3 = ChildRelationUtility.GetMelanin(mother, motherGenerationRequest);
				bool fatherIsNew = father != null && child.GetFather() != father;
				bool motherIsNew = mother != null && child.GetMother() != mother;
				float skinColorFactor = ChildRelationUtility.GetSkinColorFactor(melanin, melanin2, melanin3, fatherIsNew, motherIsNew);
				if (skinColorFactor <= 0f)
				{
					result = 0f;
				}
				else
				{
					float num = 1f;
					float num2 = 1f;
					float num3 = 1f;
					float num4 = 1f;
					if (father != null && child.GetFather() == null)
					{
						num = ChildRelationUtility.GetParentAgeFactor(father, child, 14f, 30f, 50f);
						if (num == 0f)
						{
							return 0f;
						}
						if (father.story.traits.HasTrait(TraitDefOf.Gay))
						{
							num4 = 0.1f;
						}
					}
					if (mother != null && child.GetMother() == null)
					{
						num2 = ChildRelationUtility.GetParentAgeFactor(mother, child, 16f, 27f, 45f);
						if (num2 == 0f)
						{
							return 0f;
						}
						int num5 = ChildRelationUtility.NumberOfChildrenFemaleWantsEver(mother);
						if (mother.relations.ChildrenCount >= num5)
						{
							return 0f;
						}
						num3 = 1f - (float)mother.relations.ChildrenCount / (float)num5;
						if (mother.story.traits.HasTrait(TraitDefOf.Gay))
						{
							num4 = 0.1f;
						}
					}
					float num6 = 1f;
					if (mother != null)
					{
						Pawn firstDirectRelationPawn = mother.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
						if (firstDirectRelationPawn != null && firstDirectRelationPawn != father)
						{
							num6 *= 0.15f;
						}
					}
					if (father != null)
					{
						Pawn firstDirectRelationPawn2 = father.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
						if (firstDirectRelationPawn2 != null && firstDirectRelationPawn2 != mother)
						{
							num6 *= 0.15f;
						}
					}
					result = skinColorFactor * num * num2 * num3 * num6 * num4;
				}
			}
			return result;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x000BE520 File Offset: 0x000BC920
		private static float GetParentAgeFactor(Pawn parent, Pawn child, float minAgeToHaveChildren, float usualAgeToHaveChildren, float maxAgeToHaveChildren)
		{
			float num = PawnRelationUtility.MaxPossibleBioAgeAt(parent.ageTracker.AgeBiologicalYearsFloat, parent.ageTracker.AgeChronologicalYearsFloat, child.ageTracker.AgeChronologicalYearsFloat);
			float num2 = PawnRelationUtility.MinPossibleBioAgeAt(parent.ageTracker.AgeBiologicalYearsFloat, child.ageTracker.AgeChronologicalYearsFloat);
			float result;
			if (num <= 0f)
			{
				result = 0f;
			}
			else if (num2 > num)
			{
				if (num2 > num + 0.1f)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Min possible bio age (",
						num2,
						") is greater than max possible bio age (",
						num,
						")."
					}), false);
				}
				result = 0f;
			}
			else if (num2 <= usualAgeToHaveChildren && num >= usualAgeToHaveChildren)
			{
				result = 1f;
			}
			else
			{
				float ageFactor = ChildRelationUtility.GetAgeFactor(num2, minAgeToHaveChildren, maxAgeToHaveChildren, usualAgeToHaveChildren);
				float ageFactor2 = ChildRelationUtility.GetAgeFactor(num, minAgeToHaveChildren, maxAgeToHaveChildren, usualAgeToHaveChildren);
				result = Mathf.Max(ageFactor, ageFactor2);
			}
			return result;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x000BE624 File Offset: 0x000BCA24
		public static bool ChildWantsNameOfAnyParent(Pawn child)
		{
			return Rand.ValueSeeded(child.thingIDNumber ^ 88271612) < 0.99f;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x000BE654 File Offset: 0x000BCA54
		private static int NumberOfChildrenFemaleWantsEver(Pawn female)
		{
			Rand.PushState();
			Rand.Seed = female.thingIDNumber * 3;
			int result = Rand.RangeInclusive(0, 3);
			Rand.PopState();
			return result;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000BE68C File Offset: 0x000BCA8C
		private static float? GetMelanin(Pawn pawn, PawnGenerationRequest? request)
		{
			float? result;
			if (request != null)
			{
				result = request.Value.FixedMelanin;
			}
			else if (pawn != null)
			{
				result = new float?(pawn.story.melanin);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x000BE6E8 File Offset: 0x000BCAE8
		private static float GetAgeFactor(float ageAtBirth, float min, float max, float mid)
		{
			return GenMath.GetFactorInInterval(min, mid, max, 1.6f, ageAtBirth);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x000BE70C File Offset: 0x000BCB0C
		private static float GetSkinColorFactor(float? childMelanin, float? fatherMelanin, float? motherMelanin, bool fatherIsNew, bool motherIsNew)
		{
			if (childMelanin != null && fatherMelanin != null && motherMelanin != null)
			{
				float num = Mathf.Min(fatherMelanin.Value, motherMelanin.Value);
				float num2 = Mathf.Max(fatherMelanin.Value, motherMelanin.Value);
				if (childMelanin < num - 0.05f)
				{
					return 0f;
				}
				if (childMelanin > num2 + 0.05f)
				{
					return 0f;
				}
			}
			float num3 = 1f;
			if (fatherIsNew)
			{
				num3 *= ChildRelationUtility.GetNewParentSkinColorFactor(fatherMelanin, motherMelanin, childMelanin);
			}
			if (motherIsNew)
			{
				num3 *= ChildRelationUtility.GetNewParentSkinColorFactor(motherMelanin, fatherMelanin, childMelanin);
			}
			return num3;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000BE7F8 File Offset: 0x000BCBF8
		private static float GetNewParentSkinColorFactor(float? newParentMelanin, float? otherParentMelanin, float? childMelanin)
		{
			float result;
			if (newParentMelanin != null)
			{
				if (otherParentMelanin == null)
				{
					if (childMelanin != null)
					{
						result = ChildRelationUtility.GetMelaninSimilarityFactor(newParentMelanin.Value, childMelanin.Value);
					}
					else
					{
						result = PawnSkinColors.GetMelaninCommonalityFactor(newParentMelanin.Value);
					}
				}
				else if (childMelanin != null)
				{
					float reflectedSkin = ChildRelationUtility.GetReflectedSkin(otherParentMelanin.Value, childMelanin.Value);
					result = ChildRelationUtility.GetMelaninSimilarityFactor(newParentMelanin.Value, reflectedSkin);
				}
				else
				{
					float melanin = (newParentMelanin.Value + otherParentMelanin.Value) / 2f;
					result = PawnSkinColors.GetMelaninCommonalityFactor(melanin);
				}
			}
			else if (otherParentMelanin == null)
			{
				if (childMelanin != null)
				{
					result = PawnSkinColors.GetMelaninCommonalityFactor(childMelanin.Value);
				}
				else
				{
					result = 1f;
				}
			}
			else if (childMelanin != null)
			{
				float reflectedSkin2 = ChildRelationUtility.GetReflectedSkin(otherParentMelanin.Value, childMelanin.Value);
				result = PawnSkinColors.GetMelaninCommonalityFactor(reflectedSkin2);
			}
			else
			{
				result = PawnSkinColors.GetMelaninCommonalityFactor(otherParentMelanin.Value);
			}
			return result;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000BE930 File Offset: 0x000BCD30
		public static float GetReflectedSkin(float value, float mirror)
		{
			return Mathf.Clamp01(GenMath.Reflection(value, mirror));
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000BE954 File Offset: 0x000BCD54
		public static float GetMelaninSimilarityFactor(float melanin1, float melanin2)
		{
			float min = Mathf.Clamp01(melanin1 - 0.15f);
			float max = Mathf.Clamp01(melanin1 + 0.15f);
			return GenMath.GetFactorInInterval(min, melanin1, max, 2.5f, melanin2);
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x000BE994 File Offset: 0x000BCD94
		public static float GetRandomChildSkinColor(float fatherMelanin, float motherMelanin)
		{
			float clampMin = Mathf.Min(fatherMelanin, motherMelanin);
			float clampMax = Mathf.Max(fatherMelanin, motherMelanin);
			float value = (fatherMelanin + motherMelanin) / 2f;
			return PawnSkinColors.GetRandomMelaninSimilarTo(value, clampMin, clampMax);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000BE9CC File Offset: 0x000BCDCC
		public static bool DefinitelyHasNotBirthName(Pawn pawn)
		{
			Pawn spouse = pawn.GetSpouse();
			bool result;
			if (spouse == null)
			{
				result = false;
			}
			else
			{
				string last = ((NameTriple)spouse.Name).Last;
				result = (!(((NameTriple)pawn.Name).Last != last) && ((spouse.GetMother() != null && ((NameTriple)spouse.GetMother().Name).Last == last) || (spouse.GetFather() != null && ((NameTriple)spouse.GetFather().Name).Last == last)));
			}
			return result;
		}
	}
}
