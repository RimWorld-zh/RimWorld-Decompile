using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE8 RID: 3304
	public struct ApparelGraphicRecord
	{
		// Token: 0x04003151 RID: 12625
		public Graphic graphic;

		// Token: 0x04003152 RID: 12626
		public Apparel sourceApparel;

		// Token: 0x060048D5 RID: 18645 RVA: 0x00263A50 File Offset: 0x00261E50
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}
	}
}
