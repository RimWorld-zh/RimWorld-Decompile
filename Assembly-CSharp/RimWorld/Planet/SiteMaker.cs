using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000607 RID: 1543
	public static class SiteMaker
	{
		// Token: 0x06001F19 RID: 7961 RVA: 0x0010E50C File Offset: 0x0010C90C
		public static Site MakeSite(SiteCoreDef core, SitePartDef sitePart, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			IEnumerable<SitePartDef> siteParts = (sitePart == null) ? null : Gen.YieldSingle<SitePartDef>(sitePart);
			return SiteMaker.MakeSite(core, siteParts, faction, ifHostileThenMustRemainHostile);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0010E540 File Offset: 0x0010C940
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

		// Token: 0x06001F1B RID: 7963 RVA: 0x0010E5AC File Offset: 0x0010C9AC
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

		// Token: 0x06001F1C RID: 7964 RVA: 0x0010E5E8 File Offset: 0x0010C9E8
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

		// Token: 0x06001F1D RID: 7965 RVA: 0x0010E624 File Offset: 0x0010CA24
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
