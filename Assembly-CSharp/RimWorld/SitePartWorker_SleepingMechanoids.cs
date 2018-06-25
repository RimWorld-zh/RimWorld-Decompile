using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class SitePartWorker_SleepingMechanoids : SitePartWorker
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		public SitePartWorker_SleepingMechanoids()
		{
		}

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

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__0(Pawn x)
		{
			return x.RaceProps.IsMechanoid;
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__1(Pawn x)
		{
			return x.GetLord() != null && x.GetLord().LordJob is LordJob_SleepThenAssaultColony;
		}
	}
}
