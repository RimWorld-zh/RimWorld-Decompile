using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BB RID: 2235
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x001B7C90 File Offset: 0x001B6090
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
