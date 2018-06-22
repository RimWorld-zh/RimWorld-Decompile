using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200060E RID: 1550
	public static class TransportPodsArrivalActionUtility
	{
		// Token: 0x06001F38 RID: 7992 RVA: 0x0010EE80 File Offset: 0x0010D280
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

		// Token: 0x06001F39 RID: 7993 RVA: 0x0010EEC8 File Offset: 0x0010D2C8
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

		// Token: 0x06001F3A RID: 7994 RVA: 0x0010EF78 File Offset: 0x0010D378
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

		// Token: 0x06001F3B RID: 7995 RVA: 0x0010F020 File Offset: 0x0010D420
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

		// Token: 0x06001F3C RID: 7996 RVA: 0x0010F0E0 File Offset: 0x0010D4E0
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

		// Token: 0x06001F3D RID: 7997 RVA: 0x0010F12C File Offset: 0x0010D52C
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
