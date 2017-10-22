namespace Verse
{
	public static class DebugTools
	{
		public static DebugTool curTool = null;

		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}
	}
}
