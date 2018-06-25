using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F01 RID: 3841
	public sealed class IngredientCount
	{
		// Token: 0x04003CE1 RID: 15585
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04003CE2 RID: 15586
		private float count = 1f;

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005C16 RID: 23574 RVA: 0x002EE0CC File Offset: 0x002EC4CC
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005C17 RID: 23575 RVA: 0x002EE0F0 File Offset: 0x002EC4F0
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

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06005C18 RID: 23576 RVA: 0x002EE12C File Offset: 0x002EC52C
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x002EE164 File Offset: 0x002EC564
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x002EE194 File Offset: 0x002EC594
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x002EE1AF File Offset: 0x002EC5AF
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x002EE1B9 File Offset: 0x002EC5B9
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x002EE1C8 File Offset: 0x002EC5C8
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}
	}
}
