using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFD RID: 3837
	public sealed class IngredientCount
	{
		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x002EBA18 File Offset: 0x002E9E18
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06005BE5 RID: 23525 RVA: 0x002EBA3C File Offset: 0x002E9E3C
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + this, false);
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x002EBA78 File Offset: 0x002E9E78
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x002EBAB0 File Offset: 0x002E9EB0
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x002EBAE0 File Offset: 0x002E9EE0
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x002EBAFB File Offset: 0x002E9EFB
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x002EBB05 File Offset: 0x002E9F05
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x002EBB14 File Offset: 0x002E9F14
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x04003CCB RID: 15563
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04003CCC RID: 15564
		private float count = 1f;
	}
}
