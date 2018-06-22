using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FD RID: 765
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x0007037C File Offset: 0x0006E77C
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
