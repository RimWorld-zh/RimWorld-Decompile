using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B8 RID: 1976
	public abstract class Designator_Area : Designator
	{
		// Token: 0x06002BD7 RID: 11223 RVA: 0x00173EE3 File Offset: 0x001722E3
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
