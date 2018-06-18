using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000747 RID: 1863
	public static class TransporterUtility
	{
		// Token: 0x0600293B RID: 10555 RVA: 0x0015EDDC File Offset: 0x0015D1DC
		public static void GetTransportersInGroup(int transportersGroup, Map map, List<CompTransporter> outTransporters)
		{
			outTransporters.Clear();
			if (transportersGroup >= 0)
			{
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
				for (int i = 0; i < list.Count; i++)
				{
					CompTransporter compTransporter = list[i].TryGetComp<CompTransporter>();
					if (compTransporter.groupID == transportersGroup)
					{
						outTransporters.Add(compTransporter);
					}
				}
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0015EE44 File Offset: 0x0015D244
		public static Lord FindLord(int transportersGroup, Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters = lords[i].LordJob as LordJob_LoadAndEnterTransporters;
				if (lordJob_LoadAndEnterTransporters != null && lordJob_LoadAndEnterTransporters.transportersGroup == transportersGroup)
				{
					return lords[i];
				}
			}
			return null;
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x0015EEB0 File Offset: 0x0015D2B0
		public static bool WasLoadingCanceled(Thing transporter)
		{
			CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
			return compTransporter != null && !compTransporter.LoadingInProgressOrReadyToLaunch;
		}
	}
}
