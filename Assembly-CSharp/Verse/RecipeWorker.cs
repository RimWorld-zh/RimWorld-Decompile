using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B28 RID: 2856
	public class RecipeWorker
	{
		// Token: 0x06003EEA RID: 16106 RVA: 0x000ABCF8 File Offset: 0x000AA0F8
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x000ABD1B File Offset: 0x000AA11B
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x000ABD20 File Offset: 0x000AA120
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.isViolation;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x000ABD54 File Offset: 0x000AA154
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x000ABD74 File Offset: 0x000AA174
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x040028C8 RID: 10440
		public RecipeDef recipe;
	}
}
