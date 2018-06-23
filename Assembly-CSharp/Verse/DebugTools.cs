using System;

namespace Verse
{
	// Token: 0x02000E21 RID: 3617
	public static class DebugTools
	{
		// Token: 0x0400380B RID: 14347
		public static DebugTool curTool = null;

		// Token: 0x060054F2 RID: 21746 RVA: 0x002B9C64 File Offset: 0x002B8064
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}
	}
}
