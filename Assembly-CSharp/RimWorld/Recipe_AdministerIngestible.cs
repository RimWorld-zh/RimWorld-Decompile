using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000465 RID: 1125
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		// Token: 0x060013C8 RID: 5064 RVA: 0x000AC344 File Offset: 0x000AA744
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

		// Token: 0x060013C9 RID: 5065 RVA: 0x000AC3F3 File Offset: 0x000AA7F3
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000AC3F8 File Offset: 0x000AA7F8
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.ingredients[0].filter.AllowedThingDefs.First<ThingDef>().IsNonMedicalDrug;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000AC448 File Offset: 0x000AA848
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			string result;
			if (pawn.IsTeetotaler() && this.recipe.ingredients[0].filter.BestThingRequest.singleDef.IsNonMedicalDrug)
			{
				result = base.GetLabelWhenUsedOn(pawn, part) + " (" + "TeetotalerUnhappy".Translate() + ")";
			}
			else if (pawn.IsProsthophobe() && this.recipe.ingredients[0].filter.BestThingRequest.singleDef == ThingDefOf.Luciferium)
			{
				result = base.GetLabelWhenUsedOn(pawn, part) + " (" + "ProsthophobeUnhappy".Translate() + ")";
			}
			else
			{
				result = base.GetLabelWhenUsedOn(pawn, part);
			}
			return result;
		}
	}
}
