using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		private PlayerPawnsArriveMethod method;

		public ScenPart_PlayerPawnsArriveMethod()
		{
		}

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
						object obj = enumerator.Current;
						PlayerPawnsArriveMethod localM2 = (PlayerPawnsArriveMethod)obj;
						PlayerPawnsArriveMethod localM = localM2;
						list.Add(new FloatMenuOption(localM.ToStringHuman(), delegate()
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
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				return "ScenPart_ArriveInDropPods".Translate();
			}
			return null;
		}

		public override void Randomize()
		{
			this.method = ((Rand.Value >= 0.5f) ? PlayerPawnsArriveMethod.Standing : PlayerPawnsArriveMethod.DropPods);
		}

		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			List<List<Thing>> list = new List<List<Thing>>();
			foreach (Pawn item in Find.GameInitData.startingAndOptionalPawns)
			{
				list.Add(new List<Thing>
				{
					item
				});
			}
			List<Thing> list2 = new List<Thing>();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				list2.AddRange(scenPart.PlayerStartingThings());
			}
			int num = 0;
			foreach (Thing thing in list2)
			{
				if (thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(Faction.OfPlayer);
				}
				list[num].Add(thing);
				num++;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			List<List<Thing>> thingsGroups = list;
			bool instaDrop = Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods;
			DropPodUtility.DropThingGroupsNear(playerStartSpot, map, thingsGroups, 110, instaDrop, true, true);
		}

		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
			}
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey0
		{
			internal PlayerPawnsArriveMethod localM;

			internal ScenPart_PlayerPawnsArriveMethod $this;

			public <DoEditInterface>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.method = this.localM;
			}
		}
	}
}
