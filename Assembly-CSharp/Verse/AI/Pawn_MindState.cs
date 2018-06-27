using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	public class Pawn_MindState : IExposable
	{
		public Pawn pawn;

		public MentalStateHandler mentalStateHandler;

		public MentalBreaker mentalBreaker;

		public InspirationHandler inspirationHandler;

		public PriorityWork priorityWork;

		private bool activeInt = true;

		public JobTag lastJobTag = JobTag.Misc;

		public int lastIngestTick = -99999;

		public int nextApparelOptimizeTick = -99999;

		public ThinkNode lastJobGiver;

		public ThinkTreeDef lastJobGiverThinkTree;

		public WorkTypeDef lastGivenWorkType;

		public bool canFleeIndividual = true;

		public int exitMapAfterTick = -99999;

		public int lastDisturbanceTick = -99999;

		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		public Thing knownExploder = null;

		public bool wantsToTradeWithColony;

		public Thing lastMannedThing;

		public int canLovinTick = -99999;

		public int canSleepTick = -99999;

		public Pawn meleeThreat = null;

		public int lastMeleeThreatHarmTick = -99999;

		public int lastEngageTargetTick = -99999;

		public int lastAttackTargetTick = -99999;

		public LocalTargetInfo lastAttackedTarget;

		public Thing enemyTarget;

		public PawnDuty duty = null;

		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		public int lastAssignedInteractTime = -99999;

		public int lastInventoryRawFoodUseTick = 0;

		public bool nextMoveOrderIsWait = false;

		public int lastTakeCombatEnhancingDrugTick = -99999;

		public int lastHarmTick = -99999;

		public bool anyCloseHostilesRecently;

		public int applyBedThoughtsTick;

		public bool applyBedThoughtsOnLeave;

		public bool willJoinColonyIfRescued;

		public bool wildManEverReachedOutside;

		public int timesGuestTendedToByPlayer;

		public int lastSelfTendTick = -99999;

		public float maxDistToSquadFlag = -1f;

		private int lastJobGiverKey = -1;

		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		private const float HarmForgetDistance = 3f;

		private const int MeleeHarmForgetDelay = 400;

		private const int CheckJoinColonyIfRescuedIntervalTicks = 30;

		public Pawn_MindState()
		{
		}

		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		public bool Active
		{
			get
			{
				return this.activeInt;
			}
			set
			{
				if (value != this.activeInt)
				{
					this.activeInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
					}
				}
			}
		}

		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.Downed && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map, false, null, 0, 0);
			}
		}

		public void Reset()
		{
			this.mentalStateHandler.Reset();
			this.mentalBreaker.Reset();
			this.inspirationHandler.Reset();
			this.activeInt = true;
			this.lastJobTag = JobTag.Misc;
			this.lastIngestTick = -99999;
			this.nextApparelOptimizeTick = -99999;
			this.lastJobGiver = null;
			this.lastJobGiverThinkTree = null;
			this.lastGivenWorkType = null;
			this.canFleeIndividual = true;
			this.exitMapAfterTick = -99999;
			this.lastDisturbanceTick = -99999;
			this.forcedGotoPosition = IntVec3.Invalid;
			this.knownExploder = null;
			this.wantsToTradeWithColony = false;
			this.lastMannedThing = null;
			this.canLovinTick = -99999;
			this.canSleepTick = -99999;
			this.meleeThreat = null;
			this.lastMeleeThreatHarmTick = -99999;
			this.lastEngageTargetTick = -99999;
			this.lastAttackTargetTick = -99999;
			this.lastAttackedTarget = LocalTargetInfo.Invalid;
			this.enemyTarget = null;
			this.duty = null;
			this.thinkData.Clear();
			this.lastAssignedInteractTime = -99999;
			this.lastInventoryRawFoodUseTick = 0;
			this.priorityWork.Clear();
			this.nextMoveOrderIsWait = true;
			this.lastTakeCombatEnhancingDrugTick = -99999;
			this.lastHarmTick = -99999;
			this.anyCloseHostilesRecently = false;
			this.willJoinColonyIfRescued = false;
			this.wildManEverReachedOutside = false;
			this.timesGuestTendedToByPlayer = 0;
			this.lastSelfTendTick = -99999;
		}

		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.lastJobGiverKey = ((this.lastJobGiver == null) ? -1 : this.lastJobGiver.UniqueSaveKey);
			}
			Scribe_Values.Look<int>(ref this.lastJobGiverKey, "lastJobGiverKey", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.lastJobGiverKey != -1)
				{
					if (!this.lastJobGiverThinkTree.TryGetThinkNodeWithSaveKey(this.lastJobGiverKey, out this.lastJobGiver))
					{
						Log.Warning("Could not find think node with key " + this.lastJobGiverKey, false);
					}
				}
			}
			Scribe_References.Look<Pawn>(ref this.meleeThreat, "meleeThreat", false);
			Scribe_References.Look<Thing>(ref this.enemyTarget, "enemyTarget", false);
			Scribe_References.Look<Thing>(ref this.knownExploder, "knownExploder", false);
			Scribe_References.Look<Thing>(ref this.lastMannedThing, "lastMannedThing", false);
			Scribe_Defs.Look<ThinkTreeDef>(ref this.lastJobGiverThinkTree, "lastJobGiverThinkTree");
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Collections.Look<int, int>(ref this.thinkData, "thinkData", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<bool>(ref this.activeInt, "active", true, false);
			Scribe_Values.Look<JobTag>(ref this.lastJobTag, "lastJobTag", JobTag.Misc, false);
			Scribe_Values.Look<int>(ref this.lastIngestTick, "lastIngestTick", -99999, false);
			Scribe_Values.Look<int>(ref this.nextApparelOptimizeTick, "nextApparelOptimizeTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastEngageTargetTick, "lastEngageTargetTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canFleeIndividual, "canFleeIndividual", false, false);
			Scribe_Values.Look<int>(ref this.exitMapAfterTick, "exitMapAfterTick", -99999, false);
			Scribe_Values.Look<IntVec3>(ref this.forcedGotoPosition, "forcedGotoPosition", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.lastMeleeThreatHarmTick, "lastMeleeThreatHarmTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAssignedInteractTime, "lastAssignedInteractTime", -99999, false);
			Scribe_Values.Look<int>(ref this.lastInventoryRawFoodUseTick, "lastInventoryRawFoodUseTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastDisturbanceTick, "lastDisturbanceTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.wantsToTradeWithColony, "wantsToTradeWithColony", false, false);
			Scribe_Values.Look<int>(ref this.canLovinTick, "canLovinTick", -99999, false);
			Scribe_Values.Look<int>(ref this.canSleepTick, "canSleepTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.nextMoveOrderIsWait, "nextMoveOrderIsWait", true, false);
			Scribe_Values.Look<int>(ref this.lastTakeCombatEnhancingDrugTick, "lastTakeCombatEnhancingDrugTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastHarmTick, "lastHarmTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.anyCloseHostilesRecently, "anyCloseHostilesRecently", false, false);
			Scribe_Deep.Look<PawnDuty>(ref this.duty, "duty", new object[0]);
			Scribe_Deep.Look<MentalStateHandler>(ref this.mentalStateHandler, "mentalStateHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalBreaker>(ref this.mentalBreaker, "mentalBreaker", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<InspirationHandler>(ref this.inspirationHandler, "inspirationHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PriorityWork>(ref this.priorityWork, "priorityWork", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.applyBedThoughtsTick, "applyBedThoughtsTick", 0, false);
			Scribe_Values.Look<bool>(ref this.applyBedThoughtsOnLeave, "applyBedThoughtsOnLeave", false, false);
			Scribe_Values.Look<bool>(ref this.willJoinColonyIfRescued, "willJoinColonyIfRescued", false, false);
			Scribe_Values.Look<bool>(ref this.wildManEverReachedOutside, "wildManEverReachedOutside", false, false);
			Scribe_Values.Look<int>(ref this.timesGuestTendedToByPlayer, "timesGuestTendedToByPlayer", 0, false);
			Scribe_Values.Look<int>(ref this.lastSelfTendTick, "lastSelfTendTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.MindStatePostLoadInit(this);
			}
		}

		public void MindStateTick()
		{
			if (this.wantsToTradeWithColony)
			{
				TradeUtility.CheckInteractWithTradersTeachOpportunity(this.pawn);
			}
			if (this.meleeThreat != null && !this.MeleeThreatStillThreat)
			{
				this.meleeThreat = null;
			}
			this.mentalStateHandler.MentalStateHandlerTick();
			this.mentalBreaker.MentalBreakerTick();
			this.inspirationHandler.InspirationHandlerTick();
			if (!this.pawn.GetPosture().Laying())
			{
				this.applyBedThoughtsTick = 0;
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				if (this.pawn.Spawned)
				{
					int regionsToScan = (!this.anyCloseHostilesRecently) ? 18 : 24;
					this.anyCloseHostilesRecently = PawnUtility.EnemiesAreNearby(this.pawn, regionsToScan, true);
				}
				else
				{
					this.anyCloseHostilesRecently = false;
				}
			}
			if (this.willJoinColonyIfRescued && this.pawn.Spawned && this.pawn.IsHashIntervalTick(30))
			{
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					this.willJoinColonyIfRescued = false;
				}
				else if (this.pawn.IsPrisoner && !this.pawn.HostFaction.HostileTo(Faction.OfPlayer))
				{
					this.willJoinColonyIfRescued = false;
				}
				else if (!this.pawn.IsPrisoner && this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer) && !this.pawn.Downed)
				{
					this.willJoinColonyIfRescued = false;
				}
				else
				{
					foreach (Pawn pawn in this.pawn.Map.mapPawns.FreeColonistsSpawned)
					{
						if (pawn.IsColonistPlayerControlled && pawn.Position.InHorDistOf(this.pawn.Position, 4f) && GenSight.LineOfSight(this.pawn.Position, pawn.Position, this.pawn.Map, false, null, 0, 0))
						{
							this.JoinColonyBecauseRescuedBy(pawn);
							break;
						}
					}
				}
			}
			if (this.pawn.Spawned && this.pawn.IsWildMan() && !this.wildManEverReachedOutside && this.pawn.GetRoom(RegionType.Set_Passable) != null && this.pawn.GetRoom(RegionType.Set_Passable).TouchesMapEdge)
			{
				this.wildManEverReachedOutside = true;
				this.pawn.Map.reachability.ClearCache();
			}
			if (Find.TickManager.TicksGame % 123 == 0)
			{
				if (this.pawn.Spawned && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null)
				{
					TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
					if (terrain.traversedThought != null)
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(terrain.traversedThought);
					}
					WeatherDef curWeatherLerped = this.pawn.Map.weatherManager.CurWeatherLerped;
					if (curWeatherLerped.exposedThought != null && !this.pawn.Position.Roofed(this.pawn.Map))
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(curWeatherLerped.exposedThought);
					}
				}
			}
		}

		private void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.willJoinColonyIfRescued = false;
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, 1f, false);
			if (this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), "LetterRescueQuestFinished".Translate().AdjustedFor(this.pawn, "PAWN").CapitalizeFirst(), LetterDefOf.PositiveEvent, this.pawn, null, null);
		}

		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.IsColonistPlayerControlled)
			{
				foreach (Gizmo g in this.priorityWork.GetGizmos())
				{
					yield return g;
				}
			}
			foreach (Gizmo g2 in CaravanFormingUtility.GetGizmos(this.pawn))
			{
				yield return g2;
			}
			yield break;
		}

		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		public void Notify_WorkPriorityDisabled(WorkTypeDef wType)
		{
			JobGiver_Work jobGiver_Work = this.lastJobGiver as JobGiver_Work;
			if (jobGiver_Work != null)
			{
				if (this.lastGivenWorkType == wType)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			}
		}

		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			this.mentalStateHandler.Notify_DamageTaken(dinfo);
			if (dinfo.Def.externalViolence)
			{
				this.lastHarmTick = Find.TickManager.TicksGame;
				if (this.pawn.Spawned)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (!this.mentalStateHandler.InMentalState && dinfo.Instigator != null && (pawn != null || dinfo.Instigator is Building_Turret) && dinfo.Instigator.Faction != null && (dinfo.Instigator.Faction.def.humanlikeFaction || (pawn != null && pawn.def.race.intelligence >= Intelligence.ToolUser)) && this.pawn.Faction == null && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.PredatorHunt || dinfo.Instigator != ((JobDriver_PredatorHunt)this.pawn.jobs.curDriver).Prey) && Rand.Chance(PawnUtility.GetManhunterOnDamageChance(this.pawn, dinfo.Instigator)))
					{
						this.StartManhunterBecauseOfPawnAction("AnimalManhunterFromDamage");
					}
					else if (dinfo.Instigator != null && Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
					{
						this.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
					}
				}
				if (this.pawn.GetPosture() != PawnPosture.Standing)
				{
					this.lastDisturbanceTick = Find.TickManager.TicksGame;
				}
			}
		}

		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		internal bool CheckStartMentalStateBecauseRecruitAttempted(Pawn tamer)
		{
			bool result;
			if (!this.pawn.RaceProps.Animal && (!this.pawn.IsWildMan() || this.pawn.IsPrisoner))
			{
				result = false;
			}
			else if (!this.mentalStateHandler.InMentalState && this.pawn.Faction == null && Rand.Value < PawnUtility.GetManhunterOnTameFailChance(this.pawn))
			{
				this.StartManhunterBecauseOfPawnAction("AnimalManhunterFromTaming");
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		public void Notify_Explosion(Explosion explosion)
		{
			if (this.pawn.Faction == null)
			{
				if (explosion.radius >= 3.5f && this.pawn.Position.InHorDistOf(explosion.Position, explosion.radius + 7f))
				{
					if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
					{
						this.StartFleeingBecauseOfPawnAction(explosion);
					}
				}
			}
		}

		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.wildManEverReachedOutside = false;
			}
		}

		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		private IEnumerable<Pawn> GetPackmates(Pawn pawn, float radius)
		{
			Room pawnRoom = pawn.GetRoom(RegionType.Set_Passable);
			List<Pawn> raceMates = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < raceMates.Count; i++)
			{
				if (pawn != raceMates[i] && raceMates[i].def == pawn.def && raceMates[i].Faction == pawn.Faction && raceMates[i].Position.InHorDistOf(pawn.Position, radius) && raceMates[i].GetRoom(RegionType.Set_Passable) == pawnRoom)
				{
					yield return raceMates[i];
				}
			}
			yield break;
		}

		private void StartManhunterBecauseOfPawnAction(string letterTextKey)
		{
			if (this.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
			{
				string text = letterTextKey.Translate(new object[]
				{
					this.pawn.Label
				});
				GlobalTargetInfo target = this.pawn;
				int num = 1;
				if (Find.Storyteller.difficulty.allowBigThreats && Rand.Value < 0.5f)
				{
					foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
					{
						if (pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
						{
							num++;
						}
					}
					if (num > 1)
					{
						target = new TargetInfo(this.pawn.Position, this.pawn.Map, false);
						text += "\n\n";
						text += ((!"AnimalManhunterOthers".CanTranslate()) ? "AnimalManhunterFromDamageOthers".Translate(new object[]
						{
							this.pawn.def.label
						}) : "AnimalManhunterOthers".Translate(new object[]
						{
							this.pawn.kindDef.GetLabelPlural(-1)
						}));
					}
				}
				string label = (!"LetterLabelAnimalManhunterRevenge".CanTranslate()) ? "LetterLabelAnimalManhunterFromDamage".Translate(new object[]
				{
					this.pawn.Label
				}).CapitalizeFirst() : "LetterLabelAnimalManhunterRevenge".Translate(new object[]
				{
					this.pawn.Label
				}).CapitalizeFirst();
				Find.LetterStack.ReceiveLetter(label, text, (num != 1) ? LetterDefOf.ThreatBig : LetterDefOf.ThreatSmall, target, null, null);
			}
		}

		private static bool CanStartFleeingBecauseOfPawnAction(Pawn p)
		{
			return p.RaceProps.Animal && !p.InMentalState && !p.IsFighting() && !p.Downed && !p.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p);
		}

		public void StartFleeingBecauseOfPawnAction(Thing instigator)
		{
			List<Thing> threats = new List<Thing>
			{
				instigator
			};
			IntVec3 fleeDest = CellFinderLoose.GetFleeDest(this.pawn, threats, this.pawn.Position.DistanceTo(instigator.Position) + 14f);
			if (fleeDest != this.pawn.Position)
			{
				this.pawn.jobs.StartJob(new Job(JobDefOf.Flee, fleeDest, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false);
			}
			if (this.pawn.RaceProps.herdAnimal && Rand.Chance(0.1f))
			{
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(pawn))
					{
						IntVec3 fleeDest2 = CellFinderLoose.GetFleeDest(pawn, threats, pawn.Position.DistanceTo(instigator.Position) + 14f);
						if (fleeDest2 != pawn.Position)
						{
							pawn.jobs.StartJob(new Job(JobDefOf.Flee, fleeDest2, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false);
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal IEnumerator<Gizmo> $locvar1;

			internal Gizmo <g>__2;

			internal Pawn_MindState $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (!this.pawn.IsColonistPlayerControlled)
					{
						goto IL_D4;
					}
					enumerator = this.priorityWork.GetGizmos().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					Block_4:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							g2 = enumerator2.Current;
							this.$current = g2;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IL_D4:
				enumerator2 = CaravanFormingUtility.GetGizmos(this.pawn).GetEnumerator();
				num = 4294967293u;
				goto Block_4;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn_MindState.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Pawn_MindState.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetPackmates>c__Iterator1 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal Pawn pawn;

			internal Room <pawnRoom>__0;

			internal List<Pawn> <raceMates>__0;

			internal int <i>__1;

			internal float radius;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetPackmates>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					pawnRoom = pawn.GetRoom(RegionType.Set_Passable);
					raceMates = pawn.Map.mapPawns.AllPawnsSpawned;
					i = 0;
					goto IL_156;
				case 1u:
					break;
				default:
					return false;
				}
				IL_147:
				i++;
				IL_156:
				if (i >= raceMates.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (pawn != raceMates[i] && raceMates[i].def == pawn.def && raceMates[i].Faction == pawn.Faction && raceMates[i].Position.InHorDistOf(pawn.Position, radius) && raceMates[i].GetRoom(RegionType.Set_Passable) == pawnRoom)
					{
						this.$current = raceMates[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_147;
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
				Pawn_MindState.<GetPackmates>c__Iterator1 <GetPackmates>c__Iterator = new Pawn_MindState.<GetPackmates>c__Iterator1();
				<GetPackmates>c__Iterator.pawn = pawn;
				<GetPackmates>c__Iterator.radius = radius;
				return <GetPackmates>c__Iterator;
			}
		}
	}
}
