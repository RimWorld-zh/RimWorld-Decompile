using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public static class SiteMakerHelper
	{
		private static List<Faction> possibleFactions = new List<Faction>();

		public static bool TryFindSiteParams_SingleSitePart(SiteCoreDef core, IEnumerable<SitePartDef> singleSitePartCandidates, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			faction = factionToUse;
			bool result;
			if (singleSitePartCandidates != null)
			{
				if (!SiteMakerHelper.TryFindNewRandomSitePartFor(core, (IEnumerable<SitePartDef>)null, singleSitePartCandidates, faction, out sitePart, disallowAlliedFactions, extraFactionValidator))
				{
					result = false;
					goto IL_006e;
				}
			}
			else
			{
				sitePart = null;
			}
			if (faction == null)
			{
				IEnumerable<SitePartDef> parts = (sitePart == null) ? null : Gen.YieldSingle(sitePart);
				if (!SiteMakerHelper.TryFindRandomFactionFor(core, parts, out faction, disallowAlliedFactions, extraFactionValidator))
				{
					result = false;
					goto IL_006e;
				}
			}
			result = true;
			goto IL_006e;
			IL_006e:
			return result;
		}

		public static bool TryFindSiteParams_SingleSitePart(SiteCoreDef core, string singleSitePartTag, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			IEnumerable<SitePartDef> singleSitePartCandidates = (singleSitePartTag == null) ? null : (from x in DefDatabase<SitePartDef>.AllDefsListForReading
			where x.tags.Contains(singleSitePartTag)
			select x);
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, factionToUse, disallowAlliedFactions, extraFactionValidator);
		}

		public static bool TryFindNewRandomSitePartFor(SiteCoreDef core, IEnumerable<SitePartDef> existingSiteParts, IEnumerable<SitePartDef> possibleSiteParts, Faction faction, out SitePartDef sitePart, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			bool result;
			if (faction != null)
			{
				if ((from x in possibleSiteParts
				where x == null || SiteMakerHelper.FactionCanOwn(x, faction, disallowAlliedFactions, extraFactionValidator)
				select x).TryRandomElement<SitePartDef>(out sitePart))
				{
					result = true;
					goto IL_00d4;
				}
			}
			else
			{
				SiteMakerHelper.possibleFactions.Clear();
				SiteMakerHelper.possibleFactions.Add(null);
				SiteMakerHelper.possibleFactions.AddRange(Find.FactionManager.AllFactionsListForReading);
				if ((from x in possibleSiteParts
				where x == null || SiteMakerHelper.possibleFactions.Any((Predicate<Faction>)((Faction fac) => SiteMakerHelper.FactionCanOwn(core, existingSiteParts, fac, disallowAlliedFactions, extraFactionValidator) && SiteMakerHelper.FactionCanOwn(x, fac, disallowAlliedFactions, extraFactionValidator)))
				select x).TryRandomElement<SitePartDef>(out sitePart))
				{
					SiteMakerHelper.possibleFactions.Clear();
					result = true;
					goto IL_00d4;
				}
				SiteMakerHelper.possibleFactions.Clear();
			}
			sitePart = null;
			result = false;
			goto IL_00d4;
			IL_00d4:
			return result;
		}

		public static bool TryFindRandomFactionFor(SiteCoreDef core, IEnumerable<SitePartDef> parts, out Faction faction, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			bool result;
			if (SiteMakerHelper.FactionCanOwn(core, parts, null, disallowAlliedFactions, extraFactionValidator))
			{
				faction = null;
				result = true;
			}
			else if ((from x in Find.FactionManager.AllFactionsListForReading
			where SiteMakerHelper.FactionCanOwn(core, parts, x, disallowAlliedFactions, extraFactionValidator)
			select x).TryRandomElement<Faction>(out faction))
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

		public static bool FactionCanOwn(SiteCoreDef core, IEnumerable<SitePartDef> parts, Faction faction, bool disallowAlliedFactions, Predicate<Faction> extraFactionValidator)
		{
			bool result;
			if (!SiteMakerHelper.FactionCanOwn(core, faction, disallowAlliedFactions, extraFactionValidator))
			{
				result = false;
			}
			else
			{
				if (parts != null)
				{
					foreach (SitePartDef item in parts)
					{
						if (!SiteMakerHelper.FactionCanOwn(item, faction, disallowAlliedFactions, extraFactionValidator))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		private static bool FactionCanOwn(SiteDefBase siteDefBase, Faction faction, bool disallowAlliedFactions, Predicate<Faction> extraFactionValidator)
		{
			bool result;
			if (siteDefBase == null)
			{
				Log.Error("Called FactionCanOwn() with null SiteDefBase.");
				result = false;
			}
			else
			{
				result = ((byte)(siteDefBase.FactionCanOwn(faction) ? ((!disallowAlliedFactions || faction == null || faction.HostileTo(Faction.OfPlayer)) ? (((object)extraFactionValidator == null || extraFactionValidator(faction)) ? 1 : 0) : 0) : 0) != 0);
			}
			return result;
		}
	}
}
