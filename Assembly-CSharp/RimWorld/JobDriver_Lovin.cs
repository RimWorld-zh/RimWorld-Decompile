using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000072 RID: 114
	public class JobDriver_Lovin : JobDriver
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00021D64 File Offset: 0x00020164
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00021D94 File Offset: 0x00020194
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00021DC4 File Offset: 0x000201C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00021DE0 File Offset: 0x000201E0
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00021E48 File Offset: 0x00020248
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00021E7C File Offset: 0x0002027C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn(() => !this.Partner.health.capacities.CanBeAwake);
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(this.BedInd);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin)
					{
						Job newJob = new Job(JobDefOf.Lovin, this.pawn, this.Bed);
						this.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);
						this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
					}
					else
					{
						this.ticksLeft = 9999999;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil doLovin = Toils_LayDown.LayDown(this.BedInd, true, false, false, false);
			doLovin.FailOn(() => this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin);
			doLovin.AddPreTickAction(delegate
			{
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
				}
				else if (this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			});
			doLovin.AddFinishAction(delegate
			{
				Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, this.Partner);
				this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + this.GenerateRandomMinTicksToNextLovin(this.pawn);
			});
			doLovin.socialMode = RandomSocialMode.Off;
			yield return doLovin;
			yield break;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00021EA8 File Offset: 0x000202A8
		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			int result;
			if (DebugSettings.alwaysDoLovin)
			{
				result = 100;
			}
			else
			{
				float num = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
				num = Rand.Gaussian(num, 0.3f);
				if (num < 0.5f)
				{
					num = 0.5f;
				}
				result = (int)(num * 2500f);
			}
			return result;
		}

		// Token: 0x04000219 RID: 537
		private int ticksLeft = 0;

		// Token: 0x0400021A RID: 538
		private TargetIndex PartnerInd = TargetIndex.A;

		// Token: 0x0400021B RID: 539
		private TargetIndex BedInd = TargetIndex.B;

		// Token: 0x0400021C RID: 540
		private const int TicksBetweenHeartMotes = 100;

		// Token: 0x0400021D RID: 541
		private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
		{
			{
				new CurvePoint(16f, 1.5f),
				true
			},
			{
				new CurvePoint(22f, 1.5f),
				true
			},
			{
				new CurvePoint(30f, 4f),
				true
			},
			{
				new CurvePoint(50f, 12f),
				true
			},
			{
				new CurvePoint(75f, 36f),
				true
			}
		};
	}
}
