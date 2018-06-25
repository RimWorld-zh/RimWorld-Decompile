using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class SitePartWorker_Outpost : SitePartWorker
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public SitePartWorker_Outpost()
		{
		}

		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			lookTargets = (from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike && x.HostileTo(Faction.OfPlayer)
			select x).FirstOrDefault<Pawn>();
			return arrivedLetterPart;
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__0(Pawn x)
		{
			return x.RaceProps.Humanlike && x.HostileTo(Faction.OfPlayer);
		}
	}
}
