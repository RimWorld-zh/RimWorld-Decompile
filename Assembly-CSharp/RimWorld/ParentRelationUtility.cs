using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BC RID: 1212
	public static class ParentRelationUtility
	{
		// Token: 0x060015B1 RID: 5553 RVA: 0x000C13A4 File Offset: 0x000BF7A4
		public static Pawn GetFather(this Pawn pawn)
		{
			Pawn result;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = null;
			}
			else
			{
				List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation directPawnRelation = directRelations[i];
					if (directPawnRelation.def == PawnRelationDefOf.Parent && directPawnRelation.otherPawn.gender != Gender.Female)
					{
						return directPawnRelation.otherPawn;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x000C142C File Offset: 0x000BF82C
		public static Pawn GetMother(this Pawn pawn)
		{
			Pawn result;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = null;
			}
			else
			{
				List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation directPawnRelation = directRelations[i];
					if (directPawnRelation.def == PawnRelationDefOf.Parent && directPawnRelation.otherPawn.gender == Gender.Female)
					{
						return directPawnRelation.otherPawn;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000C14B4 File Offset: 0x000BF8B4
		public static void SetFather(this Pawn pawn, Pawn newFather)
		{
			if (newFather != null && newFather.gender == Gender.Female)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ",
					newFather,
					" with gender ",
					newFather.gender,
					" as ",
					pawn,
					"'s father."
				}), false);
			}
			else
			{
				Pawn father = pawn.GetFather();
				if (father != newFather)
				{
					if (father != null)
					{
						pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, father);
					}
					if (newFather != null)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newFather);
					}
				}
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000C1560 File Offset: 0x000BF960
		public static void SetMother(this Pawn pawn, Pawn newMother)
		{
			if (newMother != null && newMother.gender != Gender.Female)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ",
					newMother,
					" with gender ",
					newMother.gender,
					" as ",
					pawn,
					"'s mother."
				}), false);
			}
			else
			{
				Pawn mother = pawn.GetMother();
				if (mother != newMother)
				{
					if (mother != null)
					{
						pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, mother);
					}
					if (newMother != null)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newMother);
					}
				}
			}
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000C160C File Offset: 0x000BFA0C
		public static float GetRandomSecondParentSkinColor(float otherParentSkin, float childSkin, float? secondChildSkin = null)
		{
			float mirror;
			if (secondChildSkin != null)
			{
				mirror = (childSkin + secondChildSkin.Value) / 2f;
			}
			else
			{
				mirror = childSkin;
			}
			float reflectedSkin = ChildRelationUtility.GetReflectedSkin(otherParentSkin, mirror);
			float num = childSkin;
			float num2 = childSkin;
			if (secondChildSkin != null)
			{
				num = Mathf.Min(num, secondChildSkin.Value);
				num2 = Mathf.Max(num2, secondChildSkin.Value);
			}
			float clampMin = 0f;
			float clampMax = 1f;
			if (reflectedSkin >= num2)
			{
				clampMin = num2;
			}
			else
			{
				clampMax = num;
			}
			return PawnSkinColors.GetRandomMelaninSimilarTo(reflectedSkin, clampMin, clampMax);
		}
	}
}
