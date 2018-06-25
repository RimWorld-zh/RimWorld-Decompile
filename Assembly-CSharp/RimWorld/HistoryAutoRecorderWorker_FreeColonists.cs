using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FE RID: 766
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC2 RID: 3266 RVA: 0x000704AC File Offset: 0x0006E8AC
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
		}
	}
}
