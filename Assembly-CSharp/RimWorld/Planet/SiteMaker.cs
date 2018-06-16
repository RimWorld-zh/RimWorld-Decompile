using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200060B RID: 1547
	public static class SiteMaker
	{
		// Token: 0x06001F20 RID: 7968 RVA: 0x0010E440 File Offset: 0x0010C840
		public static Site MakeSite(SiteCoreDef core, SitePartDef sitePart, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			IEnumerable<SitePartDef> siteParts = (sitePart == null) ? null : Gen.YieldSingle<SitePartDef>(sitePart);
			return SiteMaker.MakeSite(core, siteParts, faction, ifHostileThenMustRemainHostile);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0010E474 File Offset: 0x0010C874
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

		// Token: 0x06001F22 RID: 7970 RVA: 0x0010E4E0 File Offset: 0x0010C8E0
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

		// Token: 0x06001F23 RID: 7971 RVA: 0x0010E51C File Offset: 0x0010C91C
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

		// Token: 0x06001F24 RID: 7972 RVA: 0x0010E558 File Offset: 0x0010C958
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
