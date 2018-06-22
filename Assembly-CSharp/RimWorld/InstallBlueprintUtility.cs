using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000676 RID: 1654
	public static class InstallBlueprintUtility
	{
		// Token: 0x060022CF RID: 8911 RVA: 0x0012BCF8 File Offset: 0x0012A0F8
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0012BD1C File Offset: 0x0012A11C
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
