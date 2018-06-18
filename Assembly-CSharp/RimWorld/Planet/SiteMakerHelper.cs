using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200060C RID: 1548
	public static class SiteMakerHelper
	{
		// Token: 0x06001F27 RID: 7975 RVA: 0x0010E608 File Offset: 0x0010CA08
		public static bool TryFindSiteParams_SingleSitePart(SiteCoreDef core, IEnumerable<SitePartDef> singleSitePartCandidates, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			faction = factionToUse;
			if (singleSitePartCandidates != null)
			{
				if (!SiteMakerHelper.TryFindNewRandomSitePartFor(core, null, singleSitePartCandidates, faction, out sitePart, disallowNonHostileFactions, extraFactionValidator))
				{
					return false;
				}
			}
			else
			{
				sitePart = null;
			}
			if (faction == null)
			{
				IEnumerable<SitePartDef> parts = (sitePart == null) ? null : Gen.YieldSingle<SitePartDef>(sitePart);
				if (!SiteMakerHelper.TryFindRandomFactionFor(core, parts, out faction, disallowNonHostileFactions, extraFactionValidator))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0010E684 File Offset: 0x0010CA84
		public static bool TryFindSiteParams_SingleSitePart(SiteCoreDef core, string singleSitePartTag, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			IEnumerable<SitePartDef> singleSitePartCandidates = (singleSitePartTag == null) ? null : (from x in DefDatabase<SitePartDef>.AllDefsListForReading
			where x.tags.Contains(singleSitePartTag)
			select x);
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, factionToUse, disallowNonHostileFactions, extraFactionValidator);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0010E6E0 File Offset: 0x0010CAE0
		public static bool TryFindNewRandomSitePartFor(SiteCoreDef core, IEnumerable<SitePartDef> existingSiteParts, IEnumerable<SitePartDef> possibleSiteParts, Faction faction, out SitePartDef sitePart, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			if (faction != null)
			{
				if ((from x in possibleSiteParts
				where x == null || SiteMakerHelper.FactionCanOwn(x, faction, disallowNonHostileFactions, extraFactionValidator)
				select x).TryRandomElement(out sitePart))
				{
					return true;
				}
			}
			else
			{
				SiteMakerHelper.possibleFactions.Clear();
				SiteMakerHelper.possibleFactions.Add(null);
				SiteMakerHelper.possibleFactions.AddRange(Find.FactionManager.AllFactionsListForReading);
				if ((from x in possibleSiteParts
				where x == null || SiteMakerHelper.possibleFactions.Any((Faction fac) => SiteMakerHelper.FactionCanOwn(core, existingSiteParts, fac, disallowNonHostileFactions, extraFactionValidator) && SiteMakerHelper.FactionCanOwn(x, fac, disallowNonHostileFactions, extraFactionValidator))
				select x).TryRandomElement(out sitePart))
				{
					SiteMakerHelper.possibleFactions.Clear();
					return true;
				}
				SiteMakerHelper.possibleFactions.Clear();
			}
			sitePart = null;
			return false;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0010E7C4 File Offset: 0x0010CBC4
		public static bool TryFindRandomFactionFor(SiteCoreDef core, IEnumerable<SitePartDef> parts, out Faction faction, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			bool result;
			if (SiteMakerHelper.FactionCanOwn(core, parts, null, disallowNonHostileFactions, extraFactionValidator))
			{
				faction = null;
				result = true;
			}
			else if ((from x in Find.FactionManager.AllFactionsListForReading
			where SiteMakerHelper.FactionCanOwn(core, parts, x, disallowNonHostileFactions, extraFactionValidator)
			select x).TryRandomElement(out faction))
			{
				result = true;
			}
			else
			{
				faction = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0010E85C File Offset: 0x0010CC5C
		public static bool FactionCanOwn(SiteCoreDef core, IEnumerable<SitePartDef> parts, Faction faction, bool disallowNonHostileFactions, Predicate<Faction> extraFactionValidator)
		{
			bool result;
			if (!SiteMakerHelper.FactionCanOwn(core, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				result = false;
			}
			else
			{
				if (parts != null)
				{
					foreach (SitePartDef siteDefBase in parts)
					{
						if (!SiteMakerHelper.FactionCanOwn(siteDefBase, faction, disallowNonHostileFactions, extraFactionValidator))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0010E8EC File Offset: 0x0010CCEC
		private static bool FactionCanOwn(SiteDefBase siteDefBase, Faction faction, bool disallowNonHostileFactions, Predicate<Faction> extraFactionValidator)
		{
			bool result;
			if (siteDefBase == null)
			{
				Log.Error("Called FactionCanOwn() with null SiteDefBase.", false);
				result = false;
			}
			else
			{
				result = (siteDefBase.FactionCanOwn(faction) && (!disallowNonHostileFactions || faction == null || faction.HostileTo(Faction.OfPlayer)) && (extraFactionValidator == null || extraFactionValidator(faction)));
			}
			return result;
		}

		// Token: 0x0400123E RID: 4670
		private static List<Faction> possibleFactions = new List<Faction>();
	}
}
