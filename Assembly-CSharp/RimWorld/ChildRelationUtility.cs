using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ChildRelationUtility
	{
		public const float MinFemaleAgeToHaveChildren = 16f;

		public const float MaxFemaleAgeToHaveChildren = 45f;

		public const float UsualFemaleAgeToHaveChildren = 27f;

		public const float MinMaleAgeToHaveChildren = 14f;

		public const float MaxMaleAgeToHaveChildren = 50f;

		public const float UsualMaleAgeToHaveChildren = 30f;

		public const float ChanceForChildToHaveNameOfAnyParent = 0.99f;

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

		public static bool ChildWantsNameOfAnyParent(Pawn child)
		{
			return Rand.ValueSeeded(child.thingIDNumber ^ 88271612) < 0.99f;
		}

		private static int NumberOfChildrenFemaleWantsEver(Pawn female)
		{
			Rand.PushState();
			Rand.Seed = female.thingIDNumber * 3;
			int result = Rand.RangeInclusive(0, 3);
			Rand.PopState();
			return result;
		}

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

		private static float GetAgeFactor(float ageAtBirth, float min, float max, float mid)
		{
			return GenMath.GetFactorInInterval(min, mid, max, 1.6f, ageAtBirth);
		}

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

		public static float GetReflectedSkin(float value, float mirror)
		{
			return Mathf.Clamp01(GenMath.Reflection(value, mirror));
		}

		public static float GetMelaninSimilarityFactor(float melanin1, float melanin2)
		{
			float min = Mathf.Clamp01(melanin1 - 0.15f);
			float max = Mathf.Clamp01(melanin1 + 0.15f);
			return GenMath.GetFactorInInterval(min, melanin1, max, 2.5f, melanin2);
		}

		public static float GetRandomChildSkinColor(float fatherMelanin, float motherMelanin)
		{
			float clampMin = Mathf.Min(fatherMelanin, motherMelanin);
			float clampMax = Mathf.Max(fatherMelanin, motherMelanin);
			float value = (fatherMelanin + motherMelanin) / 2f;
			return PawnSkinColors.GetRandomMelaninSimilarTo(value, clampMin, clampMax);
		}

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
