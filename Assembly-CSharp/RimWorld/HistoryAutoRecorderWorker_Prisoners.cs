using System.Linq;

namespace RimWorld
{
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_PrisonersOfColony.Count();
		}
	}
}
