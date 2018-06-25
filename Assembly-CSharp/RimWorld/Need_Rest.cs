using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000503 RID: 1283
	public class Need_Rest : Need
	{
		// Token: 0x04000DA0 RID: 3488
		private int lastRestTick = -999;

		// Token: 0x04000DA1 RID: 3489
		private float lastRestEffectiveness = 1f;

		// Token: 0x04000DA2 RID: 3490
		private int ticksAtZero = 0;

		// Token: 0x04000DA3 RID: 3491
		private const float FullSleepHours = 10.5f;

		// Token: 0x04000DA4 RID: 3492
		public const float BaseRestGainPerTick = 3.8095237E-05f;

		// Token: 0x04000DA5 RID: 3493
		private const float BaseRestFallPerTick = 1.58333332E-05f;

		// Token: 0x04000DA6 RID: 3494
		public const float ThreshTired = 0.28f;

		// Token: 0x04000DA7 RID: 3495
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x04000DA8 RID: 3496
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x04000DA9 RID: 3497
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x04000DAA RID: 3498
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04000DAB RID: 3499
		private const float BaseInvoluntarySleepMTBDays = 0.25f;

		// Token: 0x0600170B RID: 5899 RVA: 0x000CB6D8 File Offset: 0x000C9AD8
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x000CB738 File Offset: 0x000C9B38
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
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x000CB794 File Offset: 0x000C9B94
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
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x000CB82C File Offset: 0x000C9C2C
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x000CB858 File Offset: 0x000C9C58
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
		// (get) Token: 0x06001710 RID: 5904 RVA: 0x000CB880 File Offset: 0x000C9C80
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x000CB89C File Offset: 0x000C9C9C
		private bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + 2;
			}
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x000CB8C5 File Offset: 0x000C9CC5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x000CB8E0 File Offset: 0x000C9CE0
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x000CB8F8 File Offset: 0x000C9CF8
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

		// Token: 0x06001715 RID: 5909 RVA: 0x000CBB3B File Offset: 0x000C9F3B
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness > 0f)
			{
				this.lastRestTick = Find.TickManager.TicksGame;
				this.lastRestEffectiveness = restEffectiveness;
			}
		}
	}
}
