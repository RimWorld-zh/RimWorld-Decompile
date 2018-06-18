using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public struct ApparelGraphicRecord
	{
		// Token: 0x060048C4 RID: 18628 RVA: 0x00262638 File Offset: 0x00260A38
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x04003146 RID: 12614
		public Graphic graphic;

		// Token: 0x04003147 RID: 12615
		public Apparel sourceApparel;
	}
}
