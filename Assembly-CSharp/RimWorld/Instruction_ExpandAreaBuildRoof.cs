using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C3 RID: 2243
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x001B9238 File Offset: 0x001B7638
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
