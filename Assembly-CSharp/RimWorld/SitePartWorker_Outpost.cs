using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200060A RID: 1546
	public class SitePartWorker_Outpost : SitePartWorker
	{
		// Token: 0x06001F29 RID: 7977 RVA: 0x0010EBDC File Offset: 0x0010CFDC
		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			lookTargets = (from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike && x.HostileTo(Faction.OfPlayer)
			select x).FirstOrDefault<Pawn>();
			return arrivedLetterPart;
		}
	}
}
