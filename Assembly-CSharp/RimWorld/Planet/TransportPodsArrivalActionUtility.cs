using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000612 RID: 1554
	public static class TransportPodsArrivalActionUtility
	{
		// Token: 0x06001F3F RID: 7999 RVA: 0x0010EDB4 File Offset: 0x0010D1B4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, CompLaunchable representative, int destinationTile) where T : TransportPodsArrivalAction
		{
			FloatMenuAcceptanceReport rep = acceptanceReportGetter();
			if (rep.Accepted || !rep.FailReason.NullOrEmpty() || !rep.FailMessage.NullOrEmpty())
			{
				if (!rep.FailReason.NullOrEmpty())
				{
					yield return new FloatMenuOption(label + " (" + rep.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					yield return new FloatMenuOption(label, delegate()
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
						if (floatMenuAcceptanceReport.Accepted)
						{
							representative.TryLaunch(destinationTile, arrivalActionGetter());
						}
						else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
			}
			yield break;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0010EDFC File Offset: 0x0010D1FC
		public static bool AnyNonDownedColonist(IEnumerable<IThingHolder> pods)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0010EEAC File Offset: 0x0010D2AC
		public static bool AnyPotentialCaravanOwner(IEnumerable<IThingHolder> pods, Faction faction)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && CaravanUtility.IsOwner(pawn, faction))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0010EF54 File Offset: 0x0010D354
		public static Thing GetLookTarget(List<ActiveDropPodInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner directlyHeldThings = pods[i].GetDirectlyHeldThings();
				for (int j = 0; j < directlyHeldThings.Count; j++)
				{
					Pawn pawn = directlyHeldThings[j] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						return pawn;
					}
				}
			}
			for (int k = 0; k < pods.Count; k++)
			{
				Thing thing = pods[k].GetDirectlyHeldThings().FirstOrDefault<Thing>();
				if (thing != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0010F014 File Offset: 0x0010D414
		public static void DropTravelingTransportPods(List<ActiveDropPodInfo> dropPods, IntVec3 near, Map map)
		{
			TransportPodsArrivalActionUtility.RemovePawnsFromWorldPawns(dropPods);
			for (int i = 0; i < dropPods.Count; i++)
			{
				IntVec3 c;
				DropCellFinder.TryFindDropSpotNear(near, map, out c, false, true, false);
				DropPodUtility.MakeDropPodAt(c, map, dropPods[i], false);
			}
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0010F060 File Offset: 0x0010D460
		public static void RemovePawnsFromWorldPawns(List<ActiveDropPodInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = 0; j < innerContainer.Count; j++)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
		}
	}
}
