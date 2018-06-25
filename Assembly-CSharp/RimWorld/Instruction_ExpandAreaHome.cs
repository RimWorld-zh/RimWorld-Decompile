using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C2 RID: 2242
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x001B8F34 File Offset: 0x001B7334
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
