using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200031E RID: 798
	public class IncidentWorker_CaravanDemand : IncidentWorker
	{
		// Token: 0x06000D97 RID: 3479 RVA: 0x0007429C File Offset: 0x0007269C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000742DC File Offset: 0x000726DC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.points *= 1.5f;
			Caravan caravan = (Caravan)parms.target;
			bool result;
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				result = false;
			}
			else
			{
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
					result = false;
				}
				else
				{
					List<Thing> demands = this.GenerateDemands(caravan);
					if (demands.Count == 0)
					{
						result = false;
					}
					else
					{
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
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00074528 File Offset: 0x00072928
		private List<Thing> GenerateDemands(Caravan caravan)
		{
			List<Thing> list = new List<Thing>();
			List<Thing> list2 = new List<Thing>();
			list2.AddRange(caravan.PawnsListForReading.Cast<Thing>());
			list2.AddRange(caravan.PawnsListForReading.SelectMany((Pawn pawn) => ThingOwnerUtility.GetAllThingsRecursively(pawn, false)));
			float num = list2.Sum((Thing thing) => thing.MarketValue);
			float num2 = IncidentWorker_CaravanDemand.DemandAsPercentageOfCaravan.RandomInRange * num;
			while (num2 > 0f)
			{
				if (list2.Count((Thing thing) => thing is Pawn && ((Pawn)thing).IsColonist) == 1)
				{
					list2.RemoveAll((Thing thing) => thing is Pawn && ((Pawn)thing).IsColonist);
				}
				if (list2.Count == 0)
				{
					break;
				}
				Thing thing2 = list2.RandomElementByWeight(delegate(Thing thing)
				{
					float result;
					if (thing.def == ThingDefOf.Silver)
					{
						result = 5f;
					}
					else
					{
						if (thing is Pawn)
						{
							Pawn pawn = (Pawn)thing;
							if (pawn.RaceProps.Animal)
							{
								return 1f;
							}
							if (pawn.IsPrisoner)
							{
								return 1f;
							}
							if (pawn.IsColonist)
							{
								return 0.2f;
							}
						}
						result = 1f;
					}
					return result;
				});
				num2 -= thing2.MarketValue;
				list.Add(thing2);
				list2.Remove(thing2);
			}
			return list;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00074670 File Offset: 0x00072A70
		private string GenerateMessageText(Faction enemyFaction, int attackerCount, List<Thing> demands, Caravan caravan)
		{
			return "CaravanDemand".Translate(new object[]
			{
				caravan.Name,
				enemyFaction.Name,
				attackerCount,
				GenLabel.ThingsLabel(demands),
				enemyFaction.def.pawnsPlural
			}).CapitalizeFirst();
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000746D0 File Offset: 0x00072AD0
		private void TakeFromCaravan(Caravan caravan, List<Thing> demands, Faction enemyFaction)
		{
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < demands.Count; i++)
			{
				Thing thing = demands[i];
				if (thing is Pawn)
				{
					Pawn pawn = (Pawn)thing;
					caravan.RemovePawn(pawn);
					foreach (Thing thing2 in ThingOwnerUtility.GetAllThingsRecursively(pawn, false))
					{
						list.Add(thing2);
						thing2.holdingOwner.Take(thing2);
					}
					enemyFaction.kidnapped.KidnapPawn(pawn, null);
				}
				else
				{
					if (thing.holdingOwner != null)
					{
						thing.holdingOwner.Take(thing);
					}
					thing.Destroy(DestroyMode.Vanish);
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

		// Token: 0x06000D9C RID: 3484 RVA: 0x000747F8 File Offset: 0x00072BF8
		private void ActionGive(Caravan caravan, List<Thing> demands, List<Pawn> attackers)
		{
			this.TakeFromCaravan(caravan, demands, attackers[0].Faction);
			for (int i = 0; i < attackers.Count; i++)
			{
				Find.WorldPawns.PassToWorld(attackers[i], PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00074848 File Offset: 0x00072C48
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
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				CameraJumper.TryJump(attackers[0]);
			}, "GeneratingMapForNewEncounter", false, null);
		}

		// Token: 0x040008AC RID: 2220
		private static readonly FloatRange DemandAsPercentageOfCaravan = new FloatRange(0.02f, 0.35f);

		// Token: 0x040008AD RID: 2221
		private const float IncidentPointsFactor = 1.5f;

		// Token: 0x040008AE RID: 2222
		private const float DemandSilverWeight = 5f;

		// Token: 0x040008AF RID: 2223
		private const float DemandAnimalWeight = 1f;

		// Token: 0x040008B0 RID: 2224
		private const float DemandPrisonerWeight = 1f;

		// Token: 0x040008B1 RID: 2225
		private const float DemandColonistWeight = 0.2f;

		// Token: 0x040008B2 RID: 2226
		private const float DemandFallbackWeight = 1f;
	}
}
