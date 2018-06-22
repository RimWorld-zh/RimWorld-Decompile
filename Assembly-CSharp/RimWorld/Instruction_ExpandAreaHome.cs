using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C0 RID: 2240
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x001B8DF4 File Offset: 0x001B71F4
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
