using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086B RID: 2155
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x060030FA RID: 12538 RVA: 0x001A9947 File Offset: 0x001A7D47
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
