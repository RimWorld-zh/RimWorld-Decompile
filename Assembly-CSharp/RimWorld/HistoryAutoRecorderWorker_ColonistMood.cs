using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		public override float PullRecord()
		{
			IEnumerable<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			return (float)(allMaps_FreeColonists.Any() ? allMaps_FreeColonists.Average((Func<Pawn, float>)((Pawn x) => (float)(x.needs.mood.CurLevel * 100.0))) : 0.0);
		}
	}
}
