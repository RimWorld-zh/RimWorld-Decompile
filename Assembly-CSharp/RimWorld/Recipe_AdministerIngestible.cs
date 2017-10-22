using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
			ingredients[0].Ingested(pawn, 0f);
			if (pawn.IsTeetotaler() && ingredients[0].def.IsDrug)
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeDrugs, billDoer);
			}
			else if (pawn.IsProsthophobe() && ingredients[0].def == ThingDefOf.Luciferium)
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeLuciferium, billDoer);
			}
		}

		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if (pawn.Faction == billDoerFaction)
			{
				return false;
			}
			return base.recipe.ingredients[0].filter.AllowedThingDefs.First().ingestible.drugCategory != DrugCategory.Medical;
		}

		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.IsTeetotaler())
			{
				ThingRequest bestThingRequest = base.recipe.ingredients[0].filter.BestThingRequest;
				if (bestThingRequest.singleDef.IsDrug)
				{
					return base.GetLabelWhenUsedOn(pawn, part) + " (" + "TeetotalerUnhappy".Translate() + ")";
				}
			}
			if (pawn.IsProsthophobe())
			{
				ThingRequest bestThingRequest2 = base.recipe.ingredients[0].filter.BestThingRequest;
				if (bestThingRequest2.singleDef == ThingDefOf.Luciferium)
				{
					return base.GetLabelWhenUsedOn(pawn, part) + " (" + "ProsthophobeUnhappy".Translate() + ")";
				}
			}
			return base.GetLabelWhenUsedOn(pawn, part);
		}
	}
}
