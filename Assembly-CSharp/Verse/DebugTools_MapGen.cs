using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E2A RID: 3626
	public static class DebugTools_MapGen
	{
		// Token: 0x0600551C RID: 21788 RVA: 0x002BB720 File Offset: 0x002B9B20
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
