using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFE RID: 3838
	public sealed class IngredientCount
	{
		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x002EB93C File Offset: 0x002E9D3C
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005BE7 RID: 23527 RVA: 0x002EB960 File Offset: 0x002E9D60
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

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x002EB99C File Offset: 0x002E9D9C
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x002EB9D4 File Offset: 0x002E9DD4
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x002EBA04 File Offset: 0x002E9E04
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x002EBA1F File Offset: 0x002E9E1F
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06005BEC RID: 23532 RVA: 0x002EBA29 File Offset: 0x002E9E29
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06005BED RID: 23533 RVA: 0x002EBA38 File Offset: 0x002E9E38
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x04003CCC RID: 15564
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04003CCD RID: 15565
		private float count = 1f;
	}
}
