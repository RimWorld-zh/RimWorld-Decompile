using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class SiteMaker
	{
		public static Site MakeSite(SiteCoreDef core, SitePartDef sitePart, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			IEnumerable<SitePartDef> siteParts = (sitePart == null) ? null : Gen.YieldSingle<SitePartDef>(sitePart);
			return SiteMaker.MakeSite(core, siteParts, faction, ifHostileThenMustRemainHostile);
		}

		public static Site MakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.core = new SiteCore(core, core.Worker.GenerateDefaultParams());
			site.SetFaction(faction);
			if (siteParts != null)
			{
				foreach (SitePartDef sitePartDef in siteParts)
				{
					site.parts.Add(new SitePart(sitePartDef, sitePartDef.Worker.GenerateDefaultParams()));
				}
			}
			if (ifHostileThenMustRemainHostile && faction != null && faction.HostileTo(Faction.OfPlayer))
			{
				site.factionMustRemainHostile = true;
			}
			return site;
		}

		public static Site TryMakeSite_SingleSitePart(SiteCoreDef core, IEnumerable<SitePartDef> singleSitePartCandidates, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true)
		{
			SitePartDef sitePart;
			Site result;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = null;
			}
			else
			{
				result = SiteMaker.MakeSite(core, sitePart, faction, ifHostileThenMustRemainHostile);
			}
			return result;
		}

		public static Site TryMakeSite_SingleSitePart(SiteCoreDef core, string singleSitePartTag, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true)
		{
			SitePartDef sitePart;
			Site result;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartTag, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = null;
			}
			else
			{
				result = SiteMaker.MakeSite(core, sitePart, faction, ifHostileThenMustRemainHostile);
			}
			return result;
		}

		public static Site TryMakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true)
		{
			Faction faction;
			Site result;
			if (!SiteMakerHelper.TryFindRandomFactionFor(core, siteParts, out faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = null;
			}
			else
			{
				result = SiteMaker.MakeSite(core, siteParts, faction, ifHostileThenMustRemainHostile);
			}
			return result;
		}
	}
}
