using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FF RID: 767
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC5 RID: 3269 RVA: 0x000704CC File Offset: 0x0006E8CC
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
