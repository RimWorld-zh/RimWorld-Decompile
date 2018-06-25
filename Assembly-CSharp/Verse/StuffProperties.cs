using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B2F RID: 2863
	public class StuffProperties
	{
		// Token: 0x040028EA RID: 10474
		public string stuffAdjective = null;

		// Token: 0x040028EB RID: 10475
		public float commonality = 1f;

		// Token: 0x040028EC RID: 10476
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x040028ED RID: 10477
		public bool smeltable = false;

		// Token: 0x040028EE RID: 10478
		public List<StatModifier> statOffsets = null;

		// Token: 0x040028EF RID: 10479
		public List<StatModifier> statFactors = null;

		// Token: 0x040028F0 RID: 10480
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x040028F1 RID: 10481
		public EffecterDef constructEffect = null;

		// Token: 0x040028F2 RID: 10482
		public StuffAppearanceDef appearance;

		// Token: 0x040028F3 RID: 10483
		public bool allowColorGenerators = false;

		// Token: 0x040028F4 RID: 10484
		public SoundDef soundImpactStuff = null;

		// Token: 0x040028F5 RID: 10485
		public SoundDef soundMeleeHitSharp = null;

		// Token: 0x040028F6 RID: 10486
		public SoundDef soundMeleeHitBlunt = null;

		// Token: 0x06003F08 RID: 16136 RVA: 0x002136D4 File Offset: 0x00211AD4
		public bool CanMake(BuildableDef t)
		{
			bool result;
			if (!t.MadeFromStuff)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < t.stuffCategories.Count; i++)
				{
					for (int j = 0; j < this.categories.Count; j++)
					{
						if (t.stuffCategories[i] == this.categories[j])
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0021375C File Offset: 0x00211B5C
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}
	}
}
