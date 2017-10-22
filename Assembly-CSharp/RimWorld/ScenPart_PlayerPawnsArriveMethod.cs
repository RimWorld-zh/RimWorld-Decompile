using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		private PlayerPawnsArriveMethod method = PlayerPawnsArriveMethod.Standing;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlayerPawnsArriveMethod>(ref this.method, "method", PlayerPawnsArriveMethod.Standing, false);
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.method.ToStringHuman(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				IEnumerator enumerator = Enum.GetValues(typeof(PlayerPawnsArriveMethod)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						PlayerPawnsArriveMethod playerPawnsArriveMethod = (PlayerPawnsArriveMethod)enumerator.Current;
						PlayerPawnsArriveMethod localM = playerPawnsArriveMethod;
						list.Add(new FloatMenuOption(localM.ToStringHuman(), (Action)delegate
						{
							this.method = localM;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public override string Summary(Scenario scen)
		{
			return (this.method != PlayerPawnsArriveMethod.DropPods) ? null : "ScenPart_ArriveInDropPods".Translate();
		}

		public override void Randomize()
		{
			this.method = (PlayerPawnsArriveMethod)((Rand.Value < 0.5) ? 1 : 0);
		}

		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData != null)
			{
				List<List<Thing>> list = new List<List<Thing>>();
				foreach (Pawn startingPawn in Find.GameInitData.startingPawns)
				{
					List<Thing> list2 = new List<Thing>();
					list2.Add(startingPawn);
					list.Add(list2);
				}
				List<Thing> list3 = new List<Thing>();
				foreach (ScenPart allPart in Find.Scenario.AllParts)
				{
					list3.AddRange(allPart.PlayerStartingThings());
				}
				int num = 0;
				foreach (Thing item in list3)
				{
					if (item.def.CanHaveFaction)
					{
						item.SetFactionDirect(Faction.OfPlayer);
					}
					list[num].Add(item);
					num++;
					if (num >= list.Count)
					{
						num = 0;
					}
				}
				IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
				List<List<Thing>> thingsGroups = list;
				bool instaDrop = Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods;
				DropPodUtility.DropThingGroupsNear(playerStartSpot, map, thingsGroups, 110, instaDrop, true, true, false);
			}
		}

		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData != null && this.method == PlayerPawnsArriveMethod.DropPods)
			{
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
			}
		}
	}
}
