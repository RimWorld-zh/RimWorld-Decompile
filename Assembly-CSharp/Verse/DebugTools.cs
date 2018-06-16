using System;

namespace Verse
{
	// Token: 0x02000E25 RID: 3621
	public static class DebugTools
	{
		// Token: 0x060054D8 RID: 21720 RVA: 0x002B80AC File Offset: 0x002B64AC
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		// Token: 0x040037FF RID: 14335
		public static DebugTool curTool = null;
	}
}
