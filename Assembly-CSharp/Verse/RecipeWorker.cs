using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B26 RID: 2854
	public class RecipeWorker
	{
		// Token: 0x040028C5 RID: 10437
		public RecipeDef recipe;

		// Token: 0x06003EEA RID: 16106 RVA: 0x000ABE58 File Offset: 0x000AA258
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x000ABE7B File Offset: 0x000AA27B
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x000ABE80 File Offset: 0x000AA280
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.isViolation;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x000ABEB4 File Offset: 0x000AA2B4
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x000ABED4 File Offset: 0x000AA2D4
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}
	}
}
