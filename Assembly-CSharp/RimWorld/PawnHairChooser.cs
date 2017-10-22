using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnHairChooser
	{
		public static HairDef RandomHairDefFor(Pawn pawn, FactionDef factionType)
		{
			IEnumerable<HairDef> source = from hair in DefDatabase<HairDef>.AllDefs
			where hair.hairTags.SharesElementWith(factionType.hairTags)
			select hair;
			return source.RandomElementByWeight((Func<HairDef, float>)((HairDef hair) => PawnHairChooser.HairChoiceLikelihoodFor(hair, pawn)));
		}

		private static float HairChoiceLikelihoodFor(HairDef hair, Pawn pawn)
		{
			if (pawn.gender == Gender.None)
			{
				return 100f;
			}
			if (pawn.gender == Gender.Male)
			{
				switch (hair.hairGender)
				{
				case HairGender.Female:
				{
					return 1f;
				}
				case HairGender.FemaleUsually:
				{
					return 5f;
				}
				case HairGender.MaleUsually:
				{
					return 30f;
				}
				case HairGender.Male:
				{
					return 70f;
				}
				case HairGender.Any:
				{
					return 60f;
				}
				}
			}
			if (pawn.gender == Gender.Female)
			{
				switch (hair.hairGender)
				{
				case HairGender.Female:
				{
					return 70f;
				}
				case HairGender.FemaleUsually:
				{
					return 30f;
				}
				case HairGender.MaleUsually:
				{
					return 5f;
				}
				case HairGender.Male:
				{
					return 1f;
				}
				case HairGender.Any:
				{
					return 60f;
				}
				}
			}
			Log.Error("Unknown hair likelihood for " + hair + " with " + pawn);
			return 0f;
		}
	}
}
