using System;

namespace Verse
{
	public static class DebugTools
	{
		public static DebugTool curTool;

		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static DebugTools()
		{
		}
	}
}
