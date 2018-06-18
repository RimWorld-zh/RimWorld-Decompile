using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086B RID: 2155
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x060030FC RID: 12540 RVA: 0x001A9A0F File Offset: 0x001A7E0F
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
