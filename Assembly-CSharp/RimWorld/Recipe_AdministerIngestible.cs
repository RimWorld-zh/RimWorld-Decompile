using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].Ingested(pawn, 0f);
			if (pawn.IsTeetotaler() && ingredients[0].def.IsNonMedicalDrug)
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
			return pawn.Faction != billDoerFaction && base.recipe.ingredients[0].filter.AllowedThingDefs.First().IsNonMedicalDrug;
		}

		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			string result;
			if (pawn.IsTeetotaler())
			{
				ThingRequest bestThingRequest = base.recipe.ingredients[0].filter.BestThingRequest;
				if (bestThingRequest.singleDef.IsNonMedicalDrug)
				{
					result = base.GetLabelWhenUsedOn(pawn, part) + " (" + "TeetotalerUnhappy".Translate() + ")";
					goto IL_00cf;
				}
			}
			if (pawn.IsProsthophobe())
			{
				ThingRequest bestThingRequest2 = base.recipe.ingredients[0].filter.BestThingRequest;
				if (bestThingRequest2.singleDef == ThingDefOf.Luciferium)
				{
					result = base.GetLabelWhenUsedOn(pawn, part) + " (" + "ProsthophobeUnhappy".Translate() + ")";
					goto IL_00cf;
				}
			}
			result = base.GetLabelWhenUsedOn(pawn, part);
			goto IL_00cf;
			IL_00cf:
			return result;
		}
	}
}
