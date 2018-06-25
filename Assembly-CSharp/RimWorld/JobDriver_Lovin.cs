using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Lovin : JobDriver
	{
		private int ticksLeft = 0;

		private TargetIndex PartnerInd = TargetIndex.A;

		private TargetIndex BedInd = TargetIndex.B;

		private const int TicksBetweenHeartMotes = 100;

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

		public JobDriver_Lovin()
		{
		}

		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null);
		}

		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static JobDriver_Lovin()
		{
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <preparePartner>__0;

			internal Toil <doLovin>__0;

			internal JobDriver_Lovin $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.FailOnDespawnedOrNull(this.BedInd);
					this.FailOnDespawnedOrNull(this.PartnerInd);
					this.FailOn(() => !base.Partner.health.capacities.CanBeAwake);
					this.KeepLyingDown(this.BedInd);
					this.$current = Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Bed.GotoBed(this.BedInd);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil preparePartner = new Toil();
					preparePartner.initAction = delegate()
					{
						if (base.Partner.CurJob == null || base.Partner.CurJob.def != JobDefOf.Lovin)
						{
							Job newJob = new Job(JobDefOf.Lovin, this.pawn, base.Bed);
							base.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);
							this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
						}
						else
						{
							this.ticksLeft = 9999999;
						}
					};
					preparePartner.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = preparePartner;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
					doLovin = Toils_LayDown.LayDown(this.BedInd, true, false, false, false);
					doLovin.FailOn(() => base.Partner.CurJob == null || base.Partner.CurJob.def != JobDefOf.Lovin);
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
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, base.Partner);
						this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + base.GenerateRandomMinTicksToNextLovin(this.pawn);
					});
					doLovin.socialMode = RandomSocialMode.Off;
					this.$current = doLovin;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_Lovin.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Lovin.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Partner.health.capacities.CanBeAwake;
			}

			internal void <>m__1()
			{
				if (base.Partner.CurJob == null || base.Partner.CurJob.def != JobDefOf.Lovin)
				{
					Job newJob = new Job(JobDefOf.Lovin, this.pawn, base.Bed);
					base.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);
					this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
				}
				else
				{
					this.ticksLeft = 9999999;
				}
			}

			internal bool <>m__2()
			{
				return base.Partner.CurJob == null || base.Partner.CurJob.def != JobDefOf.Lovin;
			}

			internal void <>m__3()
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
			}

			internal void <>m__4()
			{
				Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, base.Partner);
				this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + base.GenerateRandomMinTicksToNextLovin(this.pawn);
			}
		}
	}
}
