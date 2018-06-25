using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public static class SiteMakerHelper
	{
		private static List<Faction> possibleFactions = new List<Faction>();

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

		public static bool TryFindSiteParams_SingleSitePart(SiteCoreDef core, string singleSitePartTag, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			IEnumerable<SitePartDef> singleSitePartCandidates = (singleSitePartTag == null) ? null : (from x in DefDatabase<SitePartDef>.AllDefsListForReading
			where x.tags.Contains(singleSitePartTag)
			select x);
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(core, singleSitePartCandidates, out sitePart, out faction, factionToUse, disallowNonHostileFactions, extraFactionValidator);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static SiteMakerHelper()
		{
		}

		[CompilerGenerated]
		private sealed class <TryFindSiteParams_SingleSitePart>c__AnonStorey0
		{
			internal string singleSitePartTag;

			public <TryFindSiteParams_SingleSitePart>c__AnonStorey0()
			{
			}

			internal bool <>m__0(SitePartDef x)
			{
				return x.tags.Contains(this.singleSitePartTag);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindNewRandomSitePartFor>c__AnonStorey1
		{
			internal Faction faction;

			internal bool disallowNonHostileFactions;

			internal Predicate<Faction> extraFactionValidator;

			internal SiteCoreDef core;

			internal IEnumerable<SitePartDef> existingSiteParts;

			public <TryFindNewRandomSitePartFor>c__AnonStorey1()
			{
			}

			internal bool <>m__0(SitePartDef x)
			{
				return x == null || SiteMakerHelper.FactionCanOwn(x, this.faction, this.disallowNonHostileFactions, this.extraFactionValidator);
			}

			internal bool <>m__1(SitePartDef x)
			{
				return x == null || SiteMakerHelper.possibleFactions.Any((Faction fac) => SiteMakerHelper.FactionCanOwn(this.core, this.existingSiteParts, fac, this.disallowNonHostileFactions, this.extraFactionValidator) && SiteMakerHelper.FactionCanOwn(x, fac, this.disallowNonHostileFactions, this.extraFactionValidator));
			}

			private sealed class <TryFindNewRandomSitePartFor>c__AnonStorey2
			{
				internal SitePartDef x;

				internal SiteMakerHelper.<TryFindNewRandomSitePartFor>c__AnonStorey1 <>f__ref$1;

				public <TryFindNewRandomSitePartFor>c__AnonStorey2()
				{
				}

				internal bool <>m__0(Faction fac)
				{
					return SiteMakerHelper.FactionCanOwn(this.<>f__ref$1.core, this.<>f__ref$1.existingSiteParts, fac, this.<>f__ref$1.disallowNonHostileFactions, this.<>f__ref$1.extraFactionValidator) && SiteMakerHelper.FactionCanOwn(this.x, fac, this.<>f__ref$1.disallowNonHostileFactions, this.<>f__ref$1.extraFactionValidator);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomFactionFor>c__AnonStorey3
		{
			internal SiteCoreDef core;

			internal IEnumerable<SitePartDef> parts;

			internal bool disallowNonHostileFactions;

			internal Predicate<Faction> extraFactionValidator;

			public <TryFindRandomFactionFor>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return SiteMakerHelper.FactionCanOwn(this.core, this.parts, x, this.disallowNonHostileFactions, this.extraFactionValidator);
			}
		}
	}
}
