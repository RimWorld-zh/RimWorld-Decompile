using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000648 RID: 1608
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		// Token: 0x0600214E RID: 8526 RVA: 0x0011A8E2 File Offset: 0x00118CE2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlayerPawnsArriveMethod>(ref this.method, "method", PlayerPawnsArriveMethod.Standing, false);
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x0011A900 File Offset: 0x00118D00
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

		// Token: 0x06002150 RID: 8528 RVA: 0x0011A9F0 File Offset: 0x00118DF0
		public override string Summary(Scenario scen)
		{
			string result;
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				result = "ScenPart_ArriveInDropPods".Translate();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x0011AA22 File Offset: 0x00118E22
		public override void Randomize()
		{
			this.method = ((Rand.Value >= 0.5f) ? PlayerPawnsArriveMethod.Standing : PlayerPawnsArriveMethod.DropPods);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x0011AA44 File Offset: 0x00118E44
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData != null)
			{
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
				DropPodUtility.DropThingGroupsNear(playerStartSpot, map, thingsGroups, 110, instaDrop, true, true, false);
			}
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0011ABF8 File Offset: 0x00118FF8
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData != null)
			{
				if (this.method == PlayerPawnsArriveMethod.DropPods)
				{
					PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
				}
			}
		}

		// Token: 0x040012F7 RID: 4855
		private PlayerPawnsArriveMethod method = PlayerPawnsArriveMethod.Standing;
	}
}
