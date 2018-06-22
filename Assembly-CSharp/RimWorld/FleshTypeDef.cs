using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000296 RID: 662
	public class FleshTypeDef : Def
	{
		// Token: 0x06000B35 RID: 2869 RVA: 0x00065BDC File Offset: 0x00063FDC
		public Material ChooseWoundOverlay()
		{
			Material result;
			if (this.wounds == null)
			{
				result = null;
			}
			else
			{
				if (this.woundsResolved == null)
				{
					this.woundsResolved = (from wound in this.wounds
					select wound.GetMaterial()).ToList<Material>();
				}
				result = this.woundsResolved.RandomElement<Material>();
			}
			return result;
		}

		// Token: 0x040005F5 RID: 1525
		public ThoughtDef ateDirect = null;

		// Token: 0x040005F6 RID: 1526
		public ThoughtDef ateAsIngredient = null;

		// Token: 0x040005F7 RID: 1527
		public ThingCategoryDef corpseCategory = null;

		// Token: 0x040005F8 RID: 1528
		public bool requiresBedForSurgery = true;

		// Token: 0x040005F9 RID: 1529
		public List<FleshTypeDef.Wound> wounds = null;

		// Token: 0x040005FA RID: 1530
		private List<Material> woundsResolved = null;

		// Token: 0x02000297 RID: 663
		public class Wound
		{
			// Token: 0x06000B38 RID: 2872 RVA: 0x00065C80 File Offset: 0x00064080
			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}

			// Token: 0x040005FC RID: 1532
			[NoTranslate]
			public string texture;

			// Token: 0x040005FD RID: 1533
			public Color color = Color.white;
		}
	}
}
