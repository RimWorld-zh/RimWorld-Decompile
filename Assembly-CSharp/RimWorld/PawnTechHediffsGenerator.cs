using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200048A RID: 1162
	public static class PawnTechHediffsGenerator
	{
		// Token: 0x06001486 RID: 5254 RVA: 0x000B47D0 File Offset: 0x000B2BD0
		public static void GenerateTechHediffsFor(Pawn pawn)
		{
			if (pawn.kindDef.techHediffsTags != null)
			{
				if (Rand.Value <= pawn.kindDef.techHediffsChance)
				{
					float partsMoney = pawn.kindDef.techHediffsMoney.RandomInRange;
					IEnumerable<ThingDef> source = from x in DefDatabase<ThingDef>.AllDefs
					where x.isTechHediff && x.BaseMarketValue <= partsMoney && x.techHediffsTags != null && pawn.kindDef.techHediffsTags.Any((string tag) => x.techHediffsTags.Contains(tag))
					select x;
					if (source.Any<ThingDef>())
					{
						ThingDef partDef = source.RandomElementByWeight((ThingDef w) => w.BaseMarketValue);
						IEnumerable<RecipeDef> source2 = from x in DefDatabase<RecipeDef>.AllDefs
						where x.IsIngredient(partDef) && pawn.def.AllRecipes.Contains(x)
						select x;
						if (source2.Any<RecipeDef>())
						{
							RecipeDef recipeDef = source2.RandomElement<RecipeDef>();
							if (recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).Any<BodyPartRecord>())
							{
								recipeDef.Worker.ApplyOnPawn(pawn, recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).RandomElement<BodyPartRecord>(), null, PawnTechHediffsGenerator.emptyIngredientsList, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000C53 RID: 3155
		private static List<Thing> emptyIngredientsList = new List<Thing>();
	}
}
