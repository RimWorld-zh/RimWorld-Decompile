using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BB RID: 2235
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x001B7BC8 File Offset: 0x001B5FC8
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
