using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Skygaze : JobDriver
	{
		private Toil gaze;

		public override PawnPosture Posture
		{
			get
			{
				return (PawnPosture)((base.CurToil == this.gaze) ? 1 : 0);
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			this.gaze = new Toil();
			this.gaze.tickAction = (Action)delegate
			{
				JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator20)/*Error near IL_0059: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.EndJob, 1f);
			};
			this.gaze.defaultCompleteMode = ToilCompleteMode.Delay;
			this.gaze.defaultDuration = base.CurJob.def.joyDuration;
			this.gaze.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator20)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.pawn.Position.Roofed(((_003CMakeNewToils_003Ec__Iterator20)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.pawn.Map)));
			this.gaze.FailOn((Func<bool>)(() => !JoyUtility.EnjoyableOutsideNow(((_003CMakeNewToils_003Ec__Iterator20)/*Error near IL_00c8: stateMachine*/)._003C_003Ef__this.pawn, null)));
			yield return this.gaze;
		}

		public override string GetReport()
		{
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				return "WatchingEclipse".Translate();
			}
			float num = GenCelestial.CurCelestialSunGlow(base.Map);
			if (num < 0.10000000149011612)
			{
				return "Stargazing".Translate();
			}
			if (num < 0.64999997615814209)
			{
				if (GenLocalDate.DayPercent(base.pawn) < 0.5)
				{
					return "WatchingSunrise".Translate();
				}
				return "WatchingSunset".Translate();
			}
			return "CloudWatching".Translate();
		}
	}
}
