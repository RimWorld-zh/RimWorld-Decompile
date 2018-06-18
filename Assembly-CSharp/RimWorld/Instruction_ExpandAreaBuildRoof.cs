using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x001B8C3C File Offset: 0x001B703C
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
