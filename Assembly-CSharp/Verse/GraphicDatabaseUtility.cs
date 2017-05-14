using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public static class GraphicDatabaseUtility
	{
		[DebuggerHidden]
		public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
		{
			GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator22B <GraphicNamesInFolder>c__Iterator22B = new GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator22B();
			<GraphicNamesInFolder>c__Iterator22B.folderPath = folderPath;
			<GraphicNamesInFolder>c__Iterator22B.<$>folderPath = folderPath;
			GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator22B expr_15 = <GraphicNamesInFolder>c__Iterator22B;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
