using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		private PlayerPawnsArriveMethod method;

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
				foreach (int value in Enum.GetValues(typeof(PlayerPawnsArriveMethod)))
				{
					PlayerPawnsArriveMethod localM = (PlayerPawnsArriveMethod)value;
					list.Add(new FloatMenuOption(localM.ToStringHuman(), (Action)delegate
					{
						this.method = localM;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public override string Summary(Scenario scen)
		{
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				return "ScenPart_ArriveInDropPods".Translate();
			}
			return (string)null;
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
				List<Pawn>.Enumerator enumerator = Find.GameInitData.startingPawns.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Pawn current = enumerator.Current;
						List<Thing> list2 = new List<Thing>();
						list2.Add(current);
						list.Add(list2);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				List<Thing> list3 = new List<Thing>();
				foreach (ScenPart allPart in Find.Scenario.AllParts)
				{
					list3.AddRange(allPart.PlayerStartingThings());
				}
				int num = 0;
				List<Thing>.Enumerator enumerator3 = list3.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						Thing current3 = enumerator3.Current;
						if (current3.def.CanHaveFaction)
						{
							current3.SetFactionDirect(Faction.OfPlayer);
						}
						list[num].Add(current3);
						num++;
						if (num >= list.Count)
						{
							num = 0;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator3).Dispose();
				}
				bool instaDrop = Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods;
				DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot, map, list, 110, instaDrop, true, true);
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
