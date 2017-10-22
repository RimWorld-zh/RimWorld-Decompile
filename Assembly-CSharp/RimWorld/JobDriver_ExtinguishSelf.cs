using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		protected const int NumSpeechesToSay = 5;

		protected Fire TargetFire
		{
			get
			{
				return (Fire)base.CurJob.targetA.Thing;
			}
		}

		public override string GetReport()
		{
			if (this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap);
			}
			return "ReportExtinguishingFire".Translate();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = (Action)delegate
				{
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 150
			};
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (!((_003CMakeNewToils_003Ec__Iterator15)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.TargetFire.Destroyed)
					{
						((_003CMakeNewToils_003Ec__Iterator15)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.TargetFire.Destroy(DestroyMode.Vanish);
						((_003CMakeNewToils_003Ec__Iterator15)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
