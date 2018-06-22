using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000463 RID: 1123
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		// Token: 0x060013C4 RID: 5060 RVA: 0x000AC1F4 File Offset: 0x000AA5F4
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

		// Token: 0x060013C5 RID: 5061 RVA: 0x000AC2A3 File Offset: 0x000AA6A3
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x000AC2A8 File Offset: 0x000AA6A8
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.ingredients[0].filter.AllowedThingDefs.First<ThingDef>().IsNonMedicalDrug;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x000AC2F8 File Offset: 0x000AA6F8
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
