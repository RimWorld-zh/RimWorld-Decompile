using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AD RID: 685
	public class LifeStageDef : Def
	{
		// Token: 0x04000666 RID: 1638
		[MustTranslate]
		private string adjective = null;

		// Token: 0x04000667 RID: 1639
		public bool visible = true;

		// Token: 0x04000668 RID: 1640
		public bool reproductive = false;

		// Token: 0x04000669 RID: 1641
		public bool milkable = false;

		// Token: 0x0400066A RID: 1642
		public bool shearable = false;

		// Token: 0x0400066B RID: 1643
		public float voxPitch = 1f;

		// Token: 0x0400066C RID: 1644
		public float voxVolume = 1f;

		// Token: 0x0400066D RID: 1645
		[NoTranslate]
		public string icon;

		// Token: 0x0400066E RID: 1646
		[Unsaved]
		public Texture2D iconTex;

		// Token: 0x0400066F RID: 1647
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x04000670 RID: 1648
		public float bodySizeFactor = 1f;

		// Token: 0x04000671 RID: 1649
		public float healthScaleFactor = 1f;

		// Token: 0x04000672 RID: 1650
		public float hungerRateFactor = 1f;

		// Token: 0x04000673 RID: 1651
		public float marketValueFactor = 1f;

		// Token: 0x04000674 RID: 1652
		public float foodMaxFactor = 1f;

		// Token: 0x04000675 RID: 1653
		public float meleeDamageFactor = 1f;

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00067318 File Offset: 0x00065718
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00067340 File Offset: 0x00065740
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.icon.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				});
			}
		}
	}
}
