using Verse;

namespace RimWorld
{
	public static class WardenFeedUtility
	{
		public static bool ShouldBeFed(Pawn p)
		{
			return (byte)(p.IsPrisonerOfColony ? (p.InBed() ? (p.guest.CanBeBroughtFood ? (HealthAIUtility.ShouldSeekMedicalRest(p) ? 1 : 0) : 0) : 0) : 0) != 0;
		}
	}
}
