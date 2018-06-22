using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B24 RID: 2852
	public class RecipeWorker
	{
		// Token: 0x06003EE6 RID: 16102 RVA: 0x000ABD08 File Offset: 0x000AA108
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x000ABD2B File Offset: 0x000AA12B
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x000ABD30 File Offset: 0x000AA130
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.isViolation;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x000ABD64 File Offset: 0x000AA164
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x000ABD84 File Offset: 0x000AA184
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x040028C4 RID: 10436
		public RecipeDef recipe;
	}
}
