using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000298 RID: 664
	public class FleshTypeDef : Def
	{
		// Token: 0x040005F7 RID: 1527
		public ThoughtDef ateDirect = null;

		// Token: 0x040005F8 RID: 1528
		public ThoughtDef ateAsIngredient = null;

		// Token: 0x040005F9 RID: 1529
		public ThingCategoryDef corpseCategory = null;

		// Token: 0x040005FA RID: 1530
		public bool requiresBedForSurgery = true;

		// Token: 0x040005FB RID: 1531
		public List<FleshTypeDef.Wound> wounds = null;

		// Token: 0x040005FC RID: 1532
		private List<Material> woundsResolved = null;

		// Token: 0x06000B38 RID: 2872 RVA: 0x00065D28 File Offset: 0x00064128
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

		// Token: 0x02000299 RID: 665
		public class Wound
		{
			// Token: 0x040005FE RID: 1534
			[NoTranslate]
			public string texture;

			// Token: 0x040005FF RID: 1535
			public Color color = Color.white;

			// Token: 0x06000B3B RID: 2875 RVA: 0x00065DCC File Offset: 0x000641CC
			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}
		}
	}
}
