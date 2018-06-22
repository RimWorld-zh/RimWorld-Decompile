using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200060B RID: 1547
	public class SitePartWorker_SleepingMechanoids : SitePartWorker
	{
		// Token: 0x06001F2C RID: 7980 RVA: 0x0010EC74 File Offset: 0x0010D074
		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			IEnumerable<Pawn> source = from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.IsMechanoid
			select x;
			Pawn pawn = (from x in source
			where x.GetLord() != null && x.GetLord().LordJob is LordJob_SleepThenAssaultColony
			select x).FirstOrDefault<Pawn>();
			if (pawn == null)
			{
				pawn = source.FirstOrDefault<Pawn>();
			}
			lookTargets = pawn;
			return arrivedLetterPart;
		}
	}
}
