using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FF RID: 767
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC4 RID: 3268 RVA: 0x000704D4 File Offset: 0x0006E8D4
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
