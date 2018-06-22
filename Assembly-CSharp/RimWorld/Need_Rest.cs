using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000501 RID: 1281
	public class Need_Rest : Need
	{
		// Token: 0x06001708 RID: 5896 RVA: 0x000CB388 File Offset: 0x000C9788
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x000CB3E8 File Offset: 0x000C97E8
		public RestCategory CurCategory
		{
			get
			{
				RestCategory result;
				if (this.CurLevel < 0.01f)
				{
					result = RestCategory.Exhausted;
				}
				else if (this.CurLevel < 0.14f)
				{
					result = RestCategory.VeryTired;
				}
				else if (this.CurLevel < 0.28f)
				{
					result = RestCategory.Tired;
				}
				else
				{
					result = RestCategory.Rested;
				}
				return result;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x000CB444 File Offset: 0x000C9844
		public float RestFallPerTick
		{
			get
			{
				float result;
				switch (this.CurCategory)
				{
				case RestCategory.Rested:
					result = 1.58333332E-05f * this.RestFallFactor;
					break;
				case RestCategory.Tired:
					result = 1.58333332E-05f * this.RestFallFactor * 0.7f;
					break;
				case RestCategory.VeryTired:
					result = 1.58333332E-05f * this.RestFallFactor * 0.3f;
					break;
				case RestCategory.Exhausted:
					result = 1.58333332E-05f * this.RestFallFactor * 0.6f;
					break;
				default:
					result = 999f;
					break;
				}
				return result;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x000CB4DC File Offset: 0x000C98DC
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x000CB508 File Offset: 0x000C9908
		public override int GUIChangeArrow
		{
			get
			{
				int result;
				if (this.Resting)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
				return result;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x000CB530 File Offset: 0x000C9930
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x000CB54C File Offset: 0x000C994C
		private bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + 2;
			}
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x000CB575 File Offset: 0x000C9975
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x000CB590 File Offset: 0x000C9990
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x000CB5A8 File Offset: 0x000C99A8
		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				if (this.Resting)
				{
					float num = this.lastRestEffectiveness;
					float num2 = RestUtility.PawnHealthRestEffectivenessFactor(this.pawn);
					num = 0.7f * num + 0.3f * num * num2;
					num *= this.pawn.GetStatValue(StatDefOf.RestRateMultiplier, true);
					if (num > 0f)
					{
						this.CurLevel += 0.00571428565f * num;
					}
				}
				else
				{
					this.CurLevel -= this.RestFallPerTick * 150f;
				}
			}
			if (this.CurLevel < 0.0001f)
			{
				this.ticksAtZero += 150;
			}
			else
			{
				this.ticksAtZero = 0;
			}
			if (this.ticksAtZero > 1000 && this.pawn.Spawned)
			{
				float mtb;
				if (this.ticksAtZero < 15000)
				{
					mtb = 0.25f;
				}
				else if (this.ticksAtZero < 30000)
				{
					mtb = 0.125f;
				}
				else if (this.ticksAtZero < 45000)
				{
					mtb = 0.0833333358f;
				}
				else
				{
					mtb = 0.0625f;
				}
				if (Rand.MTBEventOccurs(mtb, 60000f, 150f) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown))
				{
					this.pawn.jobs.StartJob(new Job(JobDefOf.LayDown, this.pawn.Position), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false);
					if (this.pawn.InMentalState)
					{
						this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
					}
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageInvoluntarySleep".Translate(new object[]
						{
							this.pawn.LabelShort
						}), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					TaleRecorder.RecordTale(TaleDefOf.Exhausted, new object[]
					{
						this.pawn
					});
				}
			}
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x000CB7EB File Offset: 0x000C9BEB
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness > 0f)
			{
				this.lastRestTick = Find.TickManager.TicksGame;
				this.lastRestEffectiveness = restEffectiveness;
			}
		}

		// Token: 0x04000D9D RID: 3485
		private int lastRestTick = -999;

		// Token: 0x04000D9E RID: 3486
		private float lastRestEffectiveness = 1f;

		// Token: 0x04000D9F RID: 3487
		private int ticksAtZero = 0;

		// Token: 0x04000DA0 RID: 3488
		private const float FullSleepHours = 10.5f;

		// Token: 0x04000DA1 RID: 3489
		public const float BaseRestGainPerTick = 3.8095237E-05f;

		// Token: 0x04000DA2 RID: 3490
		private const float BaseRestFallPerTick = 1.58333332E-05f;

		// Token: 0x04000DA3 RID: 3491
		public const float ThreshTired = 0.28f;

		// Token: 0x04000DA4 RID: 3492
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x04000DA5 RID: 3493
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x04000DA6 RID: 3494
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x04000DA7 RID: 3495
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04000DA8 RID: 3496
		private const float BaseInvoluntarySleepMTBDays = 0.25f;
	}
}
