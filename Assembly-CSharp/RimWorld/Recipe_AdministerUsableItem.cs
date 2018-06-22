using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000464 RID: 1124
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x060013C9 RID: 5065 RVA: 0x000AC3DD File Offset: 0x000AA7DD
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000AC3F3 File Offset: 0x000AA7F3
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
