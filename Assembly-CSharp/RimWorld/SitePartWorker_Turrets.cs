using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class SitePartWorker_Turrets : SitePartWorker
	{
		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache1;

		public SitePartWorker_Turrets()
		{
		}

		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			Thing t;
			if ((t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun && x.HostileTo(Faction.OfPlayer))) == null)
			{
				t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun);
			}
			lookTargets = t;
			return arrivedLetterPart;
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__0(Thing x)
		{
			return x is Building_TurretGun && x.HostileTo(Faction.OfPlayer);
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__1(Thing x)
		{
			return x is Building_TurretGun;
		}
	}
}
