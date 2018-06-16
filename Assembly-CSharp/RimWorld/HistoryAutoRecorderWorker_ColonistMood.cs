using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FB RID: 763
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x00070218 File Offset: 0x0006E618
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
