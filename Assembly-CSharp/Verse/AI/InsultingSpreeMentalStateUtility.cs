using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A58 RID: 2648
	public static class InsultingSpreeMentalStateUtility
	{
		// Token: 0x04002542 RID: 9538
		private const int MaxRegionsToSearch = 40;

		// Token: 0x04002543 RID: 9539
		public const int MaxDistance = 40;

		// Token: 0x04002544 RID: 9540
		public const int MinTicksBetweenInsults = 1200;

		// Token: 0x06003AF5 RID: 15093 RVA: 0x001F49FC File Offset: 0x001F2DFC
		public static bool CanChaseAndInsult(Pawn bully, Pawn insulted, bool skipReachabilityCheck = false, bool allowPrisoners = true)
		{
			return insulted.RaceProps.Humanlike && (insulted.Faction == bully.Faction || (allowPrisoners && insulted.HostFaction == bully.Faction)) && insulted != bully && !insulted.Dead && !insulted.Downed && insulted.Spawned && insulted.Awake() && insulted.Position.InHorDistOf(bully.Position, 40f) && InteractionUtility.CanReceiveInteraction(insulted) && !insulted.HostileTo(bully) && Find.TickManager.TicksGame - insulted.mindState.lastHarmTick >= 833 && (skipReachabilityCheck || bully.CanReach(insulted, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn));
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x001F4AF0 File Offset: 0x001F2EF0
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
