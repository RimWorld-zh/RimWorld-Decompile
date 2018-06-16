using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A61 RID: 2657
	public class MentalStateHandler : IExposable
	{
		// Token: 0x06003B20 RID: 15136 RVA: 0x001F5C11 File Offset: 0x001F4011
		public MentalStateHandler()
		{
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x001F5C1A File Offset: 0x001F401A
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x001F5C2C File Offset: 0x001F402C
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x001F5C50 File Offset: 0x001F4050
		public MentalStateDef CurStateDef
		{
			get
			{
				MentalStateDef result;
				if (this.curStateInt == null)
				{
					result = null;
				}
				else
				{
					result = this.curStateInt.def;
				}
				return result;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x001F5C84 File Offset: 0x001F4084
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x001F5CA0 File Offset: 0x001F40A0
		public void ExposeData()
		{
			Scribe_Deep.Look<MentalState>(ref this.curStateInt, "curState", new object[0]);
			Scribe_Values.Look<bool>(ref this.neverFleeIndividual, "neverFleeIndividual", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curStateInt != null)
				{
					this.curStateInt.pawn = this.pawn;
				}
				if (Current.ProgramState != ProgramState.Entry && this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x001F5D34 File Offset: 0x001F4134
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x001F5D40 File Offset: 0x001F4140
		public void MentalStateHandlerTick()
		{
			if (this.curStateInt != null)
			{
				if (this.pawn.Downed)
				{
					Log.Error("In mental state while downed: " + this.pawn, false);
					this.CurState.RecoverFromState();
				}
				else
				{
					this.curStateInt.MentalStateTick();
				}
			}
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x001F5DA0 File Offset: 0x001F41A0
		public bool TryStartMentalState(MentalStateDef stateDef, string reason = null, bool forceWake = false, bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false)
		{
			bool result;
			if ((!this.pawn.Spawned && !this.pawn.IsCaravanMember()) || this.CurStateDef == stateDef || this.pawn.Downed || (!forceWake && !this.pawn.Awake()))
			{
				result = false;
			}
			else if (TutorSystem.TutorialMode && this.pawn.Faction == Faction.OfPlayer)
			{
				result = false;
			}
			else if (!stateDef.Worker.StateCanOccur(this.pawn))
			{
				result = false;
			}
			else
			{
				if (!transitionSilently)
				{
					if ((this.pawn.IsColonist || this.pawn.HostFaction == Faction.OfPlayer) && stateDef.tale != null)
					{
						TaleRecorder.RecordTale(stateDef.tale, new object[]
						{
							this.pawn
						});
					}
					if (stateDef.IsExtreme && this.pawn.IsPlayerControlledCaravanMember())
					{
						Messages.Message("MessageCaravanMemberHasExtremeMentalBreak".Translate(), this.pawn.GetCaravan(), MessageTypeDefOf.ThreatSmall, true);
					}
					this.pawn.records.Increment(RecordDefOf.TimesInMentalState);
				}
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				MentalState mentalState = (MentalState)Activator.CreateInstance(stateDef.stateClass);
				this.curStateInt = mentalState;
				this.curStateInt.pawn = this.pawn;
				this.curStateInt.def = stateDef;
				this.curStateInt.causedByMood = causedByMood;
				if (otherPawn != null)
				{
					((MentalState_SocialFighting)this.curStateInt).otherPawn = otherPawn;
				}
				if (this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
				if (stateDef != null && stateDef.IsAggro && this.pawn.caller != null)
				{
					this.pawn.caller.Notify_InAggroMentalState();
				}
				if (this.curStateInt != null)
				{
					this.curStateInt.PostStart(reason);
				}
				if (this.pawn.CurJob != null)
				{
					this.pawn.jobs.StopAll(false);
				}
				if (this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
				if (this.pawn.Spawned && forceWake && !this.pawn.Awake())
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
				if (!transitionSilently && PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					string text = mentalState.GetBeginLetterText();
					if (!text.NullOrEmpty())
					{
						string label = "MentalBreakLetterLabel".Translate() + ": " + stateDef.beginLetterLabel.CapitalizeFirst();
						if (reason != null)
						{
							text = text + "\n\n" + "FinalStraw".Translate(new object[]
							{
								reason.CapitalizeFirst()
							});
						}
						Find.LetterStack.ReceiveLetter(label, text, stateDef.beginLetterDef, this.pawn, null, null);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x001F6110 File Offset: 0x001F4510
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (!this.neverFleeIndividual && this.pawn.Spawned && this.pawn.MentalStateDef == null && !this.pawn.Downed && dinfo.Def.externalViolence && this.pawn.RaceProps.Humanlike && this.pawn.mindState.canFleeIndividual)
			{
				float lerpPct = (float)(this.pawn.HashOffset() % 100) / 100f;
				float num = this.pawn.kindDef.fleeHealthThresholdRange.LerpThroughRange(lerpPct);
				if (this.pawn.health.summaryHealth.SummaryHealthPercent < num && this.pawn.Faction != Faction.OfPlayer && this.pawn.HostFaction == null)
				{
					this.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false);
				}
			}
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x001F6218 File Offset: 0x001F4618
		internal void ClearMentalStateDirect()
		{
			if (this.curStateInt != null)
			{
				this.curStateInt = null;
				if (this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x04002558 RID: 9560
		private Pawn pawn;

		// Token: 0x04002559 RID: 9561
		private MentalState curStateInt;

		// Token: 0x0400255A RID: 9562
		public bool neverFleeIndividual;
	}
}
