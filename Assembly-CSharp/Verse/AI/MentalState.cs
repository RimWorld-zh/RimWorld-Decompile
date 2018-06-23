using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6F RID: 2671
	public class MentalState : IExposable
	{
		// Token: 0x04002565 RID: 9573
		public Pawn pawn;

		// Token: 0x04002566 RID: 9574
		public MentalStateDef def;

		// Token: 0x04002567 RID: 9575
		private int age = 0;

		// Token: 0x04002568 RID: 9576
		public bool causedByMood = false;

		// Token: 0x04002569 RID: 9577
		private const int TickInterval = 150;

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003B62 RID: 15202 RVA: 0x001F6CD4 File Offset: 0x001F50D4
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003B63 RID: 15203 RVA: 0x001F6CF0 File Offset: 0x001F50F0
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003B64 RID: 15204 RVA: 0x001F6D10 File Offset: 0x001F5110
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x001F6D26 File Offset: 0x001F5126
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x001F6D5D File Offset: 0x001F515D
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x001F6D60 File Offset: 0x001F5160
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
					Messages.Message(text.AdjustedFor(this.pawn, "PAWN").CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x001F6E1C File Offset: 0x001F521C
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

		// Token: 0x06003B69 RID: 15209 RVA: 0x001F6EE4 File Offset: 0x001F52E4
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

		// Token: 0x06003B6A RID: 15210 RVA: 0x001F6FE8 File Offset: 0x001F53E8
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x001F7000 File Offset: 0x001F5400
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x001F7018 File Offset: 0x001F5418
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x001F7038 File Offset: 0x001F5438
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x001F7050 File Offset: 0x001F5450
		public virtual string GetBeginLetterText()
		{
			string result;
			if (this.def.beginLetter.NullOrEmpty())
			{
				result = null;
			}
			else
			{
				result = string.Format(this.def.beginLetter, this.pawn.LabelShort).AdjustedFor(this.pawn, "PAWN").CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x001F70B1 File Offset: 0x001F54B1
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x001F70B4 File Offset: 0x001F54B4
		public virtual void Notify_SlaughteredAnimal()
		{
		}
	}
}
