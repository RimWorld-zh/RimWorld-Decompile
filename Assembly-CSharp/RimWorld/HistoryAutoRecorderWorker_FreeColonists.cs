using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FE RID: 766
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC3 RID: 3267 RVA: 0x000704A4 File Offset: 0x0006E8A4
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
		}
	}
}
