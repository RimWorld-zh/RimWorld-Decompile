using System;
using System.Collections.Generic;
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

		private Pawn Partner
		{
			get
			{
				return (Pawn)(Thing)base.job.GetTarget(this.PartnerInd);
			}
		}

		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)(Thing)base.job.GetTarget(this.BedInd);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Partner, base.job, 1, -1, null) && base.pawn.Reserve((Thing)this.Bed, base.job, this.Bed.SleepingSlotsCount, 0, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0062: stateMachine*/)._0024this.Partner.health.capacities.CanBeAwake));
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			/*Error: Unable to find new state assignment for yield return*/;
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
				float centerX = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
				centerX = Rand.Gaussian(centerX, 0.3f);
				if (centerX < 0.5)
				{
					centerX = 0.5f;
				}
				result = (int)(centerX * 2500.0);
			}
			return result;
		}
	}
}
