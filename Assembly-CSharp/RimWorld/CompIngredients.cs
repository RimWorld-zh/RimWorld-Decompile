using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000719 RID: 1817
	public class CompIngredients : ThingComp
	{
		// Token: 0x040015EA RID: 5610
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x040015EB RID: 5611
		private const int MaxNumIngredients = 3;

		// Token: 0x060027F0 RID: 10224 RVA: 0x00155BBC File Offset: 0x00153FBC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.ingredients, "ingredients", LookMode.Def, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ingredients == null)
				{
					this.ingredients = new List<ThingDef>();
				}
			}
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x00155C0A File Offset: 0x0015400A
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x00155C2C File Offset: 0x0015402C
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (piece != this.parent)
			{
				CompIngredients compIngredients = piece.TryGetComp<CompIngredients>();
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					compIngredients.ingredients.Add(this.ingredients[i]);
				}
			}
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x00155C8C File Offset: 0x0015408C
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompIngredients compIngredients = otherStack.TryGetComp<CompIngredients>();
			List<ThingDef> list = compIngredients.ingredients;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.ingredients.Contains(list[i]))
				{
					this.ingredients.Add(list[i]);
				}
			}
			if (this.ingredients.Count > 3)
			{
				this.ingredients.Shuffle<ThingDef>();
				while (this.ingredients.Count > 3)
				{
					this.ingredients.Remove(this.ingredients[this.ingredients.Count - 1]);
				}
			}
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x00155D4C File Offset: 0x0015414C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ingredients.Count > 0)
			{
				stringBuilder.Append("Ingredients".Translate() + ": ");
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					stringBuilder.Append(this.ingredients[i].label);
					if (i < this.ingredients.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
