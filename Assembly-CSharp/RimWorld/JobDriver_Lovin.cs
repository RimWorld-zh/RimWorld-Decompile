using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return (Pawn)((Thing)base.CurJob.GetTarget(this.PartnerInd));
			}
		}

		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)base.CurJob.GetTarget(this.BedInd));
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Lovin.<MakeNewToils>c__Iterator32 <MakeNewToils>c__Iterator = new JobDriver_Lovin.<MakeNewToils>c__Iterator32();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Lovin.<MakeNewToils>c__Iterator32 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float num = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			num = Rand.Gaussian(num, 0.3f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return (int)(num * 2500f);
		}
	}
}
