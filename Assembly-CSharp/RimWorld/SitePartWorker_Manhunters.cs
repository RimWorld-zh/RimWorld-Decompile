using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200060D RID: 1549
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x06001F2F RID: 7983 RVA: 0x0010EAF0 File Offset: 0x0010CEF0
		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			lookTargets = (from x in map.mapPawns.AllPawnsSpawned
			where x.MentalStateDef == MentalStateDefOf.Manhunter || x.MentalStateDef == MentalStateDefOf.ManhunterPermanent
			select x).FirstOrDefault<Pawn>();
			return arrivedLetterPart;
		}
	}
}
