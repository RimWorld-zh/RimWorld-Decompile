using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BA RID: 1978
	public abstract class Designator_Area : Designator
	{
		// Token: 0x06002BDA RID: 11226 RVA: 0x00174297 File Offset: 0x00172697
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
