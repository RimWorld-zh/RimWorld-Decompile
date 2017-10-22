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

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			/*Error: Unable to find new state assignment for yield return*/;
		}

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
				result = ((!(num < 0.10000000149011612)) ? ((!(num < 0.64999997615814209)) ? "CloudWatching".Translate() : ((!(GenLocalDate.DayPercent(base.pawn) < 0.5)) ? "WatchingSunset".Translate() : "WatchingSunrise".Translate())) : "Stargazing".Translate());
			}
			return result;
		}
	}
}
