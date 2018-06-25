using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BE RID: 1214
	public static class ParentRelationUtility
	{
		// Token: 0x060015B5 RID: 5557 RVA: 0x000C14F4 File Offset: 0x000BF8F4
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

		// Token: 0x060015B6 RID: 5558 RVA: 0x000C157C File Offset: 0x000BF97C
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

		// Token: 0x060015B7 RID: 5559 RVA: 0x000C1604 File Offset: 0x000BFA04
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

		// Token: 0x060015B8 RID: 5560 RVA: 0x000C16B0 File Offset: 0x000BFAB0
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

		// Token: 0x060015B9 RID: 5561 RVA: 0x000C175C File Offset: 0x000BFB5C
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
