using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004EC RID: 1260
	public class Pawn_GuestTracker : IExposable
	{
		// Token: 0x04000D25 RID: 3365
		private Pawn pawn;

		// Token: 0x04000D26 RID: 3366
		private bool getsFoodInt = true;

		// Token: 0x04000D27 RID: 3367
		public PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.NoInteraction;

		// Token: 0x04000D28 RID: 3368
		private Faction hostFactionInt = null;

		// Token: 0x04000D29 RID: 3369
		public bool isPrisonerInt = false;

		// Token: 0x04000D2A RID: 3370
		private bool releasedInt = false;

		// Token: 0x04000D2B RID: 3371
		private int ticksWhenAllowedToEscapeAgain;

		// Token: 0x04000D2C RID: 3372
		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		// Token: 0x04000D2D RID: 3373
		public int lastPrisonBreakTicks = -1;

		// Token: 0x04000D2E RID: 3374
		public bool everParticipatedInPrisonBreak;

		// Token: 0x04000D2F RID: 3375
		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		// Token: 0x04000D30 RID: 3376
		public int MinInteractionInterval = 7500;

		// Token: 0x04000D31 RID: 3377
		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		// Token: 0x06001685 RID: 5765 RVA: 0x000C7EE0 File Offset: 0x000C62E0
		public Pawn_GuestTracker()
		{
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000C7F38 File Offset: 0x000C6338
		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x000C7F98 File Offset: 0x000C6398
		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x000C7FB4 File Offset: 0x000C63B4
		// (set) Token: 0x06001689 RID: 5769 RVA: 0x000C7FED File Offset: 0x000C63ED
		public bool GetsFood
		{
			get
			{
				bool result;
				if (this.HostFaction == null)
				{
					Log.Error("GetsFood without host faction.", false);
					result = true;
				}
				else
				{
					result = this.getsFoodInt;
				}
				return result;
			}
			set
			{
				this.getsFoodInt = value;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x000C7FF8 File Offset: 0x000C63F8
		public bool CanBeBroughtFood
		{
			get
			{
				return this.GetsFood && this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600168B RID: 5771 RVA: 0x000C804C File Offset: 0x000C644C
		public bool IsPrisoner
		{
			get
			{
				return this.isPrisonerInt;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x000C8068 File Offset: 0x000C6468
		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - this.MinInteractionInterval;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x000C80A0 File Offset: 0x000C64A0
		// (set) Token: 0x0600168E RID: 5774 RVA: 0x000C80BB File Offset: 0x000C64BB
		public bool Released
		{
			get
			{
				return this.releasedInt;
			}
			set
			{
				if (value != this.releasedInt)
				{
					this.releasedInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x000C80FC File Offset: 0x000C64FC
		public bool PrisonerIsSecure
		{
			get
			{
				bool result;
				if (this.Released)
				{
					result = false;
				}
				else if (this.pawn.HostFaction == null)
				{
					result = false;
				}
				else if (this.pawn.InMentalState)
				{
					result = false;
				}
				else
				{
					if (this.pawn.Spawned)
					{
						if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
						{
							return false;
						}
						if (PrisonBreakUtility.IsPrisonBreaking(this.pawn))
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x000C81B4 File Offset: 0x000C65B4
		public bool ShouldWaitInsteadOfEscaping
		{
			get
			{
				bool result;
				if (!this.IsPrisoner)
				{
					result = false;
				}
				else
				{
					Map mapHeld = this.pawn.MapHeld;
					result = (mapHeld != null && mapHeld.mapPawns.FreeColonistsSpawnedCount != 0 && Find.TickManager.TicksGame < this.ticksWhenAllowedToEscapeAgain);
				}
				return result;
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000C8220 File Offset: 0x000C6620
		public void GuestTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(2500))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x000C827C File Offset: 0x000C667C
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_Values.Look<bool>(ref this.isPrisonerInt, "prisoner", false, false);
			Scribe_Values.Look<bool>(ref this.getsFoodInt, "getsFood", false, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000C8334 File Offset: 0x000C6734
		public void SetGuestStatus(Faction newHost, bool prisoner = false)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			if (newHost != this.HostFaction || prisoner != this.IsPrisoner)
			{
				if (!prisoner && this.pawn.Faction.HostileTo(newHost))
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to make ",
						this.pawn,
						" a guest of ",
						newHost,
						" but their faction ",
						this.pawn.Faction,
						" is hostile to ",
						newHost
					}), false);
				}
				else if (newHost != null && newHost == this.pawn.Faction && !prisoner)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to make ",
						this.pawn,
						" a guest of their own faction ",
						this.pawn.Faction
					}), false);
				}
				else
				{
					bool flag = prisoner && (!this.IsPrisoner || this.HostFaction != newHost);
					this.isPrisonerInt = prisoner;
					this.hostFactionInt = newHost;
					this.pawn.ClearMind(false);
					if (flag)
					{
						this.pawn.DropAndForbidEverything(false);
						Lord lord = this.pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(this.pawn, PawnLostCondition.MadePrisoner);
						}
						if (this.pawn.Drafted)
						{
							this.pawn.drafter.Drafted = false;
						}
					}
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
					this.pawn.health.surgeryBills.Clear();
					if (this.pawn.ownership != null)
					{
						this.pawn.ownership.Notify_ChangedGuestStatus();
					}
					ReachabilityUtility.ClearCache();
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
						this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
					}
					AddictionUtility.CheckDrugAddictionTeachOpportunity(this.pawn);
					if (prisoner && this.pawn.playerSettings != null)
					{
						this.pawn.playerSettings.Notify_MadePrisoner();
					}
				}
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000C858C File Offset: 0x000C698C
		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			if (this.pawn.Faction != null)
			{
				this.pawn.Faction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, true);
			if (this.IsPrisoner && byPawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
				{
					byPawn,
					this.pawn
				});
				byPawn.records.Increment(RecordDefOf.PeopleCaptured);
			}
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x000C8609 File Offset: 0x000C6A09
		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x000C8617 File Offset: 0x000C6A17
		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (this.IsPrisoner)
			{
				this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
				this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
			}
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x000C8648 File Offset: 0x000C6A48
		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && this.HostFaction == Faction.OfPlayer && (this.pawn.Faction == null || this.pawn.Faction.def.rescueesCanJoin) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned)
			{
				Map mapHeld = this.pawn.MapHeld;
				float num;
				if (!this.pawn.SafeTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp) || mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
				{
					num = 1f;
				}
				else
				{
					num = 0.5f;
				}
				if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 8976612) < num)
				{
					this.pawn.SetFaction(Faction.OfPlayer, null);
					Messages.Message("MessageRescueeJoined".Translate(new object[]
					{
						this.pawn.LabelShort
					}).AdjustedFor(this.pawn, "PAWN"), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}
	}
}
