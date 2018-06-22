using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B2C RID: 2860
	public class StuffProperties
	{
		// Token: 0x06003F04 RID: 16132 RVA: 0x002132C8 File Offset: 0x002116C8
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

		// Token: 0x06003F05 RID: 16133 RVA: 0x00213350 File Offset: 0x00211750
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x040028E2 RID: 10466
		public string stuffAdjective = null;

		// Token: 0x040028E3 RID: 10467
		public float commonality = 1f;

		// Token: 0x040028E4 RID: 10468
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x040028E5 RID: 10469
		public bool smeltable = false;

		// Token: 0x040028E6 RID: 10470
		public List<StatModifier> statOffsets = null;

		// Token: 0x040028E7 RID: 10471
		public List<StatModifier> statFactors = null;

		// Token: 0x040028E8 RID: 10472
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x040028E9 RID: 10473
		public EffecterDef constructEffect = null;

		// Token: 0x040028EA RID: 10474
		public StuffAppearanceDef appearance;

		// Token: 0x040028EB RID: 10475
		public bool allowColorGenerators = false;

		// Token: 0x040028EC RID: 10476
		public SoundDef soundImpactStuff = null;

		// Token: 0x040028ED RID: 10477
		public SoundDef soundMeleeHitSharp = null;

		// Token: 0x040028EE RID: 10478
		public SoundDef soundMeleeHitBlunt = null;
	}
}
