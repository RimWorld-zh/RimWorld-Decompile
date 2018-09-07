using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public abstract class Designator_Area : Designator
	{
		protected Designator_Area()
		{
		}

		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
