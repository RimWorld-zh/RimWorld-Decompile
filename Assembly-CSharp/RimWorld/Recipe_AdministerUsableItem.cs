using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000468 RID: 1128
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x000AC3CD File Offset: 0x000AA7CD
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000AC3E3 File Offset: 0x000AA7E3
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
