using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A56 RID: 2646
	public static class InsultingSpreeMentalStateUtility
	{
		// Token: 0x04002541 RID: 9537
		private const int MaxRegionsToSearch = 40;

		// Token: 0x04002542 RID: 9538
		public const int MaxDistance = 40;

		// Token: 0x04002543 RID: 9539
		public const int MinTicksBetweenInsults = 1200;

		// Token: 0x06003AF1 RID: 15089 RVA: 0x001F48D0 File Offset: 0x001F2CD0
		public static bool CanChaseAndInsult(Pawn bully, Pawn insulted, bool skipReachabilityCheck = false, bool allowPrisoners = true)
		{
			return insulted.RaceProps.Humanlike && (insulted.Faction == bully.Faction || (allowPrisoners && insulted.HostFaction == bully.Faction)) && insulted != bully && !insulted.Dead && !insulted.Downed && insulted.Spawned && insulted.Awake() && insulted.Position.InHorDistOf(bully.Position, 40f) && InteractionUtility.CanReceiveInteraction(insulted) && !insulted.HostileTo(bully) && Find.TickManager.TicksGame - insulted.mindState.lastHarmTick >= 833 && (skipReachabilityCheck || bully.CanReach(insulted, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn));
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x001F49C4 File Offset: 0x001F2DC4
		public static void GetInsultCandidatesFor(Pawn bully, List<Pawn> outCandidates, bool allowPrisoners = true)
		{
			outCandidates.Clear();
			Region region = bully.GetRegion(RegionType.Set_Passable);
			if (region != null)
			{
				TraverseParms traverseParams = TraverseParms.For(bully, Danger.Deadly, TraverseMode.ByPawn, false);
				RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
				{
					List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
					for (int i = 0; i < list.Count; i++)
					{
						Pawn pawn = (Pawn)list[i];
						if (InsultingSpreeMentalStateUtility.CanChaseAndInsult(bully, pawn, true, allowPrisoners))
						{
							outCandidates.Add(pawn);
						}
					}
					return false;
				}, 40, RegionType.Set_Passable);
			}
		}
	}
}
