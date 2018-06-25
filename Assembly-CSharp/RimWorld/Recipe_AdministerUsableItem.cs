using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000466 RID: 1126
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x060013CC RID: 5068 RVA: 0x000AC72D File Offset: 0x000AAB2D
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000AC743 File Offset: 0x000AAB43
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
