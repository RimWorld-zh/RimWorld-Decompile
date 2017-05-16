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
			GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator22D <GraphicNamesInFolder>c__Iterator22D = new GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator22D();
			<GraphicNamesInFolder>c__Iterator22D.folderPath = folderPath;
			<GraphicNamesInFolder>c__Iterator22D.<$>folderPath = folderPath;
			GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator22D expr_15 = <GraphicNamesInFolder>c__Iterator22D;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
