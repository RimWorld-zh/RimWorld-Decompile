using System;

namespace Verse
{
	// Token: 0x02000E24 RID: 3620
	public static class DebugTools
	{
		// Token: 0x04003812 RID: 14354
		public static DebugTool curTool = null;

		// Token: 0x060054F6 RID: 21750 RVA: 0x002BA084 File Offset: 0x002B8484
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}
	}
}
