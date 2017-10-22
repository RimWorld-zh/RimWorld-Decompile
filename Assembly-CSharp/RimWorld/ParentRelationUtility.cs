using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ParentRelationUtility
	{
		public static Pawn GetFather(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Parent, (Predicate<Pawn>)((Pawn x) => x.gender != Gender.Female));
		}

		public static Pawn GetMother(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Parent, (Predicate<Pawn>)((Pawn x) => x.gender == Gender.Female));
		}

		public static void SetFather(this Pawn pawn, Pawn newFather)
		{
			if (newFather != null && newFather.gender == Gender.Female)
			{
				Log.Warning("Tried to set " + newFather + " with gender " + newFather.gender + " as " + pawn + "'s father.");
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

		public static void SetMother(this Pawn pawn, Pawn newMother)
		{
			if (newMother != null && newMother.gender != Gender.Female)
			{
				Log.Warning("Tried to set " + newMother + " with gender " + newMother.gender + " as " + pawn + "'s mother.");
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

		public static float GetRandomSecondParentSkinColor(float otherParentSkin, float childSkin, float? secondChildSkin = default(float?))
		{
			float num = 0f;
			num = (float)((!secondChildSkin.HasValue) ? childSkin : ((childSkin + secondChildSkin.Value) / 2.0));
			float reflectedSkin = ChildRelationUtility.GetReflectedSkin(otherParentSkin, num);
			float num2 = childSkin;
			float num3 = childSkin;
			if (secondChildSkin.HasValue)
			{
				num2 = Mathf.Min(num2, secondChildSkin.Value);
				num3 = Mathf.Max(num3, secondChildSkin.Value);
			}
			float clampMin = 0f;
			float clampMax = 1f;
			if (reflectedSkin >= num3)
			{
				clampMin = num3;
			}
			else
			{
				clampMax = num2;
			}
			return PawnSkinColors.GetRandomMelaninSimilarTo(reflectedSkin, clampMin, clampMax);
		}
	}
}
