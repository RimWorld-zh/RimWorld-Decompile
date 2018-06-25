using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A8E RID: 2702
	public class Pawn_MindState : IExposable
	{
		// Token: 0x040025AC RID: 9644
		public Pawn pawn;

		// Token: 0x040025AD RID: 9645
		public MentalStateHandler mentalStateHandler;

		// Token: 0x040025AE RID: 9646
		public MentalBreaker mentalBreaker;

		// Token: 0x040025AF RID: 9647
		public InspirationHandler inspirationHandler;

		// Token: 0x040025B0 RID: 9648
		public PriorityWork priorityWork;

		// Token: 0x040025B1 RID: 9649
		private bool activeInt = true;

		// Token: 0x040025B2 RID: 9650
		public JobTag lastJobTag = JobTag.Misc;

		// Token: 0x040025B3 RID: 9651
		public int lastIngestTick = -99999;

		// Token: 0x040025B4 RID: 9652
		public int nextApparelOptimizeTick = -99999;

		// Token: 0x040025B5 RID: 9653
		public ThinkNode lastJobGiver;

		// Token: 0x040025B6 RID: 9654
		public ThinkTreeDef lastJobGiverThinkTree;

		// Token: 0x040025B7 RID: 9655
		public WorkTypeDef lastGivenWorkType;

		// Token: 0x040025B8 RID: 9656
		public bool canFleeIndividual = true;

		// Token: 0x040025B9 RID: 9657
		public int exitMapAfterTick = -99999;

		// Token: 0x040025BA RID: 9658
		public int lastDisturbanceTick = -99999;

		// Token: 0x040025BB RID: 9659
		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		// Token: 0x040025BC RID: 9660
		public Thing knownExploder = null;

		// Token: 0x040025BD RID: 9661
		public bool wantsToTradeWithColony;

		// Token: 0x040025BE RID: 9662
		public Thing lastMannedThing;

		// Token: 0x040025BF RID: 9663
		public int canLovinTick = -99999;

		// Token: 0x040025C0 RID: 9664
		public int canSleepTick = -99999;

		// Token: 0x040025C1 RID: 9665
		public Pawn meleeThreat = null;

		// Token: 0x040025C2 RID: 9666
		public int lastMeleeThreatHarmTick = -99999;

		// Token: 0x040025C3 RID: 9667
		public int lastEngageTargetTick = -99999;

		// Token: 0x040025C4 RID: 9668
		public int lastAttackTargetTick = -99999;

		// Token: 0x040025C5 RID: 9669
		public LocalTargetInfo lastAttackedTarget;

		// Token: 0x040025C6 RID: 9670
		public Thing enemyTarget;

		// Token: 0x040025C7 RID: 9671
		public PawnDuty duty = null;

		// Token: 0x040025C8 RID: 9672
		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		// Token: 0x040025C9 RID: 9673
		public int lastAssignedInteractTime = -99999;

		// Token: 0x040025CA RID: 9674
		public int lastInventoryRawFoodUseTick = 0;

		// Token: 0x040025CB RID: 9675
		public bool nextMoveOrderIsWait = false;

		// Token: 0x040025CC RID: 9676
		public int lastTakeCombatEnhancingDrugTick = -99999;

		// Token: 0x040025CD RID: 9677
		public int lastHarmTick = -99999;

		// Token: 0x040025CE RID: 9678
		public bool anyCloseHostilesRecently;

		// Token: 0x040025CF RID: 9679
		public int applyBedThoughtsTick;

		// Token: 0x040025D0 RID: 9680
		public bool applyBedThoughtsOnLeave;

		// Token: 0x040025D1 RID: 9681
		public bool willJoinColonyIfRescued;

		// Token: 0x040025D2 RID: 9682
		public bool wildManEverReachedOutside;

		// Token: 0x040025D3 RID: 9683
		public int timesGuestTendedToByPlayer;

		// Token: 0x040025D4 RID: 9684
		public int lastSelfTendTick = -99999;

		// Token: 0x040025D5 RID: 9685
		public float maxDistToSquadFlag = -1f;

		// Token: 0x040025D6 RID: 9686
		private int lastJobGiverKey = -1;

		// Token: 0x040025D7 RID: 9687
		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		// Token: 0x040025D8 RID: 9688
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		// Token: 0x040025D9 RID: 9689
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		// Token: 0x040025DA RID: 9690
		private const float HarmForgetDistance = 3f;

		// Token: 0x040025DB RID: 9691
		private const int MeleeHarmForgetDelay = 400;

		// Token: 0x040025DC RID: 9692
		private const int CheckJoinColonyIfRescuedIntervalTicks = 30;

		// Token: 0x06003BE4 RID: 15332 RVA: 0x001F9624 File Offset: 0x001F7A24
		public Pawn_MindState()
		{
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x001F9728 File Offset: 0x001F7B28
		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x001F9864 File Offset: 0x001F7C64
		// (set) Token: 0x06003BE7 RID: 15335 RVA: 0x001F9880 File Offset: 0x001F7C80
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

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003BE8 RID: 15336 RVA: 0x001F98D0 File Offset: 0x001F7CD0
		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x001F991C File Offset: 0x001F7D1C
		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.Downed && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map, false, null, 0, 0);
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x001F99E4 File Offset: 0x001F7DE4
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

		// Token: 0x06003BEB RID: 15339 RVA: 0x001F9B4C File Offset: 0x001F7F4C
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

		// Token: 0x06003BEC RID: 15340 RVA: 0x001F9F00 File Offset: 0x001F8300
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
			if (this.pawn.Spawned)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain.traversedThought != null && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null)
				{
					Thought_Memory firstMemoryOfDef = this.pawn.needs.mood.thoughts.memories.GetFirstMemoryOfDef(terrain.traversedThought);
					if (firstMemoryOfDef != null)
					{
						firstMemoryOfDef.Renew();
					}
					else
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(terrain.traversedThought, null);
					}
				}
			}
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x001FA2B0 File Offset: 0x001F86B0
		private void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.willJoinColonyIfRescued = false;
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, 1f, false);
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), "LetterRescueQuestFinished".Translate().AdjustedFor(this.pawn, "PAWN").CapitalizeFirst(), LetterDefOf.PositiveEvent, this.pawn, null, null);
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x001FA31C File Offset: 0x001F871C
		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x001FA32C File Offset: 0x001F872C
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

		// Token: 0x06003BF0 RID: 15344 RVA: 0x001FA356 File Offset: 0x001F8756
		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x001FA36C File Offset: 0x001F876C
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

		// Token: 0x06003BF2 RID: 15346 RVA: 0x001FA3AC File Offset: 0x001F87AC
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

		// Token: 0x06003BF3 RID: 15347 RVA: 0x001FA55C File Offset: 0x001F895C
		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x001FA56F File Offset: 0x001F896F
		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x001FA58C File Offset: 0x001F898C
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

		// Token: 0x06003BF6 RID: 15350 RVA: 0x001FA626 File Offset: 0x001F8A26
		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x001FA658 File Offset: 0x001F8A58
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

		// Token: 0x06003BF8 RID: 15352 RVA: 0x001FA6D2 File Offset: 0x001F8AD2
		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.wildManEverReachedOutside = false;
			}
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x001FA6EC File Offset: 0x001F8AEC
		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x001FA700 File Offset: 0x001F8B00
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

		// Token: 0x06003BFB RID: 15355 RVA: 0x001FA734 File Offset: 0x001F8B34
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

		// Token: 0x06003BFC RID: 15356 RVA: 0x001FA94C File Offset: 0x001F8D4C
		private static bool CanStartFleeingBecauseOfPawnAction(Pawn p)
		{
			return p.RaceProps.Animal && !p.InMentalState && !p.IsFighting() && !p.Downed && !p.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p);
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x001FA9AC File Offset: 0x001F8DAC
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
	}
}
