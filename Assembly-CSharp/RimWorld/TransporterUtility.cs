using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000745 RID: 1861
	public static class TransporterUtility
	{
		// Token: 0x06002937 RID: 10551 RVA: 0x0015F364 File Offset: 0x0015D764
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

		// Token: 0x06002938 RID: 10552 RVA: 0x0015F3CC File Offset: 0x0015D7CC
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

		// Token: 0x06002939 RID: 10553 RVA: 0x0015F438 File Offset: 0x0015D838
		public static bool WasLoadingCanceled(Thing transporter)
		{
			CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
			return compTransporter != null && !compTransporter.LoadingInProgressOrReadyToLaunch;
		}
	}
}
