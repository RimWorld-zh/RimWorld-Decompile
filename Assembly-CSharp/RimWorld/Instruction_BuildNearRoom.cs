using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BC RID: 2236
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x001B7CC0 File Offset: 0x001B60C0
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x001B7CE8 File Offset: 0x001B60E8
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
