using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public static class SiteMaker
	{
		private static List<Faction> possibleFactions = new List<Faction>();

		public static Site TryMakeRandomSite(SiteCoreDef core, IEnumerable<SitePartDef> possibleSiteParts, Faction faction = null, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			SitePartDef sitePartDef = null;
			if (possibleSiteParts != null)
			{
				SiteMaker.TryFindNewRandomSitePartFor(core, (IEnumerable<SitePartDef>)null, possibleSiteParts, faction, out sitePartDef, disallowAlliedFactions, extraFactionValidator);
			}
			if (faction == null)
			{
				object obj;
				if (sitePartDef != null)
				{
					IEnumerable<SitePartDef> enumerable = Gen.YieldSingle(sitePartDef);
					obj = enumerable;
				}
				else
				{
					obj = null;
				}
				IEnumerable<SitePartDef> parts = (IEnumerable<SitePartDef>)obj;
				if (!SiteMaker.TryFindRandomFactionFor(core, parts, out faction, disallowAlliedFactions, extraFactionValidator))
				{
					return null;
				}
			}
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.core = core;
			site.SetFaction(faction);
			if (sitePartDef != null)
			{
				site.parts.Add(sitePartDef);
			}
			return site;
		}

		public static Site TryMakeSite(SiteCoreDef core, IEnumerable<SitePartDef> parts, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			Faction faction = default(Faction);
			if (!SiteMaker.TryFindRandomFactionFor(core, parts, out faction, disallowAlliedFactions, extraFactionValidator))
			{
				return null;
			}
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.core = core;
			if (parts != null)
			{
				site.parts.AddRange(parts);
			}
			site.SetFaction(faction);
			return site;
		}

		public static bool TryFindNewRandomSitePartFor(SiteCoreDef core, IEnumerable<SitePartDef> existingSiteParts, IEnumerable<SitePartDef> possibleSiteParts, Faction faction, out SitePartDef sitePart, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			if (faction != null)
			{
				if ((from x in possibleSiteParts
				where x == null || SiteMaker.FactionCanOwn(x, faction, disallowAlliedFactions, extraFactionValidator)
				select x).TryRandomElement<SitePartDef>(out sitePart))
				{
					return true;
				}
			}
			else
			{
				SiteMaker.possibleFactions.Clear();
				SiteMaker.possibleFactions.Add(null);
				SiteMaker.possibleFactions.AddRange(Find.FactionManager.AllFactionsListForReading);
				if ((from x in possibleSiteParts
				where x == null || SiteMaker.possibleFactions.Any((Predicate<Faction>)((Faction fac) => SiteMaker.FactionCanOwn(core, existingSiteParts, fac, disallowAlliedFactions, extraFactionValidator) && SiteMaker.FactionCanOwn(x, fac, disallowAlliedFactions, extraFactionValidator)))
				select x).TryRandomElement<SitePartDef>(out sitePart))
				{
					SiteMaker.possibleFactions.Clear();
					return true;
				}
				SiteMaker.possibleFactions.Clear();
			}
			sitePart = null;
			return false;
		}

		public static bool TryFindRandomFactionFor(SiteCoreDef core, IEnumerable<SitePartDef> parts, out Faction faction, bool disallowAlliedFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			if (SiteMaker.FactionCanOwn(core, parts, null, disallowAlliedFactions, extraFactionValidator))
			{
				faction = null;
				return true;
			}
			if ((from x in Find.FactionManager.AllFactionsListForReading
			where SiteMaker.FactionCanOwn(core, parts, x, disallowAlliedFactions, extraFactionValidator)
			select x).TryRandomElement<Faction>(out faction))
			{
				return true;
			}
			faction = null;
			return false;
		}

		public static bool FactionCanOwn(SiteCoreDef core, IEnumerable<SitePartDef> parts, Faction faction, bool disallowAlliedFactions, Predicate<Faction> extraFactionValidator)
		{
			if (!SiteMaker.FactionCanOwn(core, faction, disallowAlliedFactions, extraFactionValidator))
			{
				return false;
			}
			if (parts != null)
			{
				foreach (SitePartDef item in parts)
				{
					if (!SiteMaker.FactionCanOwn(item, faction, disallowAlliedFactions, extraFactionValidator))
					{
						return false;
					}
				}
			}
			return true;
		}

		private static bool FactionCanOwn(SiteDefBase siteDefBase, Faction faction, bool disallowAlliedFactions, Predicate<Faction> extraFactionValidator)
		{
			if (siteDefBase == null)
			{
				Log.Error("Called FactionCanOwn() with null SiteDefBase.");
				return false;
			}
			if (!siteDefBase.FactionCanOwn(faction))
			{
				return false;
			}
			if (disallowAlliedFactions && faction != null && !faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if ((object)extraFactionValidator != null && !extraFactionValidator(faction))
			{
				return false;
			}
			return true;
		}
	}
}
