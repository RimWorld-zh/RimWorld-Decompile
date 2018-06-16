using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x001B8B74 File Offset: 0x001B6F74
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
