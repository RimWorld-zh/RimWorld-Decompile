using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A73 RID: 2675
	public class MentalState : IExposable
	{
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003B65 RID: 15205 RVA: 0x001F6904 File Offset: 0x001F4D04
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003B66 RID: 15206 RVA: 0x001F6920 File Offset: 0x001F4D20
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x001F6940 File Offset: 0x001F4D40
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x001F6956 File Offset: 0x001F4D56
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x001F698D File Offset: 0x001F4D8D
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x001F6990 File Offset: 0x001F4D90
		public virtual void PostEnd()
		{
			if (!this.def.recoveryMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				string text = null;
				try
				{
					text = string.Format(this.def.recoveryMessage, this.pawn.LabelShort);
				}
				catch (Exception arg)
				{
					Log.Error("Exception formatting string: " + arg, false);
				}
				if (!text.NullOrEmpty())
				{
					Messages.Message(text.AdjustedFor(this.pawn).CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x001F6A48 File Offset: 0x001F4E48
		public virtual void MentalStateTick()
		{
			if (this.pawn.IsHashIntervalTick(150))
			{
				this.age += 150;
				if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 150f)))
				{
					this.RecoverFromState();
				}
				else if (this.def.recoverFromSleep && !this.pawn.Awake())
				{
					this.RecoverFromState();
				}
			}
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x001F6B10 File Offset: 0x001F4F10
		public void RecoverFromState()
		{
			if (this.pawn.MentalState != this)
			{
				Log.Error(string.Concat(new object[]
				{
					"Recovered from ",
					this.def,
					" but pawn's mental state is not this, it is ",
					this.pawn.MentalState
				}), false);
			}
			if (!this.pawn.Dead)
			{
				this.pawn.mindState.mentalStateHandler.ClearMentalStateDirect();
				if (this.causedByMood && this.def.moodRecoveryThought != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def.moodRecoveryThought, null);
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			}
			this.PostEnd();
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x001F6C14 File Offset: 0x001F5014
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x001F6C2C File Offset: 0x001F502C
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x001F6C44 File Offset: 0x001F5044
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x001F6C64 File Offset: 0x001F5064
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x001F6C7C File Offset: 0x001F507C
		public virtual string GetBeginLetterText()
		{
			string result;
			if (this.def.beginLetter.NullOrEmpty())
			{
				result = null;
			}
			else
			{
				result = string.Format(this.def.beginLetter, this.pawn.LabelShort).AdjustedFor(this.pawn).CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x001F6CD8 File Offset: 0x001F50D8
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x001F6CDB File Offset: 0x001F50DB
		public virtual void Notify_SlaughteredAnimal()
		{
		}

		// Token: 0x0400256A RID: 9578
		public Pawn pawn;

		// Token: 0x0400256B RID: 9579
		public MentalStateDef def;

		// Token: 0x0400256C RID: 9580
		private int age = 0;

		// Token: 0x0400256D RID: 9581
		public bool causedByMood = false;

		// Token: 0x0400256E RID: 9582
		private const int TickInterval = 150;
	}
}
