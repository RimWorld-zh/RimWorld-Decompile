using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class FacilitiesUtility
	{
		private const float MaxDistToLinkToFacilityEver = 10f;

		private static int RegionsToSearch = (1 + 2 * Mathf.CeilToInt(0.8333333f)) * (1 + 2 * Mathf.CeilToInt(0.8333333f));

		private static HashSet<Region> visited = new HashSet<Region>();

		private static HashSet<Thing> processed = new HashSet<Thing>();

		private static bool working;

		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache0;

		public static void NotifyFacilitiesAboutChangedLOSBlockers(List<Region> affectedRegions)
		{
			if (affectedRegions.Any<Region>())
			{
				if (FacilitiesUtility.working)
				{
					Log.Warning("Tried to update facilities while already updating.", false);
				}
				else
				{
					FacilitiesUtility.working = true;
					try
					{
						FacilitiesUtility.visited.Clear();
						FacilitiesUtility.processed.Clear();
						int facilitiesToProcess = affectedRegions[0].Map.listerThings.ThingsInGroup(ThingRequestGroup.Facility).Count;
						int affectedByFacilitiesToProcess = affectedRegions[0].Map.listerThings.ThingsInGroup(ThingRequestGroup.AffectedByFacilities).Count;
						int facilitiesProcessed = 0;
						int affectedByFacilitiesProcessed = 0;
						if (facilitiesToProcess > 0 && affectedByFacilitiesToProcess > 0)
						{
							for (int i = 0; i < affectedRegions.Count; i++)
							{
								if (!FacilitiesUtility.visited.Contains(affectedRegions[i]))
								{
									RegionTraverser.BreadthFirstTraverse(affectedRegions[i], (Region from, Region r) => !FacilitiesUtility.visited.Contains(r), delegate(Region x)
									{
										FacilitiesUtility.visited.Add(x);
										List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
										for (int j = 0; j < list.Count; j++)
										{
											if (!FacilitiesUtility.processed.Contains(list[j]))
											{
												FacilitiesUtility.processed.Add(list[j]);
												CompFacility compFacility = list[j].TryGetComp<CompFacility>();
												CompAffectedByFacilities compAffectedByFacilities = list[j].TryGetComp<CompAffectedByFacilities>();
												if (compFacility != null)
												{
													compFacility.Notify_LOSBlockerSpawnedOrDespawned();
													facilitiesProcessed++;
												}
												if (compAffectedByFacilities != null)
												{
													compAffectedByFacilities.Notify_LOSBlockerSpawnedOrDespawned();
													affectedByFacilitiesProcessed++;
												}
											}
										}
										return facilitiesProcessed >= facilitiesToProcess && affectedByFacilitiesProcessed >= affectedByFacilitiesToProcess;
									}, FacilitiesUtility.RegionsToSearch, RegionType.Set_Passable);
									if (facilitiesProcessed >= facilitiesToProcess && affectedByFacilitiesProcessed >= affectedByFacilitiesToProcess)
									{
										break;
									}
								}
							}
						}
					}
					finally
					{
						FacilitiesUtility.working = false;
						FacilitiesUtility.visited.Clear();
						FacilitiesUtility.processed.Clear();
					}
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FacilitiesUtility()
		{
		}

		[CompilerGenerated]
		private static bool <NotifyFacilitiesAboutChangedLOSBlockers>m__0(Region from, Region r)
		{
			return !FacilitiesUtility.visited.Contains(r);
		}

		[CompilerGenerated]
		private sealed class <NotifyFacilitiesAboutChangedLOSBlockers>c__AnonStorey0
		{
			internal int facilitiesProcessed;

			internal int affectedByFacilitiesProcessed;

			internal int facilitiesToProcess;

			internal int affectedByFacilitiesToProcess;

			public <NotifyFacilitiesAboutChangedLOSBlockers>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region x)
			{
				FacilitiesUtility.visited.Add(x);
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int i = 0; i < list.Count; i++)
				{
					if (!FacilitiesUtility.processed.Contains(list[i]))
					{
						FacilitiesUtility.processed.Add(list[i]);
						CompFacility compFacility = list[i].TryGetComp<CompFacility>();
						CompAffectedByFacilities compAffectedByFacilities = list[i].TryGetComp<CompAffectedByFacilities>();
						if (compFacility != null)
						{
							compFacility.Notify_LOSBlockerSpawnedOrDespawned();
							this.facilitiesProcessed++;
						}
						if (compAffectedByFacilities != null)
						{
							compAffectedByFacilities.Notify_LOSBlockerSpawnedOrDespawned();
							this.affectedByFacilitiesProcessed++;
						}
					}
				}
				return this.facilitiesProcessed >= this.facilitiesToProcess && this.affectedByFacilitiesProcessed >= this.affectedByFacilitiesToProcess;
			}
		}
	}
}
