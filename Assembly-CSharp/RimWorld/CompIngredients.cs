using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class CompIngredients : ThingComp
	{
		// Token: 0x060027ED RID: 10221 RVA: 0x0015580C File Offset: 0x00153C0C
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

		// Token: 0x060027EE RID: 10222 RVA: 0x0015585A File Offset: 0x00153C5A
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x0015587C File Offset: 0x00153C7C
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

		// Token: 0x060027F0 RID: 10224 RVA: 0x001558DC File Offset: 0x00153CDC
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

		// Token: 0x060027F1 RID: 10225 RVA: 0x0015599C File Offset: 0x00153D9C
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

		// Token: 0x040015E6 RID: 5606
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x040015E7 RID: 5607
		private const int MaxNumIngredients = 3;
	}
}
