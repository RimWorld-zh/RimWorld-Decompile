using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A5F RID: 2655
	public class MentalStateHandler : IExposable
	{
		// Token: 0x04002554 RID: 9556
		private Pawn pawn;

		// Token: 0x04002555 RID: 9557
		private MentalState curStateInt;

		// Token: 0x04002556 RID: 9558
		public bool neverFleeIndividual;

		// Token: 0x06003B21 RID: 15137 RVA: 0x001F610D File Offset: 0x001F450D
		public MentalStateHandler()
		{
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x001F6116 File Offset: 0x001F4516
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x001F6128 File Offset: 0x001F4528
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x001F614C File Offset: 0x001F454C
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

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x001F6180 File Offset: 0x001F4580
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x001F619C File Offset: 0x001F459C
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

		// Token: 0x06003B27 RID: 15143 RVA: 0x001F6230 File Offset: 0x001F4630
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x001F623C File Offset: 0x001F463C
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

		// Token: 0x06003B29 RID: 15145 RVA: 0x001F629C File Offset: 0x001F469C
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

		// Token: 0x06003B2A RID: 15146 RVA: 0x001F660C File Offset: 0x001F4A0C
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

		// Token: 0x06003B2B RID: 15147 RVA: 0x001F6714 File Offset: 0x001F4B14
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
	}
}
