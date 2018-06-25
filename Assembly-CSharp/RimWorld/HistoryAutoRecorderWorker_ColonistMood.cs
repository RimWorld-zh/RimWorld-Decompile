using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FD RID: 765
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC0 RID: 3264 RVA: 0x0007041C File Offset: 0x0006E81C
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
