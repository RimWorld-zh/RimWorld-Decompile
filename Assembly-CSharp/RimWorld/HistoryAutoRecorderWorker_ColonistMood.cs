using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FD RID: 765
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CBF RID: 3263 RVA: 0x00070424 File Offset: 0x0006E824
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
	}
}
