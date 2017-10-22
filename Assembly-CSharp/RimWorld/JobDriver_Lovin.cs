using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Lovin : JobDriver
	{
		private const int TicksBetweenHeartMotes = 100;

		private int ticksLeft;

		private TargetIndex PartnerInd = TargetIndex.A;

		private TargetIndex BedInd = TargetIndex.B;

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

		private Pawn Partner
		{
			get
			{
				return (Pawn)(Thing)base.CurJob.GetTarget(this.PartnerInd);
			}
		}

		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)(Thing)base.CurJob.GetTarget(this.BedInd);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0069: stateMachine*/)._003C_003Ef__this.Partner.health.capacities.CanBeAwake));
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Reserve.Reserve(this.PartnerInd, 1, -1, null);
			yield return Toils_Reserve.Reserve(this.BedInd, this.Bed.SleepingSlotsCount, 0, null);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(this.BedInd);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.Partner.CurJob == null || ((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.Partner.CurJob.def != JobDefOf.Lovin)
					{
						Job newJob = new Job(JobDefOf.Lovin, (Thing)((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.pawn, (Thing)((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.Bed);
						((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, default(JobTag?));
						((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.ticksLeft = (int)(2500.0 * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
					}
					else
					{
						((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0140: stateMachine*/)._003C_003Ef__this.ticksLeft = 9999999;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil doLovin = Toils_LayDown.LayDown(this.BedInd, true, false, false, false);
			doLovin.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0195: stateMachine*/)._003C_003Ef__this.Partner.CurJob == null || ((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_0195: stateMachine*/)._003C_003Ef__this.Partner.CurJob.def != JobDefOf.Lovin));
			doLovin.AddPreTickAction((Action)delegate
			{
				((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01ad: stateMachine*/)._003C_003Ef__this.ticksLeft--;
				if (((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01ad: stateMachine*/)._003C_003Ef__this.ticksLeft <= 0)
				{
					((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01ad: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
				}
				else if (((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01ad: stateMachine*/)._003C_003Ef__this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01ad: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01ad: stateMachine*/)._003C_003Ef__this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			});
			doLovin.AddFinishAction((Action)delegate
			{
				Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01c4: stateMachine*/)._003C_003Ef__this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, ((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01c4: stateMachine*/)._003C_003Ef__this.Partner);
				((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01c4: stateMachine*/)._003C_003Ef__this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + ((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01c4: stateMachine*/)._003C_003Ef__this.GenerateRandomMinTicksToNextLovin(((_003CMakeNewToils_003Ec__Iterator32)/*Error near IL_01c4: stateMachine*/)._003C_003Ef__this.pawn);
			});
			doLovin.socialMode = RandomSocialMode.Off;
			yield return doLovin;
		}

		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float centerX = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			centerX = Rand.Gaussian(centerX, 0.3f);
			if (centerX < 0.5)
			{
				centerX = 0.5f;
			}
			return (int)(centerX * 2500.0);
		}
	}
}
