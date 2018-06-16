using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BC RID: 2236
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x001B7BF8 File Offset: 0x001B5FF8
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x001B7C20 File Offset: 0x001B6020
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
