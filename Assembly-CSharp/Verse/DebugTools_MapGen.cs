using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E2A RID: 3626
	public static class DebugTools_MapGen
	{
		// Token: 0x060054FC RID: 21756 RVA: 0x002B9850 File Offset: 0x002B7C50
		public static List<DebugMenuOption> Options_Scatterers()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localSt2 in typeof(GenStep_Scatterer).AllLeafSubclasses())
			{
				Type localSt = localSt2;
				list.Add(new DebugMenuOption(localSt.ToString(), DebugMenuOptionMode.Tool, delegate()
				{
					GenStep_Scatterer genStep_Scatterer = (GenStep_Scatterer)Activator.CreateInstance(localSt);
					genStep_Scatterer.ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
				}));
			}
			return list;
		}
	}
}
