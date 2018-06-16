using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B28 RID: 2856
	public class RecipeWorker
	{
		// Token: 0x06003EE8 RID: 16104 RVA: 0x000ABCEC File Offset: 0x000AA0EC
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x000ABD0F File Offset: 0x000AA10F
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x000ABD14 File Offset: 0x000AA114
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.isViolation;
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x000ABD48 File Offset: 0x000AA148
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x000ABD68 File Offset: 0x000AA168
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x040028C8 RID: 10440
		public RecipeDef recipe;
	}
}
