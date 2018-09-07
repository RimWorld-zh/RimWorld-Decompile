using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_CaravanDemand : IncidentWorker
	{
		private static readonly FloatRange DemandAsPercentageOfCaravan = new FloatRange(0.05f, 0.2f);

		private static readonly FloatRange IncidentPointsFactorRange = new FloatRange(1f, 1.7f);

		private const float DemandAnimalsWeight = 0.15f;

		private const float DemandColonistOrPrisonerWeight = 0.15f;

		private const float DemandItemsWeight = 1.5f;

		private const float MaxDemandedAnimalsPct = 0.6f;

		private const float MinDemandedMarketValue = 300f;

		private const float MaxDemandedMarketValue = 3500f;

		private const float TrashMarketValueThreshold = 50f;

		private const float IgnoreApparelMarketValueThreshold = 500f;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, ThingCount> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Thing>> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache4;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<Thing, float> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<Thing, float> <>f__am$cache7;

		public IncidentWorker_CaravanDemand()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.points *= IncidentWorker_CaravanDemand.IncidentPointsFactorRange.RandomInRange;
			Caravan caravan = (Caravan)parms.target;
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				return false;
			}
			List<ThingCount> demands = this.GenerateDemands(caravan);
			if (demands.NullOrEmpty<ThingCount>())
			{
				return false;
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
			defaultPawnGroupMakerParms.generateFightersOnly = true;
			defaultPawnGroupMakerParms.dontUseSingleUseRocketLaunchers = true;
			List<Pawn> attackers = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			if (attackers.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Caravan demand incident couldn't generate any enemies even though min points have been checked. faction=",
					defaultPawnGroupMakerParms.faction,
					"(",
					(defaultPawnGroupMakerParms.faction == null) ? "null" : defaultPawnGroupMakerParms.faction.def.ToString(),
					") parms=",
					parms
				}), false);
				return false;
			}
			CameraJumper.TryJumpAndSelect(caravan);
			DiaNode diaNode = new DiaNode(this.GenerateMessageText(parms.faction, attackers.Count, demands, caravan));
			DiaOption diaOption = new DiaOption("CaravanDemand_Give".Translate());
			diaOption.action = delegate()
			{
				this.ActionGive(caravan, demands, attackers);
			};
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			DiaOption diaOption2 = new DiaOption("CaravanDemand_Fight".Translate());
			diaOption2.action = delegate()
			{
				this.ActionFight(caravan, attackers);
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			string text = "CaravanDemandTitle".Translate(new object[]
			{
				parms.faction.Name
			});
			WindowStack windowStack = Find.WindowStack;
			DiaNode nodeRoot = diaNode;
			Faction faction = parms.faction;
			bool delayInteractivity = true;
			string title = text;
			windowStack.Add(new Dialog_NodeTreeWithFactionInfo(nodeRoot, faction, delayInteractivity, false, title));
			Find.Archive.Add(new ArchivedDialog(diaNode.text, text, parms.faction));
			return true;
		}

		private List<ThingCount> GenerateDemands(Caravan caravan)
		{
			float num = 1.8f;
			float num2 = Rand.Value * num;
			if (num2 < 0.15f)
			{
				List<ThingCount> list = this.TryGenerateColonistOrPrisonerDemand(caravan);
				if (!list.NullOrEmpty<ThingCount>())
				{
					return list;
				}
			}
			if (num2 < 0.3f)
			{
				List<ThingCount> list2 = this.TryGenerateAnimalsDemand(caravan);
				if (!list2.NullOrEmpty<ThingCount>())
				{
					return list2;
				}
			}
			List<ThingCount> list3 = this.TryGenerateItemsDemand(caravan);
			if (!list3.NullOrEmpty<ThingCount>())
			{
				return list3;
			}
			if (Rand.Bool)
			{
				List<ThingCount> list4 = this.TryGenerateColonistOrPrisonerDemand(caravan);
				if (!list4.NullOrEmpty<ThingCount>())
				{
					return list4;
				}
				List<ThingCount> list5 = this.TryGenerateAnimalsDemand(caravan);
				if (!list5.NullOrEmpty<ThingCount>())
				{
					return list5;
				}
			}
			else
			{
				List<ThingCount> list6 = this.TryGenerateAnimalsDemand(caravan);
				if (!list6.NullOrEmpty<ThingCount>())
				{
					return list6;
				}
				List<ThingCount> list7 = this.TryGenerateColonistOrPrisonerDemand(caravan);
				if (!list7.NullOrEmpty<ThingCount>())
				{
					return list7;
				}
			}
			return null;
		}

		private List<ThingCount> TryGenerateColonistOrPrisonerDemand(Caravan caravan)
		{
			List<Pawn> list = new List<Pawn>();
			int num = 0;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				if (caravan.IsOwner(caravan.pawns[i]))
				{
					num++;
				}
			}
			if (num >= 2)
			{
				for (int j = 0; j < caravan.pawns.Count; j++)
				{
					if (caravan.IsOwner(caravan.pawns[j]))
					{
						list.Add(caravan.pawns[j]);
					}
				}
			}
			for (int k = 0; k < caravan.pawns.Count; k++)
			{
				if (caravan.pawns[k].IsPrisoner)
				{
					list.Add(caravan.pawns[k]);
				}
			}
			if (list.Any<Pawn>())
			{
				return new List<ThingCount>
				{
					new ThingCount(list.RandomElement<Pawn>(), 1)
				};
			}
			return null;
		}

		private List<ThingCount> TryGenerateAnimalsDemand(Caravan caravan)
		{
			int num = 0;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				if (caravan.pawns[i].RaceProps.Animal)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			int count = Rand.RangeInclusive(1, (int)Mathf.Max((float)num * 0.6f, 1f));
			return (from x in (from x in caravan.pawns.InnerListForReading
			where x.RaceProps.Animal
			orderby x.MarketValue descending
			select x).Take(count)
			select new ThingCount(x, 1)).ToList<ThingCount>();
		}

		private List<ThingCount> TryGenerateItemsDemand(Caravan caravan)
		{
			List<ThingCount> list = new List<ThingCount>();
			List<Thing> list2 = new List<Thing>();
			list2.AddRange(caravan.PawnsListForReading.SelectMany((Pawn x) => ThingOwnerUtility.GetAllThingsRecursively(x, false)));
			list2.RemoveAll((Thing x) => x.MarketValue * (float)x.stackCount < 50f);
			list2.RemoveAll((Thing x) => x.ParentHolder is Pawn_ApparelTracker && x.MarketValue < 500f);
			float num = list2.Sum((Thing x) => x.MarketValue * (float)x.stackCount);
			float requestedCaravanValue = Mathf.Clamp(IncidentWorker_CaravanDemand.DemandAsPercentageOfCaravan.RandomInRange * num, 300f, 3500f);
			while (requestedCaravanValue > 50f)
			{
				Thing thing;
				if (!(from x in list2
				where x.MarketValue * (float)x.stackCount <= requestedCaravanValue * 2f
				select x).TryRandomElementByWeight((Thing x) => Mathf.Pow(x.MarketValue / x.GetStatValue(StatDefOf.Mass, true), 2f), out thing))
				{
					return null;
				}
				int num2 = Mathf.Clamp((int)(requestedCaravanValue / thing.MarketValue), 1, thing.stackCount);
				requestedCaravanValue -= thing.MarketValue * (float)num2;
				list.Add(new ThingCount(thing, num2));
				list2.Remove(thing);
			}
			return list;
		}

		private string GenerateMessageText(Faction enemyFaction, int attackerCount, List<ThingCount> demands, Caravan caravan)
		{
			return "CaravanDemand".Translate(new object[]
			{
				caravan.Name,
				enemyFaction.Name,
				attackerCount,
				GenLabel.ThingsLabel(demands, "  - "),
				enemyFaction.def.pawnsPlural
			}).CapitalizeFirst();
		}

		private void TakeFromCaravan(Caravan caravan, List<ThingCount> demands, Faction enemyFaction)
		{
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < demands.Count; i++)
			{
				ThingCount thingCount = demands[i];
				if (thingCount.Thing is Pawn)
				{
					Pawn pawn = (Pawn)thingCount.Thing;
					caravan.RemovePawn(pawn);
					foreach (Thing thing in ThingOwnerUtility.GetAllThingsRecursively(pawn, false))
					{
						list.Add(thing);
						thing.holdingOwner.Take(thing);
					}
					enemyFaction.kidnapped.KidnapPawn(pawn, null);
				}
				else
				{
					thingCount.Thing.SplitOff(thingCount.Count).Destroy(DestroyMode.Vanish);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!list[j].Destroyed)
				{
					CaravanInventoryUtility.GiveThing(caravan, list[j]);
				}
			}
		}

		private void ActionGive(Caravan caravan, List<ThingCount> demands, List<Pawn> attackers)
		{
			this.TakeFromCaravan(caravan, demands, attackers[0].Faction);
			for (int i = 0; i < attackers.Count; i++)
			{
				Find.WorldPawns.PassToWorld(attackers[i], PawnDiscardDecideMode.Decide);
			}
		}

		private void ActionFight(Caravan caravan, List<Pawn> attackers)
		{
			Faction enemyFaction = attackers[0].Faction;
			TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushedByHumanlike, new object[]
			{
				caravan.RandomOwner()
			});
			LongEventHandler.QueueLongEvent(delegate()
			{
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, attackers, true);
				LordJob_AssaultColony lordJob_AssaultColony = new LordJob_AssaultColony(enemyFaction, true, false, false, false, true);
				if (lordJob_AssaultColony != null)
				{
					LordMaker.MakeNewLord(enemyFaction, lordJob_AssaultColony, map, attackers);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				CameraJumper.TryJump(attackers[0]);
			}, "GeneratingMapForNewEncounter", false, null);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_CaravanDemand()
		{
		}

		[CompilerGenerated]
		private static bool <TryGenerateAnimalsDemand>m__0(Pawn x)
		{
			return x.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static float <TryGenerateAnimalsDemand>m__1(Pawn x)
		{
			return x.MarketValue;
		}

		[CompilerGenerated]
		private static ThingCount <TryGenerateAnimalsDemand>m__2(Pawn x)
		{
			return new ThingCount(x, 1);
		}

		[CompilerGenerated]
		private static IEnumerable<Thing> <TryGenerateItemsDemand>m__3(Pawn x)
		{
			return ThingOwnerUtility.GetAllThingsRecursively(x, false);
		}

		[CompilerGenerated]
		private static bool <TryGenerateItemsDemand>m__4(Thing x)
		{
			return x.MarketValue * (float)x.stackCount < 50f;
		}

		[CompilerGenerated]
		private static bool <TryGenerateItemsDemand>m__5(Thing x)
		{
			return x.ParentHolder is Pawn_ApparelTracker && x.MarketValue < 500f;
		}

		[CompilerGenerated]
		private static float <TryGenerateItemsDemand>m__6(Thing x)
		{
			return x.MarketValue * (float)x.stackCount;
		}

		[CompilerGenerated]
		private static float <TryGenerateItemsDemand>m__7(Thing x)
		{
			return Mathf.Pow(x.MarketValue / x.GetStatValue(StatDefOf.Mass, true), 2f);
		}

		[CompilerGenerated]
		private sealed class <TryExecuteWorker>c__AnonStorey0
		{
			internal Caravan caravan;

			internal List<ThingCount> demands;

			internal List<Pawn> attackers;

			internal IncidentWorker_CaravanDemand $this;

			public <TryExecuteWorker>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.ActionGive(this.caravan, this.demands, this.attackers);
			}

			internal void <>m__1()
			{
				this.$this.ActionFight(this.caravan, this.attackers);
			}
		}

		[CompilerGenerated]
		private sealed class <TryGenerateItemsDemand>c__AnonStorey1
		{
			internal float requestedCaravanValue;

			public <TryGenerateItemsDemand>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.MarketValue * (float)x.stackCount <= this.requestedCaravanValue * 2f;
			}
		}

		[CompilerGenerated]
		private sealed class <ActionFight>c__AnonStorey2
		{
			internal Caravan caravan;

			internal List<Pawn> attackers;

			internal Faction enemyFaction;

			public <ActionFight>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(this.caravan, this.attackers, true);
				LordJob_AssaultColony lordJob_AssaultColony = new LordJob_AssaultColony(this.enemyFaction, true, false, false, false, true);
				if (lordJob_AssaultColony != null)
				{
					LordMaker.MakeNewLord(this.enemyFaction, lordJob_AssaultColony, map, this.attackers);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				CameraJumper.TryJump(this.attackers[0]);
			}
		}
	}
}
