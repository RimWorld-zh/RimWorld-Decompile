using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000869 RID: 2153
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x060030F9 RID: 12537 RVA: 0x001A9D47 File Offset: 0x001A8147
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
