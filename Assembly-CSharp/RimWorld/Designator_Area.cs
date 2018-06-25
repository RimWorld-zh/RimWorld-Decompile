using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BA RID: 1978
	public abstract class Designator_Area : Designator
	{
		// Token: 0x06002BDB RID: 11227 RVA: 0x00174033 File Offset: 0x00172433
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
