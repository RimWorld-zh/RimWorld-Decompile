using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000608 RID: 1544
	public static class SiteMakerHelper
	{
		// Token: 0x0400123B RID: 4667
		private static List<Faction> possibleFactions = new List<Faction>();

		// Token: 0x06001F1E RID: 7966 RVA: 0x0010E65C File Offset: 0x0010CA5C
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

		// Token: 0x06001F1F RID: 7967 RVA: 0x0010E6D8 File Offset: 0x0010CAD8
		public static bool TryFindSiteParams_SingleSitePart(SiteCoreDef core, string singleSitePartTag, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			IEnumerable<SitePartDef> singleSitePartCandidates = (singleSitePartTag == null) ? null : (from x in DefDatabase<SitePartDef>.AllDefsListForReading
			where x.tags.Contains(singleSitePartTag)
			select x);
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, factionToUse, disallowNonHostileFactions, extraFactionValidator);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0010E734 File Offset: 0x0010CB34
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

		// Token: 0x06001F21 RID: 7969 RVA: 0x0010E818 File Offset: 0x0010CC18
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

		// Token: 0x06001F22 RID: 7970 RVA: 0x0010E8B0 File Offset: 0x0010CCB0
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

		// Token: 0x06001F23 RID: 7971 RVA: 0x0010E940 File Offset: 0x0010CD40
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
	}
}
