using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B9 RID: 2233
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x001B7FB8 File Offset: 0x001B63B8
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
