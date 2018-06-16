using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000137 RID: 311
	public static class WardenFeedUtility
	{
		// Token: 0x0600065D RID: 1629 RVA: 0x00042808 File Offset: 0x00040C08
		public static bool ShouldBeFed(Pawn p)
		{
			return p.IsPrisonerOfColony && p.InBed() && p.guest.CanBeBroughtFood && HealthAIUtility.ShouldSeekMedicalRest(p);
		}
	}
}
