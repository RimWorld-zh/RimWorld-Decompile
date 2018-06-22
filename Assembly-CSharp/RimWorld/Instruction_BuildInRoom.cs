using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B7 RID: 2231
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600330F RID: 13071 RVA: 0x001B7E78 File Offset: 0x001B6278
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
