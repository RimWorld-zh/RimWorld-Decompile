using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public SitePartWorker_Manhunters()
		{
		}

		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			lookTargets = (from x in map.mapPawns.AllPawnsSpawned
			where x.MentalStateDef == MentalStateDefOf.Manhunter || x.MentalStateDef == MentalStateDefOf.ManhunterPermanent
			select x).FirstOrDefault<Pawn>();
			return arrivedLetterPart;
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__0(Pawn x)
		{
			return x.MentalStateDef == MentalStateDefOf.Manhunter || x.MentalStateDef == MentalStateDefOf.ManhunterPermanent;
		}
	}
}
