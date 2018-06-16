using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200005B RID: 91
	public class JobDriver_Skygaze : JobDriver
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0001CDAC File Offset: 0x0001B1AC
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0001CDC4 File Offset: 0x0001B1C4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil gaze = new Toil();
			gaze.initAction = delegate()
			{
				this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
			};
			gaze.tickAction = delegate()
			{
				float num = this.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(this.pawn.Map);
				Pawn pawn = this.pawn;
				float extraJoyGainFactor = num;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, null);
			};
			gaze.defaultCompleteMode = ToilCompleteMode.Delay;
			gaze.defaultDuration = this.job.def.joyDuration;
			gaze.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
			gaze.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			yield return gaze;
			yield break;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0001CDF0 File Offset: 0x0001B1F0
		public override string GetReport()
		{
			string result;
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				result = "WatchingEclipse".Translate();
			}
			else if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
			{
				result = "WatchingAurora".Translate();
			}
			else
			{
				float num = GenCelestial.CurCelestialSunGlow(base.Map);
				if (num < 0.1f)
				{
					result = "Stargazing".Translate();
				}
				else if (num < 0.65f)
				{
					if (GenLocalDate.DayPercent(this.pawn) < 0.5f)
					{
						result = "WatchingSunrise".Translate();
					}
					else
					{
						result = "WatchingSunset".Translate();
					}
				}
				else
				{
					result = "CloudWatching".Translate();
				}
			}
			return result;
		}
	}
}
