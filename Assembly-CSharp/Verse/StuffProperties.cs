using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B30 RID: 2864
	public class StuffProperties
	{
		// Token: 0x06003F06 RID: 16134 RVA: 0x00212EB8 File Offset: 0x002112B8
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

		// Token: 0x06003F07 RID: 16135 RVA: 0x00212F40 File Offset: 0x00211340
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x040028E6 RID: 10470
		public string stuffAdjective = null;

		// Token: 0x040028E7 RID: 10471
		public float commonality = 1f;

		// Token: 0x040028E8 RID: 10472
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x040028E9 RID: 10473
		public bool smeltable = false;

		// Token: 0x040028EA RID: 10474
		public List<StatModifier> statOffsets = null;

		// Token: 0x040028EB RID: 10475
		public List<StatModifier> statFactors = null;

		// Token: 0x040028EC RID: 10476
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x040028ED RID: 10477
		public EffecterDef constructEffect = null;

		// Token: 0x040028EE RID: 10478
		public StuffAppearanceDef appearance;

		// Token: 0x040028EF RID: 10479
		public bool allowColorGenerators = false;

		// Token: 0x040028F0 RID: 10480
		public SoundDef soundImpactStuff = null;

		// Token: 0x040028F1 RID: 10481
		public SoundDef soundMeleeHitSharp = null;

		// Token: 0x040028F2 RID: 10482
		public SoundDef soundMeleeHitBlunt = null;
	}
}
