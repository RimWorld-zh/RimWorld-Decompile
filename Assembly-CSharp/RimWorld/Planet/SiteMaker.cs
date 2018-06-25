using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000609 RID: 1545
	public static class SiteMaker
	{
		// Token: 0x06001F1D RID: 7965 RVA: 0x0010E65C File Offset: 0x0010CA5C
		public static Site MakeSite(SiteCoreDef core, SitePartDef sitePart, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			IEnumerable<SitePartDef> siteParts = (sitePart == null) ? null : Gen.YieldSingle<SitePartDef>(sitePart);
			return SiteMaker.MakeSite(core, siteParts, faction, ifHostileThenMustRemainHostile);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0010E690 File Offset: 0x0010CA90
		public static Site MakeSite(SiteCoreDef core, IEnumerable<SitePartDef> siteParts, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.core = core;
			site.SetFaction(faction);
			if (siteParts != null)
			{
				site.parts.AddRange(siteParts);
			}
			if (ifHostileThenMustRemainHostile && faction != null && faction.HostileTo(Faction.OfPlayer))
			{
				site.factionMustRemainHostile = true;
			}
			return site;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0010E6FC File Offset: 0x0010CAFC
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

		// Token: 0x06001F20 RID: 7968 RVA: 0x0010E738 File Offset: 0x0010CB38
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

		// Token: 0x06001F21 RID: 7969 RVA: 0x0010E774 File Offset: 0x0010CB74
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
