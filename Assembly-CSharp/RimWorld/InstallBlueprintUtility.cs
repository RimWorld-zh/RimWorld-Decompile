using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000678 RID: 1656
	public static class InstallBlueprintUtility
	{
		// Token: 0x060022D2 RID: 8914 RVA: 0x0012C0B0 File Offset: 0x0012A4B0
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x0012C0D4 File Offset: 0x0012A4D4
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
