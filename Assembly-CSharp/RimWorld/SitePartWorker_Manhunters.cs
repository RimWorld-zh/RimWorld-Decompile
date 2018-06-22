using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000609 RID: 1545
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x06001F26 RID: 7974 RVA: 0x0010EB44 File Offset: 0x0010CF44
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
