using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A73 RID: 2675
	public class MentalState : IExposable
	{
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x001F69D8 File Offset: 0x001F4DD8
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003B68 RID: 15208 RVA: 0x001F69F4 File Offset: 0x001F4DF4
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003B69 RID: 15209 RVA: 0x001F6A14 File Offset: 0x001F4E14
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x001F6A2A File Offset: 0x001F4E2A
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x001F6A61 File Offset: 0x001F4E61
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x001F6A64 File Offset: 0x001F4E64
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

		// Token: 0x06003B6D RID: 15213 RVA: 0x001F6B1C File Offset: 0x001F4F1C
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

		// Token: 0x06003B6E RID: 15214 RVA: 0x001F6BE4 File Offset: 0x001F4FE4
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

		// Token: 0x06003B6F RID: 15215 RVA: 0x001F6CE8 File Offset: 0x001F50E8
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x001F6D00 File Offset: 0x001F5100
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x001F6D18 File Offset: 0x001F5118
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x001F6D38 File Offset: 0x001F5138
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x001F6D50 File Offset: 0x001F5150
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

		// Token: 0x06003B74 RID: 15220 RVA: 0x001F6DAC File Offset: 0x001F51AC
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x001F6DAF File Offset: 0x001F51AF
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
