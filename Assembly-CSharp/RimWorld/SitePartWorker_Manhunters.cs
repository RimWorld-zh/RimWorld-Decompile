using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200060B RID: 1547
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x06001F29 RID: 7977 RVA: 0x0010EEFC File Offset: 0x0010D2FC
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
