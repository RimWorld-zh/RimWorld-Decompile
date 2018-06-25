using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000869 RID: 2153
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x060030F8 RID: 12536 RVA: 0x001A9FAF File Offset: 0x001A83AF
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
