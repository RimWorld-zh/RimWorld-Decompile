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
			float result;
			if (pawn.gender == Gender.None)
			{
				result = 100f;
			}
			else
			{
				if (pawn.gender == Gender.Male)
				{
					switch (hair.hairGender)
					{
					case HairGender.Female:
					{
						result = 1f;
						goto IL_0120;
					}
					case HairGender.FemaleUsually:
					{
						result = 5f;
						goto IL_0120;
					}
					case HairGender.MaleUsually:
					{
						result = 30f;
						goto IL_0120;
					}
					case HairGender.Male:
					{
						result = 70f;
						goto IL_0120;
					}
					case HairGender.Any:
					{
						result = 60f;
						goto IL_0120;
					}
					}
				}
				if (pawn.gender == Gender.Female)
				{
					switch (hair.hairGender)
					{
					case HairGender.Female:
					{
						result = 70f;
						goto IL_0120;
					}
					case HairGender.FemaleUsually:
					{
						result = 30f;
						goto IL_0120;
					}
					case HairGender.MaleUsually:
					{
						result = 5f;
						goto IL_0120;
					}
					case HairGender.Male:
					{
						result = 1f;
						goto IL_0120;
					}
					case HairGender.Any:
					{
						result = 60f;
						goto IL_0120;
					}
					}
				}
				Log.Error("Unknown hair likelihood for " + hair + " with " + pawn);
				result = 0f;
			}
			goto IL_0120;
			IL_0120:
			return result;
		}
	}
}
