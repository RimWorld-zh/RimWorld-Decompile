using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		public HistoryAutoRecorderWorker_ColonistMood()
		{
		}

		public override float PullRecord()
		{
			IEnumerable<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			float result;
			if (!allMaps_FreeColonists.Any<Pawn>())
			{
				result = 0f;
			}
			else
			{
				result = allMaps_FreeColonists.Average((Pawn x) => x.needs.mood.CurLevel * 100f);
			}
			return result;
		}

		[CompilerGenerated]
		private static float <PullRecord>m__0(Pawn x)
		{
			return x.needs.mood.CurLevel * 100f;
		}
	}
}
