using System;

namespace Verse
{
	// Token: 0x02000E24 RID: 3620
	public static class DebugTools
	{
		// Token: 0x060054D6 RID: 21718 RVA: 0x002B80AC File Offset: 0x002B64AC
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		// Token: 0x040037FD RID: 14333
		public static DebugTool curTool = null;
	}
}
