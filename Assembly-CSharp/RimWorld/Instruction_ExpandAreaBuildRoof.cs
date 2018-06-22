using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C1 RID: 2241
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x001B8E24 File Offset: 0x001B7224
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
