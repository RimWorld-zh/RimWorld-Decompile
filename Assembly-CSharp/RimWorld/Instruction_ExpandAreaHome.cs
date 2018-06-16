using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C4 RID: 2244
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600334D RID: 13133 RVA: 0x001B8B44 File Offset: 0x001B6F44
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
