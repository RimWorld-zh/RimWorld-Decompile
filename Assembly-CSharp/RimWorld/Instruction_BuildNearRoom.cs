using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B8 RID: 2232
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x001B7EA8 File Offset: 0x001B62A8
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x001B7ED0 File Offset: 0x001B62D0
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
