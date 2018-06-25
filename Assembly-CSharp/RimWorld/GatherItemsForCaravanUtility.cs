using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class GatherItemsForCaravanUtility
	{
		private static HashSet<Thing> neededItems = new HashSet<Thing>();

		public static Thing FindThingToHaul(Pawn p, Lord lord)
		{
			GatherItemsForCaravanUtility.neededItems.Clear();
			List<TransferableOneWay> transferables = ((LordJob_FormAndSendCaravan)lord.LordJob).transferables;
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (GatherItemsForCaravanUtility.CountLeftToTransfer(p, transferableOneWay, lord) > 0)
				{
					for (int j = 0; j < transferableOneWay.things.Count; j++)
					{
						GatherItemsForCaravanUtility.neededItems.Add(transferableOneWay.things[j]);
					}
				}
			}
			Thing result;
			if (!GatherItemsForCaravanUtility.neededItems.Any<Thing>())
			{
				result = null;
			}
			else
			{
				Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.Touch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => GatherItemsForCaravanUtility.neededItems.Contains(x) && p.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
				GatherItemsForCaravanUtility.neededItems.Clear();
				result = thing;
			}
			return result;
		}

		public static int CountLeftToTransfer(Pawn pawn, TransferableOneWay transferable, Lord lord)
		{
			int result;
			if (transferable.CountToTransfer <= 0 || !transferable.HasAnyThing)
			{
				result = 0;
			}
			else
			{
				result = Mathf.Max(transferable.CountToTransfer - GatherItemsForCaravanUtility.TransferableCountHauledByOthers(pawn, transferable, lord), 0);
			}
			return result;
		}

		private static int TransferableCountHauledByOthers(Pawn pawn, TransferableOneWay transferable, Lord lord)
		{
			int result;
			if (!transferable.HasAnyThing)
			{
				Log.Warning("Can't determine transferable count hauled by others because transferable has 0 things.", false);
				result = 0;
			}
			else
			{
				List<Pawn> allPawnsSpawned = lord.Map.mapPawns.AllPawnsSpawned;
				int num = 0;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2 != pawn)
					{
						if (pawn2.CurJob != null && pawn2.CurJob.def == JobDefOf.PrepareCaravan_GatherItems && pawn2.CurJob.lord == lord)
						{
							Thing toHaul = ((JobDriver_PrepareCaravan_GatherItems)pawn2.jobs.curDriver).ToHaul;
							if (transferable.things.Contains(toHaul) || TransferableUtility.TransferAsOne(transferable.AnyThing, toHaul, TransferAsOneMode.PodsOrCaravanPacking))
							{
								num += toHaul.stackCount;
							}
						}
					}
				}
				result = num;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GatherItemsForCaravanUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <FindThingToHaul>c__AnonStorey0
		{
			internal Pawn p;

			public <FindThingToHaul>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return GatherItemsForCaravanUtility.neededItems.Contains(x) && this.p.CanReserve(x, 1, -1, null, false);
			}
		}
	}
}
