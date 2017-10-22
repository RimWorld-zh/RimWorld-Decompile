using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnTechHediffsGenerator
	{
		private static List<Thing> emptyIngredientsList = new List<Thing>();

		public static void GeneratePartsAndImplantsFor(Pawn pawn)
		{
			if (pawn.kindDef.techHediffsTags != null && !(Rand.Value > pawn.kindDef.techHediffsChance))
			{
				float partsMoney = pawn.kindDef.techHediffsMoney.RandomInRange;
				IEnumerable<ThingDef> source = from x in DefDatabase<ThingDef>.AllDefs
				where x.isBodyPartOrImplant && x.BaseMarketValue <= partsMoney && x.techHediffsTags != null && pawn.kindDef.techHediffsTags.Any((Predicate<string>)((string tag) => x.techHediffsTags.Contains(tag)))
				select x;
				if (source.Any())
				{
					ThingDef partDef = source.RandomElementByWeight((Func<ThingDef, float>)((ThingDef w) => w.BaseMarketValue));
					IEnumerable<RecipeDef> source2 = from x in DefDatabase<RecipeDef>.AllDefs
					where x.IsIngredient(partDef) && pawn.def.AllRecipes.Contains(x)
					select x;
					if (source2.Any())
					{
						RecipeDef recipeDef = source2.RandomElement();
						if (recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).Any())
						{
							recipeDef.Worker.ApplyOnPawn(pawn, recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).RandomElement(), null, PawnTechHediffsGenerator.emptyIngredientsList);
						}
					}
				}
			}
		}
	}
}
