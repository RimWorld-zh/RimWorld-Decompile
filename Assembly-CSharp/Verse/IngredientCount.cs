using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFD RID: 3837
	public sealed class IngredientCount
	{
		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005C0C RID: 23564 RVA: 0x002EDA4C File Offset: 0x002EBE4C
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06005C0D RID: 23565 RVA: 0x002EDA70 File Offset: 0x002EBE70
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

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06005C0E RID: 23566 RVA: 0x002EDAAC File Offset: 0x002EBEAC
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x002EDAE4 File Offset: 0x002EBEE4
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x002EDB14 File Offset: 0x002EBF14
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x002EDB2F File Offset: 0x002EBF2F
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x002EDB39 File Offset: 0x002EBF39
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x002EDB48 File Offset: 0x002EBF48
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x04003CDE RID: 15582
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04003CDF RID: 15583
		private float count = 1f;
	}
}
