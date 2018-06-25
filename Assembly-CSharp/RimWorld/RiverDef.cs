using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002C4 RID: 708
	public class RiverDef : Def
	{
		// Token: 0x040006E7 RID: 1767
		public int spawnFlowThreshold = -1;

		// Token: 0x040006E8 RID: 1768
		public float spawnChance = 1f;

		// Token: 0x040006E9 RID: 1769
		public int degradeThreshold = 0;

		// Token: 0x040006EA RID: 1770
		public RiverDef degradeChild = null;

		// Token: 0x040006EB RID: 1771
		public List<RiverDef.Branch> branches;

		// Token: 0x040006EC RID: 1772
		public float widthOnWorld = 0.5f;

		// Token: 0x040006ED RID: 1773
		public float widthOnMap = 10f;

		// Token: 0x040006EE RID: 1774
		public float debugOpacity = 0f;

		// Token: 0x020002C5 RID: 709
		public class Branch
		{
			// Token: 0x040006EF RID: 1775
			public int minFlow = 0;

			// Token: 0x040006F0 RID: 1776
			public RiverDef child = null;

			// Token: 0x040006F1 RID: 1777
			public float chance = 1f;
		}
	}
}
