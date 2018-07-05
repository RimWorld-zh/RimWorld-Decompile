using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	[HasDebugOutput]
	public static class PawnUtility
	{
		private const float HumanFilthFactor = 4f;

		private static List<Pawn> tmpPawns = new List<Pawn>();

		private static List<string> tmpPawnKinds = new List<string>();

		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		private const float RecruitDifficultyMin = 0.33f;

		private const float RecruitDifficultyMax = 0.99f;

		private const float RecruitDifficultyRandomOffset = 0.2f;

		private const float RecruitDifficultyOffsetPerTechDiff = 0.15f;

		private static List<Thing> tmpThings = new List<Thing>();

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<float, float, string> <>f__am$cache5;

		public static bool IsFactionLeader(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].leader == pawn)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsKidnappedPawn(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return ThingOwnerUtility.AnyParentIs<TravelingTransportPods>(pawn);
		}

		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is SettlementBase_TraderTracker;
		}

		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Pawn x) => Find.GameInitData.startingAndOptionalPawns.Contains(x)))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		public static void DestroyStartingColonistFamily(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns.ToList<Pawn>())
			{
				if (!Find.GameInitData.startingAndOptionalPawns.Contains(pawn2))
				{
					WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn2);
					if (situation == WorldPawnSituation.Free || situation == WorldPawnSituation.Dead)
					{
						Find.WorldPawns.RemovePawn(pawn2);
						Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					}
				}
			}
		}

		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false)
		{
			TraverseParms tp = (!passDoors) ? TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false) : TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			bool foundEnemy = false;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].HostileTo(pawn))
					{
						foundEnemy = true;
						return true;
					}
				}
				return foundEnemy;
			}, regionsToScan, RegionType.Set_Passable);
			return foundEnemy;
		}

		public static bool WillSoonHaveBasicNeed(Pawn p)
		{
			return p.needs != null && ((p.needs.rest != null && p.needs.rest.CurLevel < 0.33f) || (p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + 0.05f));
		}

		public static float AnimalFilthChancePerCell(ThingDef def, float bodySize)
		{
			float num = bodySize * 0.00125f;
			return num * (1f - def.race.petness);
		}

		public static float HumanFilthChancePerCell(ThingDef def, float bodySize)
		{
			float num = bodySize * 0.00125f;
			return num * 4f;
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
				if (curJob != null)
				{
					if (twoWayInteraction)
					{
						if (!curJob.def.casualInterruptible || !curJob.playerForced)
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		public static IEnumerable<Pawn> SpawnedMasteredPawns(Pawn master)
		{
			if (Current.ProgramState != ProgramState.Playing || master.Faction == null || !master.RaceProps.Humanlike)
			{
				yield break;
			}
			if (!master.Spawned)
			{
				yield break;
			}
			List<Pawn> pawns = master.Map.mapPawns.SpawnedPawnsInFaction(master.Faction);
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i].playerSettings != null && pawns[i].playerSettings.Master == master)
				{
					yield return pawns[i];
				}
			}
			yield break;
		}

		public static bool InValidState(Pawn p)
		{
			bool result;
			if (p.health == null)
			{
				result = false;
			}
			else
			{
				if (!p.Dead)
				{
					if (p.stances == null || p.mindState == null || p.needs == null || p.ageTracker == null)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public static PawnPosture GetPosture(this Pawn p)
		{
			PawnPosture result;
			if (p.Dead)
			{
				result = PawnPosture.LayingOnGroundNormal;
			}
			else if (p.Downed)
			{
				if (p.jobs != null && p.jobs.posture.Laying())
				{
					result = p.jobs.posture;
				}
				else
				{
					result = PawnPosture.LayingOnGroundNormal;
				}
			}
			else if (p.jobs == null)
			{
				result = PawnPosture.Standing;
			}
			else
			{
				result = p.jobs.posture;
			}
			return result;
		}

		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639, false);
			}
			Job job = new Job((!maintainPosture) ? JobDefOf.Wait : JobDefOf.Wait_MaintainPosture, faceTarget);
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false);
		}

		public static void GiveNameBecauseOfNuzzle(Pawn namer, Pawn namee)
		{
			string text = (namee.Name != null) ? namee.Name.ToStringFull : namee.LabelIndefinite();
			namee.Name = PawnBioAndNameGenerator.GeneratePawnName(namee, NameStyle.Full, null);
			if (namer.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageNuzzledPawnGaveNameTo".Translate(new object[]
				{
					namer,
					text,
					namee.Name.ToStringFull
				}), namee, MessageTypeDefOf.NeutralEvent, true);
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
					if (statValue >= 0f && p.needs != null && p.needs.comfort != null)
					{
						p.needs.comfort.ComfortUsed(statValue);
					}
				}
			}
		}

		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			if (pawn.needs != null && pawn.needs.food != null)
			{
				switch (pawn.needs.food.CurCategory)
				{
				case HungerCategory.Fed:
					return 1f;
				case HungerCategory.Hungry:
					return 0.666f;
				case HungerCategory.UrgentlyHungry:
					return 0.333f;
				case HungerCategory.Starving:
					return 0f;
				}
			}
			return 1f;
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
				if (compEggLayer != null)
				{
					result = !compEggLayer.FullyFertilized;
				}
				else
				{
					result = !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false);
				}
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
				else if (Rand.Value < 0.5f && !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false))
				{
					Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, female, null);
					hediff_Pregnant.father = male;
					female.health.AddHediff(hediff_Pregnant, null, null, null);
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
				if (curJob != null)
				{
					result = curJob.playerForced;
				}
				else
				{
					result = (pawn.jobs.jobQueue.Any<QueuedJob>() && pawn.jobs.jobQueue.Peek().job.playerForced);
				}
			}
			return result;
		}

		public static bool TrySpawnHatchedOrBornPawn(Pawn pawn, Thing motherOrEgg)
		{
			bool result;
			if (motherOrEgg.SpawnedOrAnyParentSpawned)
			{
				result = (GenSpawn.Spawn(pawn, motherOrEgg.PositionHeld, motherOrEgg.MapHeld, WipeMode.Vanish) != null);
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
						return true;
					}
					if (pawn2.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
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
							return true;
						}
						if (pawn_InventoryTracker.pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
							return true;
						}
					}
				}
				result = false;
			}
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
			else if (p.Faction == Faction.OfPlayer || !p.Faction.HostileTo(Faction.OfPlayer))
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
						return null;
					}
					if (lord.CurLordToil.avoidGridMode == AvoidGridMode.Smart)
					{
						return p.Faction.GetAvoidGridSmart(p.Map);
					}
				}
				result = null;
			}
			return result;
		}

		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return !p.Downed && !p.Dead && p.mindState.anyCloseHostilesRecently;
		}

		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns, forPathFinder) != null;
		}

		public static Pawn PawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			List<Thing> thingList = c.GetThingList(forPawn.Map);
			Pawn result;
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
					if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || forPawn.jobs.curDriver.collideWithPawns))
					{
						flag = true;
					}
					else if (!forPawn.Drafted || forPawn.pather.Moving)
					{
					}
				}
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && pawn != forPawn && !pawn.Downed)
					{
						if (collideOnlyWithStandingPawns)
						{
							if (pawn.pather.MovingNow)
							{
								goto IL_1BA;
							}
							if (pawn.pather.Moving && pawn.pather.MovedRecently(60))
							{
								goto IL_1BA;
							}
						}
						if (!PawnUtility.PawnsCanShareCellBecauseOfBodySize(pawn, forPawn))
						{
							if (pawn.HostileTo(forPawn))
							{
								return pawn;
							}
							if (flag)
							{
								if (forPathFinder || !forPawn.Drafted || !pawn.RaceProps.Animal)
								{
									Job curJob2 = pawn.CurJob;
									if (curJob2 != null && (curJob2.collideWithPawns || curJob2.def.collideWithPawns || pawn.jobs.curDriver.collideWithPawns))
									{
										return pawn;
									}
								}
							}
						}
					}
					IL_1BA:;
				}
				result = null;
			}
			return result;
		}

		private static bool PawnsCanShareCellBecauseOfBodySize(Pawn p1, Pawn p2)
		{
			bool result;
			if (p1.BodySize >= 1.5f || p2.BodySize >= 1.5f)
			{
				result = false;
			}
			else
			{
				float num = p1.BodySize / p2.BodySize;
				if (num < 1f)
				{
					num = 1f / num;
				}
				result = (num > 3.57f);
			}
			return result;
		}

		public static bool KnownDangerAt(IntVec3 c, Map map, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(map);
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
						return false;
					}
					if (p.RaceProps.Humanlike && p.guest.Released && !p.Downed && !p.InBed())
					{
						return false;
					}
					if (p.CurJob != null && p.CurJob.exitMapOnArrival && !PrisonBreakUtility.IsPrisonBreaking(p))
					{
						return false;
					}
				}
				result = true;
			}
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
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.BodyPurist);
		}

		public static string PawnKindsToCommaList(List<Pawn> pawns, bool useAnd = false)
		{
			PawnUtility.tmpPawns.Clear();
			PawnUtility.tmpPawns.AddRange(pawns);
			if (PawnUtility.tmpPawns.Count >= 2)
			{
				PawnUtility.tmpPawns.SortBy((Pawn x) => !x.RaceProps.Humanlike, (Pawn x) => x.GetKindLabelPlural(-1));
			}
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
			return PawnUtility.tmpPawnKinds.ToCommaList(useAnd);
		}

		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			LocomotionUrgency result;
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
			{
				result = pawn.mindState.duty.locomotion;
			}
			else
			{
				result = secondPriority;
			}
			return result;
		}

		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			LocomotionUrgency result;
			if (locomotionUrgency != LocomotionUrgency.None)
			{
				result = locomotionUrgency;
			}
			else
			{
				result = thirdPriority;
			}
			return result;
		}

		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			Danger result;
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.maxDanger != Danger.Unspecified)
			{
				result = pawn.mindState.duty.maxDanger;
			}
			else
			{
				result = secondPriority;
			}
			return result;
		}

		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			Danger result;
			if (danger != Danger.Unspecified)
			{
				result = danger;
			}
			else
			{
				result = thirdPriority;
			}
			return result;
		}

		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.Wait_Combat || pawn.CurJob.def == JobDefOf.PredatorHunt);
		}

		public static float RecruitDifficulty(this Pawn pawn, Faction recruiterFaction, bool withPopIntent)
		{
			float num = pawn.kindDef.baseRecruitDifficulty;
			Rand.PushState();
			Rand.Seed = pawn.HashOffset();
			num += Rand.Range(-0.2f, 0.2f);
			Rand.PopState();
			if (pawn.Faction != null)
			{
				int num2 = Mathf.Abs((int)(pawn.Faction.def.techLevel - recruiterFaction.def.techLevel));
				num += (float)num2 * 0.15f;
			}
			if (withPopIntent)
			{
				float popIntent = (Current.ProgramState != ProgramState.Playing) ? 1f : Find.Storyteller.intenderPopulation.PopulationIntent;
				num = PawnUtility.PopIntentAdjustedRecruitDifficulty(num, popIntent);
			}
			return Mathf.Clamp(num, 0.33f, 0.99f);
		}

		private static float PopIntentAdjustedRecruitDifficulty(float baseDifficulty, float popIntent)
		{
			float num = Mathf.Clamp(popIntent, 0.25f, 3f);
			return 1f - (1f - baseDifficulty) * num;
		}

		[DebugOutput]
		public static void PopIntentRecruitDifficulty()
		{
			List<float> list = new List<float>();
			for (float num = -1f; num < 3f; num += 0.1f)
			{
				list.Add(num);
			}
			List<float> colValues = new List<float>
			{
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				0.95f,
				0.99f
			};
			DebugTables.MakeTablesDialog<float, float>(colValues, (float d) => "d=" + d.ToString("F0"), list, (float rv) => rv.ToString("F1"), (float d, float pi) => PawnUtility.PopIntentAdjustedRecruitDifficulty(d, pi).ToStringPercent(), "intents");
		}

		public static void GiveAllStartingPlayerPawnsThought(ThoughtDef thought)
		{
			foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
			{
				if (thought.IsSocial)
				{
					foreach (Pawn pawn2 in Find.GameInitData.startingAndOptionalPawns)
					{
						if (pawn2 != pawn)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(thought, pawn2);
						}
					}
				}
				else
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought, null);
				}
			}
		}

		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			IntVec3 result;
			if (pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid)
			{
				result = pawn.mindState.duty.focus.Cell;
			}
			else
			{
				result = pawn.Position;
			}
			return result;
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
						IntVec3 position;
						if (CellFinder.TryFindBestPawnStandCell(pawn, out position, false))
						{
							pawn.Position = position;
							pawn.Notify_Teleported(true);
						}
						else
						{
							DamageDef crush = DamageDefOf.Crush;
							float amount = 99999f;
							float armorPenetration = 999f;
							BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
							DamageInfo damageInfo = new DamageInfo(crush, amount, armorPenetration, -1f, null, brain, null, DamageInfo.SourceCategory.Collapse, null);
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

		public static float GetBaseManhunterOnDamageChance(Pawn pawn)
		{
			return PawnUtility.GetBaseManhunterOnDamageChance(pawn.kindDef);
		}

		public static float GetBaseManhunterOnDamageChance(PawnKindDef kind)
		{
			return (kind != PawnKindDefOf.WildMan) ? kind.RaceProps.manhunterOnDamageChance : 0.5f;
		}

		public static float GetManhunterOnDamageChance(Pawn pawn, float distance)
		{
			float manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			return manhunterOnDamageChance * GenMath.LerpDoubleClamped(1f, 30f, 3f, 1f, distance);
		}

		public static float GetManhunterOnDamageChance(Pawn pawn, Thing instigator = null)
		{
			float manhunterOnDamageChance;
			if (instigator != null)
			{
				manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn, pawn.Position.DistanceTo(instigator.Position));
			}
			else
			{
				manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			}
			return manhunterOnDamageChance;
		}

		public static float GetManhunterOnDamageChance(PawnKindDef kind)
		{
			return PawnUtility.GetBaseManhunterOnDamageChance(kind) * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		public static float GetManhunterOnDamageChance(RaceProperties race)
		{
			return race.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		public static float GetManhunterOnTameFailChance(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnTameFailChance(pawn.kindDef);
		}

		public static float GetManhunterOnTameFailChance(PawnKindDef kind)
		{
			return (kind != PawnKindDefOf.WildMan) ? kind.RaceProps.manhunterOnTameFailChance : 0.1f;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PawnUtility()
		{
		}

		[CompilerGenerated]
		private static bool <TryDestroyStartingColonistFamily>m__0(Pawn x)
		{
			return Find.GameInitData.startingAndOptionalPawns.Contains(x);
		}

		[CompilerGenerated]
		private static bool <PawnKindsToCommaList>m__1(Pawn x)
		{
			return !x.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static string <PawnKindsToCommaList>m__2(Pawn x)
		{
			return x.GetKindLabelPlural(-1);
		}

		[CompilerGenerated]
		private static string <PopIntentRecruitDifficulty>m__3(float d)
		{
			return "d=" + d.ToString("F0");
		}

		[CompilerGenerated]
		private static string <PopIntentRecruitDifficulty>m__4(float rv)
		{
			return rv.ToString("F1");
		}

		[CompilerGenerated]
		private static string <PopIntentRecruitDifficulty>m__5(float d, float pi)
		{
			return PawnUtility.PopIntentAdjustedRecruitDifficulty(d, pi).ToStringPercent();
		}

		[CompilerGenerated]
		private sealed class <EnemiesAreNearby>c__AnonStorey1
		{
			internal TraverseParms tp;

			internal Pawn pawn;

			internal bool foundEnemy;

			public <EnemiesAreNearby>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Region from, Region to)
			{
				return to.Allows(this.tp, false);
			}

			internal bool <>m__1(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].HostileTo(this.pawn))
					{
						this.foundEnemy = true;
						return true;
					}
				}
				return this.foundEnemy;
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnedMasteredPawns>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal Pawn master;

			internal List<Pawn> <pawns>__0;

			internal int <i>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpawnedMasteredPawns>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (Current.ProgramState != ProgramState.Playing || master.Faction == null || !master.RaceProps.Humanlike)
					{
						return false;
					}
					if (!master.Spawned)
					{
						return false;
					}
					pawns = master.Map.mapPawns.SpawnedPawnsInFaction(master.Faction);
					i = 0;
					break;
				case 1u:
					IL_10B:
					i++;
					break;
				default:
					return false;
				}
				if (i >= pawns.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (pawns[i].playerSettings != null && pawns[i].playerSettings.Master == master)
					{
						this.$current = pawns[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_10B;
				}
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnUtility.<SpawnedMasteredPawns>c__Iterator0 <SpawnedMasteredPawns>c__Iterator = new PawnUtility.<SpawnedMasteredPawns>c__Iterator0();
				<SpawnedMasteredPawns>c__Iterator.master = master;
				return <SpawnedMasteredPawns>c__Iterator;
			}
		}
	}
}
