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
		// Token: 0x06000B37 RID: 2871 RVA: 0x00065B74 File Offset: 0x00063F74
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

		// Token: 0x040005F6 RID: 1526
		public ThoughtDef ateDirect = null;

		// Token: 0x040005F7 RID: 1527
		public ThoughtDef ateAsIngredient = null;

		// Token: 0x040005F8 RID: 1528
		public ThingCategoryDef corpseCategory = null;

		// Token: 0x040005F9 RID: 1529
		public bool requiresBedForSurgery = true;

		// Token: 0x040005FA RID: 1530
		public List<FleshTypeDef.Wound> wounds = null;

		// Token: 0x040005FB RID: 1531
		private List<Material> woundsResolved = null;

		// Token: 0x02000297 RID: 663
		public class Wound
		{
			// Token: 0x06000B3A RID: 2874 RVA: 0x00065C18 File Offset: 0x00064018
			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}

			// Token: 0x040005FD RID: 1533
			[NoTranslate]
			public string texture;

			// Token: 0x040005FE RID: 1534
			public Color color = Color.white;
		}
	}
}
