using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B27 RID: 2855
	public class RecipeWorker
	{
		// Token: 0x040028CC RID: 10444
		public RecipeDef recipe;

		// Token: 0x06003EEA RID: 16106 RVA: 0x000AC058 File Offset: 0x000AA458
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x000AC07B File Offset: 0x000AA47B
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x000AC080 File Offset: 0x000AA480
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.isViolation;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x000AC0B4 File Offset: 0x000AA4B4
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x000AC0D4 File Offset: 0x000AA4D4
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}
	}
}
