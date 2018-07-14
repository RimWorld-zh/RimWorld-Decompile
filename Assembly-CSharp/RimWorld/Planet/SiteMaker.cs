using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class SiteMaker
	{
		public static Site MakeSite(SiteCoreDef core, SitePartDef sitePart, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			IEnumerable<SitePartDef> siteParts = (sitePart == null) ? null : Gen.YieldSingle<SitePartDef>(sitePart);
			return SiteMaker.MakeSite(core, siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		public static Site MakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.Tile = tile;
			site.SetFaction(faction);
			if (ifHostileThenMustRemainHostile && faction != null && faction.HostileTo(Faction.OfPlayer))
			{
				site.factionMustRemainHostile = true;
			}
			float num = (threatPoints == null) ? StorytellerUtility.DefaultSiteThreatPointsNow() : threatPoints.Value;
			site.desiredThreatPoints = num;
			int num2 = 0;
			if (core.wantsThreatPoints)
			{
				num2++;
			}
			if (siteParts != null)
			{
				foreach (SitePartDef sitePartDef in siteParts)
				{
					if (sitePartDef.wantsThreatPoints)
					{
						num2++;
					}
				}
			}
			float num3 = (num2 == 0) ? 0f : (num / (float)num2);
			float myThreatPoints = (!core.wantsThreatPoints) ? 0f : num3;
			site.core = new SiteCore(core, core.Worker.GenerateDefaultParams(site, myThreatPoints));
			if (siteParts != null)
			{
				foreach (SitePartDef sitePartDef2 in siteParts)
				{
					float myThreatPoints2 = (!sitePartDef2.wantsThreatPoints) ? 0f : num3;
					site.parts.Add(new SitePart(sitePartDef2, sitePartDef2.Worker.GenerateDefaultParams(site, myThreatPoints2)));
				}
			}
			return site;
		}

		public static Site TryMakeSite_SingleSitePart(SiteCoreDef core, IEnumerable<SitePartDef> singleSitePartCandidates, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			Site result;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = null;
			}
			else
			{
				result = SiteMaker.MakeSite(core, sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
			}
			return result;
		}

		public static Site TryMakeSite_SingleSitePart(SiteCoreDef core, string singleSitePartTag, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			Site result;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartTag, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = null;
			}
			else
			{
				result = SiteMaker.MakeSite(core, sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
			}
			return result;
		}

		public static Site TryMakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, int tile, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			Faction faction;
			Site result;
			if (!SiteMakerHelper.TryFindRandomFactionFor(core, siteParts, out faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = null;
			}
			else
			{
				result = SiteMaker.MakeSite(core, siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
			}
			return result;
		}
	}
}
