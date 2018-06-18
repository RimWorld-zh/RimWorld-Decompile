using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C0 RID: 1216
	public static class ParentRelationUtility
	{
		// Token: 0x060015BA RID: 5562 RVA: 0x000C13B8 File Offset: 0x000BF7B8
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

		// Token: 0x060015BB RID: 5563 RVA: 0x000C1440 File Offset: 0x000BF840
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

		// Token: 0x060015BC RID: 5564 RVA: 0x000C14C8 File Offset: 0x000BF8C8
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

		// Token: 0x060015BD RID: 5565 RVA: 0x000C1574 File Offset: 0x000BF974
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

		// Token: 0x060015BE RID: 5566 RVA: 0x000C1620 File Offset: 0x000BFA20
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
