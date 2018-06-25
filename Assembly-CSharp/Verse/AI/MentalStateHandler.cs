using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	public class MentalStateHandler : IExposable
	{
		private Pawn pawn;

		private MentalState curStateInt;

		public bool neverFleeIndividual;

		public MentalStateHandler()
		{
		}

		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

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

		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

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

		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

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
