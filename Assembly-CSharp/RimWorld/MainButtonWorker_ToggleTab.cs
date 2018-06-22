using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000867 RID: 2151
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x060030F5 RID: 12533 RVA: 0x001A9BF7 File Offset: 0x001A7FF7
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
