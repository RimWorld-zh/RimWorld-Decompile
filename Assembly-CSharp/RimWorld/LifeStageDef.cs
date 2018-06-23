using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AB RID: 683
	public class LifeStageDef : Def
	{
		// Token: 0x04000664 RID: 1636
		[MustTranslate]
		private string adjective = null;

		// Token: 0x04000665 RID: 1637
		public bool visible = true;

		// Token: 0x04000666 RID: 1638
		public bool reproductive = false;

		// Token: 0x04000667 RID: 1639
		public bool milkable = false;

		// Token: 0x04000668 RID: 1640
		public bool shearable = false;

		// Token: 0x04000669 RID: 1641
		public float voxPitch = 1f;

		// Token: 0x0400066A RID: 1642
		public float voxVolume = 1f;

		// Token: 0x0400066B RID: 1643
		[NoTranslate]
		public string icon;

		// Token: 0x0400066C RID: 1644
		[Unsaved]
		public Texture2D iconTex;

		// Token: 0x0400066D RID: 1645
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x0400066E RID: 1646
		public float bodySizeFactor = 1f;

		// Token: 0x0400066F RID: 1647
		public float healthScaleFactor = 1f;

		// Token: 0x04000670 RID: 1648
		public float hungerRateFactor = 1f;

		// Token: 0x04000671 RID: 1649
		public float marketValueFactor = 1f;

		// Token: 0x04000672 RID: 1650
		public float foodMaxFactor = 1f;

		// Token: 0x04000673 RID: 1651
		public float meleeDamageFactor = 1f;

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x000671CC File Offset: 0x000655CC
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x000671F4 File Offset: 0x000655F4
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
