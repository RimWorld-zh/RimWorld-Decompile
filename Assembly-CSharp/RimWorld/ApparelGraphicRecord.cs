using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEA RID: 3306
	public struct ApparelGraphicRecord
	{
		// Token: 0x04003151 RID: 12625
		public Graphic graphic;

		// Token: 0x04003152 RID: 12626
		public Apparel sourceApparel;

		// Token: 0x060048D8 RID: 18648 RVA: 0x00263B2C File Offset: 0x00261F2C
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}
	}
}
