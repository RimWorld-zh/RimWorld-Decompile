using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BC RID: 1980
	public abstract class Designator_Area : Designator
	{
		// Token: 0x06002BDC RID: 11228 RVA: 0x00173C77 File Offset: 0x00172077
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
