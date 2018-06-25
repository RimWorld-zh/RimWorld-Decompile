using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BA RID: 2234
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x001B82BC File Offset: 0x001B66BC
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x001B82E4 File Offset: 0x001B66E4
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
