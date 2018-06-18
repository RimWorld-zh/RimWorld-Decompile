using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BC RID: 1980
	public abstract class Designator_Area : Designator
	{
		// Token: 0x06002BDE RID: 11230 RVA: 0x00173D0B File Offset: 0x0017210B
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
