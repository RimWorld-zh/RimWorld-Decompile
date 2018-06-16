using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEC RID: 3308
	public struct ApparelGraphicRecord
	{
		// Token: 0x060048C6 RID: 18630 RVA: 0x00262660 File Offset: 0x00260A60
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x04003148 RID: 12616
		public Graphic graphic;

		// Token: 0x04003149 RID: 12617
		public Apparel sourceApparel;
	}
}
