using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class PawnUtility
	{
		private static List<Pawn> tmpPawns = new List<Pawn>();

		private static List<string> tmpPawnKinds = new List<string>();

		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		private const float RecruitDifficultyMin = 0.33f;

		private const float RecruitDifficultyMax = 0.99f;

		private const float RecruitDifficultyRandomOffset = 0.2f;

		private const float RecruitDifficultyOffsetPerTechDiff = 0.15f;

		private static List<Thing> tmpThings = new List<Thing>();

		public static bool IsFactionLeader(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < allFactionsListForReading.Count)
				{
					if (allFactionsListForReading[num].leader == pawn)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool IsKidnappedPawn(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < allFactionsListForReading.Count)
				{
					if (allFactionsListForReading[num].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return ThingOwnerUtility.AnyParentIs<TravelingTransportPods>(pawn);
		}

		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is Settlement_TraderTracker;
		}

		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Func<Pawn, bool>)((Pawn x) => Find.GameInitData.startingPawns.Contains(x))))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		public static void DestroyStartingColonistFamily(Pawn pawn)
		{
			foreach (Pawn item in pawn.relations.RelatedPawns.ToList())
			{
				if (!Find.GameInitData.startingPawns.Contains(item))
				{
					WorldPawnSituation situation = Find.WorldPawns.GetSituation(item);
					if (situation == WorldPawnSituation.Free || situation == WorldPawnSituation.Dead)
					{
						Find.WorldPawns.RemovePawn(item);
						Find.WorldPawns.PassToWorld(item, PawnDiscardDecideMode.Discard);
					}
				}
			}
		}

		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false)
		{
			TraverseParms tp = (!passDoors) ? TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false) : TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			bool foundEnemy = false;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (RegionEntryPredicate)((Region from, Region to) => to.Allows(tp, false)), (RegionProcessor)delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				int num = 0;
				bool result;
				while (true)
				{
					if (num < list.Count)
					{
						if (list[num].HostileTo(pawn))
						{
							foundEnemy = true;
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = foundEnemy;
					break;
				}
				return result;
			}, regionsToScan, RegionType.Set_Passable);
			return foundEnemy;
		}

		public static bool WillSoonHaveBasicNeed(Pawn p)
		{
			return (byte)((p.needs != null) ? ((p.needs.rest != null && p.needs.rest.CurLevel < 0.33000001311302185) ? 1 : ((p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + 0.05000000074505806) ? 1 : 0)) : 0) != 0;
		}

		public static float AnimalFilthChancePerCell(ThingDef def, float bodySize)
		{
			float num = (float)(bodySize * 0.0012499999720603228);
			return (float)(num * (1.0 - def.race.petness));
		}

		public static bool CanCasuallyInteractNow(this Pawn p, bool twoWayInteraction = false)
		{
			bool result;
			if (p.Drafted)
			{
				result = false;
			}
			else if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p))
			{
				result = false;
			}
			else if (p.InAggroMentalState)
			{
				result = false;
			}
			else if (!p.Awake())
			{
				result = false;
			}
			else
			{
				Job curJob = p.CurJob;
				result = ((byte)((curJob == null || !twoWayInteraction || (curJob.def.casualInterruptible && curJob.playerForced)) ? 1 : 0) != 0);
			}
			return result;
		}

		public static IEnumerable<Pawn> SpawnedMasteredPawns(Pawn master)
		{
			if (Current.ProgramState == ProgramState.Playing && master.Faction != null && master.RaceProps.Humanlike && master.Spawned)
			{
				List<Pawn> pawns = master.Map.mapPawns.SpawnedPawnsInFaction(master.Faction);
				int i = 0;
				while (true)
				{
					if (i < pawns.Count)
					{
						if (pawns[i].playerSettings != null && pawns[i].playerSettings.master == master)
							break;
						i++;
						continue;
					}
					yield break;
				}
				yield return pawns[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static bool InValidState(Pawn p)
		{
			return (byte)((p.health != null) ? ((p.Dead || (p.stances != null && p.mindState != null && p.needs != null && p.ageTracker != null)) ? 1 : 0) : 0) != 0;
		}

		public static PawnPosture GetPosture(this Pawn p)
		{
			PawnPosture result;
			if (p.Downed || p.Dead)
			{
				result = PawnPosture.LayingAny;
			}
			else if (p.jobs == null)
			{
				result = PawnPosture.Standing;
			}
			else
			{
				Job curJob = p.jobs.curJob;
				result = ((curJob != null) ? p.jobs.curDriver.Posture : PawnPosture.Standing);
			}
			return result;
		}

		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639);
			}
			Job job = new Job((!maintainPosture) ? JobDefOf.Wait : JobDefOf.WaitMaintainPosture, faceTarget);
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, default(JobTag?), false);
		}

		public static void GiveNameBecauseOfNuzzle(Pawn namer, Pawn namee)
		{
			string text = (namee.Name != null) ? namee.Name.ToStringFull : namee.LabelIndefinite();
			namee.Name = PawnBioAndNameGenerator.GeneratePawnName(namee, NameStyle.Full, (string)null);
			if (namer.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageNuzzledPawnGaveNameTo".Translate(namer, text, namee.Name.ToStringFull), (Thing)namee, MessageTypeDefOf.NeutralEvent);
			}
		}

		public static void GainComfortFromCellIfPossible(this Pawn p)
		{
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				Building edifice = p.Position.GetEdifice(p.Map);
				if (edifice != null)
				{
					float statValue = edifice.GetStatValue(StatDefOf.Comfort, true);
					if (statValue >= 0.0 && p.needs != null && p.needs.comfort != null)
					{
						p.needs.comfort.ComfortUsed(statValue);
					}
				}
			}
		}

		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			float result;
			if (pawn.needs != null && pawn.needs.food != null)
			{
				switch (pawn.needs.food.CurCategory)
				{
				case HungerCategory.Fed:
				{
					result = 1f;
					goto IL_0081;
				}
				case HungerCategory.Hungry:
				{
					result = 0.666f;
					goto IL_0081;
				}
				case HungerCategory.UrgentlyHungry:
				{
					result = 0.333f;
					goto IL_0081;
				}
				case HungerCategory.Starving:
				{
					result = 0f;
					goto IL_0081;
				}
				}
			}
			result = 1f;
			goto IL_0081;
			IL_0081:
			return result;
		}

		public static bool FertileMateTarget(Pawn male, Pawn female)
		{
			bool result;
			if (female.gender != Gender.Female || !female.ageTracker.CurLifeStage.reproductive)
			{
				result = false;
			}
			else
			{
				CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
				result = ((compEggLayer == null) ? (!female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false)) : (!compEggLayer.FullyFertilized));
			}
			return result;
		}

		public static void Mated(Pawn male, Pawn female)
		{
			if (female.ageTracker.CurLifeStage.reproductive)
			{
				CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
				if (compEggLayer != null)
				{
					compEggLayer.Fertilize(male);
				}
				else if (Rand.Value < 0.5 && !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false))
				{
					Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, female, null);
					hediff_Pregnant.father = male;
					female.health.AddHediff(hediff_Pregnant, null, default(DamageInfo?));
				}
			}
		}

		public static bool PlayerForcedJobNowOrSoon(Pawn pawn)
		{
			bool result;
			if (pawn.jobs == null)
			{
				result = false;
			}
			else
			{
				Job curJob = pawn.CurJob;
				result = ((curJob == null) ? (pawn.jobs.jobQueue.Any() && pawn.jobs.jobQueue.Peek().job.playerForced) : curJob.playerForced);
			}
			return result;
		}

		public static bool TrySpawnHatchedOrBornPawn(Pawn pawn, Thing motherOrEgg)
		{
			bool result;
			if (motherOrEgg.SpawnedOrAnyParentSpawned)
			{
				result = (GenSpawn.Spawn(pawn, motherOrEgg.PositionHeld, motherOrEgg.MapHeld) != null);
			}
			else
			{
				Pawn pawn2 = motherOrEgg as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.IsCaravanMember())
					{
						pawn2.GetCaravan().AddPawn(pawn, true);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						result = true;
						goto IL_010b;
					}
					if (pawn2.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						result = true;
						goto IL_010b;
					}
				}
				else if (motherOrEgg.ParentHolder != null)
				{
					Pawn_InventoryTracker pawn_InventoryTracker = motherOrEgg.ParentHolder as Pawn_InventoryTracker;
					if (pawn_InventoryTracker != null)
					{
						if (pawn_InventoryTracker.pawn.IsCaravanMember())
						{
							pawn_InventoryTracker.pawn.GetCaravan().AddPawn(pawn, true);
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
							result = true;
							goto IL_010b;
						}
						if (pawn_InventoryTracker.pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
							result = true;
							goto IL_010b;
						}
					}
				}
				result = false;
			}
			goto IL_010b;
			IL_010b:
			return result;
		}

		public static ByteGrid GetAvoidGrid(this Pawn p)
		{
			ByteGrid result;
			if (p.Faction == null)
			{
				result = null;
			}
			else if (!p.Faction.def.canUseAvoidGrid)
			{
				result = null;
			}
			else if (p.Faction == Faction.OfPlayer || !p.Faction.RelationWith(Faction.OfPlayer, false).hostile)
			{
				result = null;
			}
			else
			{
				Lord lord = p.GetLord();
				if (lord != null)
				{
					if (lord.CurLordToil.avoidGridMode == AvoidGridMode.Ignore)
					{
						result = null;
						goto IL_00ee;
					}
					if (lord.CurLordToil.avoidGridMode == AvoidGridMode.Basic)
					{
						result = p.Faction.GetAvoidGridBasic(p.Map);
						goto IL_00ee;
					}
					if (lord.CurLordToil.avoidGridMode == AvoidGridMode.Smart)
					{
						result = p.Faction.GetAvoidGridSmart(p.Map);
						goto IL_00ee;
					}
				}
				result = p.Faction.GetAvoidGridBasic(p.Map);
			}
			goto IL_00ee;
			IL_00ee:
			return result;
		}

		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return (byte)((!p.Downed && !p.Dead) ? (p.mindState.anyCloseHostilesRecently ? 1 : 0) : 0) != 0;
		}

		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns) != null;
		}

		public static Pawn PawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false)
		{
			List<Thing> thingList = c.GetThingList(forPawn.Map);
			Pawn result;
			Pawn pawn;
			if (thingList.Count == 0)
			{
				result = null;
			}
			else
			{
				bool flag = false;
				if (actAsIfHadCollideWithPawnsJob)
				{
					flag = true;
				}
				else
				{
					Job curJob = forPawn.CurJob;
					if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns))
					{
						flag = true;
					}
					else if (forPawn.Drafted && forPawn.pather.Moving)
					{
						flag = true;
					}
				}
				for (int i = 0; i < thingList.Count; i++)
				{
					pawn = (thingList[i] as Pawn);
					if (pawn != null && pawn != forPawn && !pawn.Downed && (!collideOnlyWithStandingPawns || (!pawn.pather.MovingNow && (!pawn.pather.Moving || !pawn.pather.MovedRecently(60)))) && !PawnUtility.PawnsCanShareCellBecauseOfBodySize(pawn, forPawn))
					{
						if (pawn.HostileTo(forPawn))
							goto IL_011f;
						if (flag)
						{
							Job curJob2 = pawn.CurJob;
							if (curJob2 != null && (curJob2.collideWithPawns || curJob2.def.collideWithPawns))
							{
								goto IL_015b;
							}
						}
					}
				}
				result = null;
			}
			goto IL_0183;
			IL_011f:
			result = pawn;
			goto IL_0183;
			IL_0183:
			return result;
			IL_015b:
			result = pawn;
			goto IL_0183;
		}

		private static bool PawnsCanShareCellBecauseOfBodySize(Pawn p1, Pawn p2)
		{
			bool result;
			if (p1.BodySize >= 1.5 || p2.BodySize >= 1.5)
			{
				result = false;
			}
			else
			{
				float num = p1.BodySize / p2.BodySize;
				if (num < 1.0)
				{
					num = (float)(1.0 / num);
				}
				result = (num > 3.5699999332427979);
			}
			return result;
		}

		public static bool KnownDangerAt(IntVec3 c, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(forPawn.Map);
			return edifice != null && edifice.IsDangerousFor(forPawn);
		}

		public static bool ShouldSendNotificationAbout(Pawn p)
		{
			bool result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = false;
			}
			else if (PawnGenerator.IsBeingGenerated(p))
			{
				result = false;
			}
			else if (p.IsWorldPawn() && (!p.IsCaravanMember() || !p.GetCaravan().IsPlayerControlled) && !PawnUtility.IsTravelingInTransportPodWorldObject(p) && p.Corpse.DestroyedOrNull())
			{
				result = false;
			}
			else
			{
				if (p.Faction != Faction.OfPlayer)
				{
					if (p.HostFaction != Faction.OfPlayer)
					{
						result = false;
						goto IL_0108;
					}
					if (p.RaceProps.Humanlike && p.guest.Released && !p.Downed && !p.InBed())
					{
						result = false;
						goto IL_0108;
					}
					if (p.CurJob != null && p.CurJob.exitMapOnArrival && !PrisonBreakUtility.IsPrisonBreaking(p))
					{
						result = false;
						goto IL_0108;
					}
				}
				result = true;
			}
			goto IL_0108;
			IL_0108:
			return result;
		}

		public static bool ShouldGetThoughtAbout(Pawn pawn, Pawn subject)
		{
			return pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn());
		}

		public static bool IsTeetotaler(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) < 0;
		}

		public static bool IsProsthophobe(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Prosthophobe);
		}

		public static string PawnKindsToCommaList(List<Pawn> pawns)
		{
			PawnUtility.tmpPawns.Clear();
			PawnUtility.tmpPawns.AddRange(pawns);
			PawnUtility.tmpPawns.SortBy((Func<Pawn, bool>)((Pawn x) => !x.RaceProps.Humanlike), (Func<Pawn, string>)((Pawn x) => x.GetKindLabelPlural(-1)));
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKinds.Clear();
			for (int i = 0; i < PawnUtility.tmpPawns.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawns[i].kindDef))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawns[i].kindDef);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawns.Count; j++)
					{
						if (PawnUtility.tmpPawns[j].kindDef == PawnUtility.tmpPawns[i].kindDef)
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKinds.Add("1 " + PawnUtility.tmpPawns[i].KindLabel);
					}
					else
					{
						PawnUtility.tmpPawnKinds.Add(num + " " + PawnUtility.tmpPawns[i].GetKindLabelPlural(num));
					}
				}
			}
			return GenText.ToCommaList(PawnUtility.tmpPawnKinds, true);
		}

		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			return (pawn.Dead || pawn.mindState.duty == null || pawn.mindState.duty.locomotion == LocomotionUrgency.None) ? secondPriority : pawn.mindState.duty.locomotion;
		}

		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			return (locomotionUrgency == LocomotionUrgency.None) ? thirdPriority : locomotionUrgency;
		}

		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			return (pawn.Dead || pawn.mindState.duty == null || pawn.mindState.duty.maxDanger == Danger.Unspecified) ? secondPriority : pawn.mindState.duty.maxDanger;
		}

		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			return (danger == Danger.Unspecified) ? thirdPriority : danger;
		}

		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.WaitCombat || pawn.CurJob.def == JobDefOf.PredatorHunt);
		}

		public static float RecruitDifficulty(this Pawn pawn, Faction recruiterFaction, bool withPopIntent)
		{
			float baseRecruitDifficulty = pawn.kindDef.baseRecruitDifficulty;
			Rand.PushState();
			Rand.Seed = pawn.HashOffset();
			baseRecruitDifficulty += Rand.Range(-0.2f, 0.2f);
			Rand.PopState();
			if (pawn.Faction != null)
			{
				int num = Mathf.Abs(pawn.Faction.def.techLevel - recruiterFaction.def.techLevel);
				baseRecruitDifficulty = (float)(baseRecruitDifficulty + (float)num * 0.15000000596046448);
			}
			if (withPopIntent)
			{
				float popIntent = (float)((Current.ProgramState != ProgramState.Playing) ? 1.0 : Find.Storyteller.intenderPopulation.PopulationIntent);
				baseRecruitDifficulty = PawnUtility.PopIntentAdjustedRecruitDifficulty(baseRecruitDifficulty, popIntent);
			}
			return Mathf.Clamp(baseRecruitDifficulty, 0.33f, 0.99f);
		}

		private static float PopIntentAdjustedRecruitDifficulty(float baseDifficulty, float popIntent)
		{
			float num = Mathf.Clamp(popIntent, 0.25f, 3f);
			return (float)(1.0 - (1.0 - baseDifficulty) * num);
		}

		public static void DoTable_PopIntentRecruitDifficulty()
		{
			List<float> list = new List<float>();
			for (float num = -1f; num < 3.0; num = (float)(num + 0.10000000149011612))
			{
				list.Add(num);
			}
			List<float> list2 = new List<float>();
			list2.Add(0.1f);
			list2.Add(0.2f);
			list2.Add(0.3f);
			list2.Add(0.4f);
			list2.Add(0.5f);
			list2.Add(0.6f);
			list2.Add(0.7f);
			list2.Add(0.8f);
			list2.Add(0.9f);
			list2.Add(0.95f);
			list2.Add(0.99f);
			List<float> colValues = list2;
			DebugTables.MakeTablesDialog(colValues, (Func<float, string>)((float d) => "d=" + d.ToString("F0")), list, (Func<float, string>)((float rv) => rv.ToString("F1")), (Func<float, float, string>)((float d, float pi) => PawnUtility.PopIntentAdjustedRecruitDifficulty(d, pi).ToStringPercent()), "intents");
		}

		public static void GiveAllStartingPlayerPawnsThought(ThoughtDef thought)
		{
			foreach (Pawn startingPawn in Find.GameInitData.startingPawns)
			{
				if (thought.IsSocial)
				{
					foreach (Pawn startingPawn2 in Find.GameInitData.startingPawns)
					{
						if (startingPawn2 != startingPawn)
						{
							startingPawn.needs.mood.thoughts.memories.TryGainMemory(thought, startingPawn2);
						}
					}
				}
				else
				{
					startingPawn.needs.mood.thoughts.memories.TryGainMemory(thought, null);
				}
			}
		}

		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			return (pawn.mindState.duty == null || !pawn.mindState.duty.focus.IsValid) ? pawn.Position : pawn.mindState.duty.focus.Cell;
		}

		public static bool EverBeenColonistOrTameAnimal(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsColonistOrColonyAnimal) > 0;
		}

		public static bool EverBeenPrisoner(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsPrisoner) > 0;
		}

		public static void RecoverFromUnwalkablePositionOrKill(IntVec3 c, Map map)
		{
			if (c.InBounds(map) && !c.Walkable(map))
			{
				PawnUtility.tmpThings.Clear();
				PawnUtility.tmpThings.AddRange(c.GetThingList(map));
				for (int i = 0; i < PawnUtility.tmpThings.Count; i++)
				{
					Pawn pawn = PawnUtility.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						IntVec3 position = default(IntVec3);
						if (CellFinder.TryFindBestPawnStandCell(pawn, out position, false))
						{
							pawn.Position = position;
							pawn.Notify_Teleported(true);
						}
						else
						{
							DamageDef crush = DamageDefOf.Crush;
							int amount = 99999;
							BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
							DamageInfo damageInfo = new DamageInfo(crush, amount, -1f, null, brain, null, DamageInfo.SourceCategory.Collapse);
							pawn.TakeDamage(damageInfo);
							if (!pawn.Dead)
							{
								pawn.Kill(new DamageInfo?(damageInfo), null);
							}
						}
					}
				}
			}
		}
	}
}
