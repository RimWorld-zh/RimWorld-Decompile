using System;

namespace Verse
{
	// Token: 0x02000E23 RID: 3619
	public static class DebugTools
	{
		// Token: 0x0400380B RID: 14347
		public static DebugTool curTool = null;

		// Token: 0x060054F6 RID: 21750 RVA: 0x002B9D90 File Offset: 0x002B8190
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}
	}
}
