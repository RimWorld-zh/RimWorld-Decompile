using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class SiteMaker
	{
		public static Site MakeSite(SiteCoreDef core, SitePartDef sitePart, Faction faction)
		{
			IEnumerable<SitePartDef> siteParts = (sitePart == null) ? null : Gen.YieldSingle(sitePart);
			return SiteMaker.MakeSite(core, siteParts, faction);
		}

		public static Site MakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, Faction faction)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.core = core;
			site.SetFaction(faction);
			if (siteParts != null)
			{
				site.parts.AddRange(siteParts);
			}
			return site;
		}

		public static Site TryMakeSite_SingleSitePart(SiteCoreDef core, IEnumerable<SitePartDef> singleSitePartCandidates, Faction faction = null, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			SitePartDef sitePart = default(SitePartDef);
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, faction, disallowAlliedFactions, extraFactionValidator) ? SiteMaker.MakeSite(core, sitePart, faction) : null;
		}

		public static Site TryMakeSite_SingleSitePart(SiteCoreDef core, string singleSitePartTag, Faction faction = null, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			SitePartDef sitePart = default(SitePartDef);
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartTag, out sitePart, out faction, faction, disallowAlliedFactions, extraFactionValidator) ? SiteMaker.MakeSite(core, sitePart, faction) : null;
		}

		public static Site TryMakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			Faction faction = default(Faction);
			return SiteMakerHelper.TryFindRandomFactionFor(core, siteParts, out faction, disallowAlliedFactions, extraFactionValidator) ? SiteMaker.MakeSite(core, siteParts, faction) : null;
		}
	}
}
