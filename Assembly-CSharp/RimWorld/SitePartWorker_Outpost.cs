using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200060E RID: 1550
	public class SitePartWorker_Outpost : SitePartWorker
	{
		// Token: 0x06001F32 RID: 7986 RVA: 0x0010EB88 File Offset: 0x0010CF88
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
