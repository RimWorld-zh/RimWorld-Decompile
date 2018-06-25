using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public struct ApparelGraphicRecord
	{
		// Token: 0x04003158 RID: 12632
		public Graphic graphic;

		// Token: 0x04003159 RID: 12633
		public Apparel sourceApparel;

		// Token: 0x060048D8 RID: 18648 RVA: 0x00263E0C File Offset: 0x0026220C
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}
	}
}
