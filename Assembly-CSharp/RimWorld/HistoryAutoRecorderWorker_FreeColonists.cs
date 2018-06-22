using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FC RID: 764
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CBF RID: 3263 RVA: 0x00070354 File Offset: 0x0006E754
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
		}
	}
}
