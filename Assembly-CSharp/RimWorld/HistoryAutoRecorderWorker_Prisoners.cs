using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FD RID: 765
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x000702C8 File Offset: 0x0006E6C8
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
