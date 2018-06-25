using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200060B RID: 1547
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x06001F2A RID: 7978 RVA: 0x0010EC94 File Offset: 0x0010D094
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
