using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class DebugTools_MapGen
	{
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

		[CompilerGenerated]
		private sealed class <Options_Scatterers>c__AnonStorey0
		{
			internal Type localSt;

			public <Options_Scatterers>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				GenStep_Scatterer genStep_Scatterer = (GenStep_Scatterer)Activator.CreateInstance(this.localSt);
				genStep_Scatterer.ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
			}
		}
	}
}
