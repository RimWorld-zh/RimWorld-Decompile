using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067A RID: 1658
	public static class InstallBlueprintUtility
	{
		// Token: 0x060022D5 RID: 8917 RVA: 0x0012BB38 File Offset: 0x00129F38
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0012BB5C File Offset: 0x00129F5C
		public static Blueprint_Install ExistingBlueprintFor(Thing th)
		{
			ThingDef installBlueprintDef = th.GetInnerIfMinified().def.installBlueprintDef;
			Blueprint_Install result;
			if (installBlueprintDef == null)
			{
				result = null;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsMatching(ThingRequest.ForDef(installBlueprintDef));
					for (int j = 0; j < list.Count; j++)
					{
						Blueprint_Install blueprint_Install = list[j] as Blueprint_Install;
						if (blueprint_Install != null && blueprint_Install.MiniToInstallOrBuildingToReinstall == th)
						{
							return blueprint_Install;
						}
					}
				}
				result = null;
			}
			return result;
		}
	}
}
