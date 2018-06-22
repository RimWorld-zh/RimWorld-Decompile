using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000488 RID: 1160
	public static class PawnHairChooser
	{
		// Token: 0x06001480 RID: 5248 RVA: 0x000B40C0 File Offset: 0x000B24C0
		public static HairDef RandomHairDefFor(Pawn pawn, FactionDef factionType)
		{
			IEnumerable<HairDef> source = from hair in DefDatabase<HairDef>.AllDefs
			where hair.hairTags.SharesElementWith(factionType.hairTags)
			select hair;
			return source.RandomElementByWeight((HairDef hair) => PawnHairChooser.HairChoiceLikelihoodFor(hair, pawn));
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x000B4114 File Offset: 0x000B2514
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
					case HairGender.Male:
						return 70f;
					case HairGender.MaleUsually:
						return 30f;
					case HairGender.Any:
						return 60f;
					case HairGender.FemaleUsually:
						return 5f;
					case HairGender.Female:
						return 1f;
					}
				}
				if (pawn.gender == Gender.Female)
				{
					switch (hair.hairGender)
					{
					case HairGender.Male:
						return 1f;
					case HairGender.MaleUsually:
						return 5f;
					case HairGender.Any:
						return 60f;
					case HairGender.FemaleUsually:
						return 30f;
					case HairGender.Female:
						return 70f;
					}
				}
				Log.Error(string.Concat(new object[]
				{
					"Unknown hair likelihood for ",
					hair,
					" with ",
					pawn
				}), false);
				result = 0f;
			}
			return result;
		}
	}
}
